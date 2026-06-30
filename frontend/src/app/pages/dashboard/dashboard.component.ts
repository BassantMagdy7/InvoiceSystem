import { CommonModule } from '@angular/common';
import { Component,OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { InvoiceService } from '../../services/invoice.service';
import { LookupService } from '../../services/lookup.service';
import { SignalrService } from '../../services/signalr.service';
import {  Invoice, InvoiceForm, InvoiceItem, InvoiceItemForm } from '../../models/invoice.model';
import { DeleteConfirmModalComponent } from './delete-confirm-modal/delete-confirm-modal.component';
import { InvoiceFormComponent } from './invoice-form/invoice-form.component';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { Customer, Product, Store } from '../../models/lookup.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, InvoiceFormComponent, InvoiceListComponent, DeleteConfirmModalComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit ,OnDestroy  {
  invoices: Invoice[] = [];
  stores: Store[] = [];
  customers: Customer[] = [];
  products: Product[] = [];
  searchText = '';
  editingInvoiceId: number | null = null;
  invoiceIdToDelete: number | null = null;
  errorMessage = '';
  pageNumber = 1;
  pageSize = 5;
  totalPages = 1;
  totalCount = 0;
  isLoadingInvoices = false;
  isSaving = false;
  isDeleting = false;

  invoice: InvoiceForm = {
    serial: '',
    note: '',
    storeId: null,
    customerId: null,
    items: [
      {
        productId: null,
        quantity: 1,
        price: 0,
        discountPercent: 0,
        taxPercent: 0
      }
    ]
  };

  constructor(
    private invoiceService: InvoiceService,
    private authService: AuthService,
    private router: Router,
    private signalrService: SignalrService,
    private lookupService: LookupService

  ) {}
  // ngOnInit lifecycle hook to load invoices and lookups when the component is initialized
  ngOnInit() {
    this.loadInvoices();
    this.loadLookups();
      this.signalrService.startConnection()
    .then(() => {
      this.signalrService.onInvoiceChanged(() => {
        this.loadInvoices();
      });
    })
    .catch(() => {
  this.errorMessage = 'Real-time connection failed';
});
  }
// Method to load data for stores, customers, and products
  loadLookups() {
  this.lookupService.getStores().subscribe({
    next: (data) => this.stores = data,
    error: () => this.errorMessage = 'Cannot load stores'
  });

  this.lookupService.getCustomers().subscribe({
    next: (data) => this.customers = data,
    error: () => this.errorMessage = 'Cannot load customers'
  
  });

  this.lookupService.getProducts().subscribe({
    next: (data) => this.products = data,
    error: () => this.errorMessage = 'Cannot load products'
  });
}
// Method to load invoices with pagination
  loadInvoices() {
  this.isLoadingInvoices = true;

  this.invoiceService.getAll(this.pageNumber, this.pageSize).pipe(
    finalize(() => this.isLoadingInvoices = false)
  ).subscribe({
    next: (data) => {
      this.invoices = data.items;
      this.pageNumber = data.pageNumber;
      this.pageSize = data.pageSize;
      this.totalPages = data.totalPages;
      this.totalCount = data.totalCount;
    },
    error: () => {
      this.errorMessage = 'Cannot load invoices';
    }
  });
}
//methods to handle pagination
previousPage() {
  if (this.pageNumber > 1) {
    this.pageNumber--;
    this.loadInvoices();
  }
}

nextPage() {
  if (this.pageNumber < this.totalPages) {
    this.pageNumber++;
    this.loadInvoices();
  }
}
// Method to save an invoice 
  saveInvoice() {
    this.errorMessage = '';
    this.isSaving = true;

    if (this.editingInvoiceId !== null) {
      this.invoiceService.update(this.editingInvoiceId, this.invoice).pipe(
        finalize(() => this.isSaving = false)
      ).subscribe({
        next: () => {
          this.resetForm();
          this.loadInvoices();
        },
        error: () => {
          this.errorMessage = 'Update invoice failed';
        }
      });

      return;
    }
// Create a new invoice
    this.invoiceService.create(this.invoice).pipe(
      finalize(() => this.isSaving = false)
    ).subscribe({
      next: () => {
        this.resetForm();
        this.loadInvoices();
      },
      error: () => {
        this.errorMessage = 'Create invoice failed. Check StoreId, CustomerId and ProductId exist in database.';
      }
    });
  }
// Method to edit an existing invoice
  editInvoice(id: number) {
    this.invoiceService.getById(id).subscribe({
      next: (data) => {
        this.editingInvoiceId = data.id;

        this.invoice = {
          serial: data.serial,
          note: data.note,
          storeId: data.storeId,
          customerId: data.customerId,
          items: data.items.map((item: InvoiceItem) => ({
            productId: item.productId,
            quantity: item.quantity,
            price: item.price,
            discountPercent: item.discountPercent,
            taxPercent: item.taxPercent
          }))
        };
      },
      error: () => {
        this.errorMessage = 'Cannot load invoice details';
      }
    });
  }
// Method to open the delete confirmation modal
  openDeleteConfirm(id: number) {
    this.invoiceIdToDelete = id;
  }
// Method to cancel the delete operation
  cancelDelete() {
    this.invoiceIdToDelete = null;
  }
// Method to confirm the delete operation
  confirmDelete() {
    if (this.invoiceIdToDelete === null) {
      return;
    }

    this.isDeleting = true;

    this.invoiceService.delete(this.invoiceIdToDelete).pipe(
      finalize(() => this.isDeleting = false)
    ).subscribe({
      next: () => {
        this.invoiceIdToDelete = null;
        this.loadInvoices();
      },
      error: () => {
        this.errorMessage = 'Delete invoice failed';
      }
    });
  }
// Method to reset the invoice form
  resetForm() {
    this.editingInvoiceId = null;

    this.invoice = this.createEmptyInvoice();
  }
  // Method to handle stop the SignalR connection

  ngOnDestroy() {
  this.signalrService.stopConnection();
}

// Method to handle user logout
 logout() {
  this.authService.logout().subscribe({
    next: () => {
      this.router.navigate(['/login']);
    },
    error: () => {
      this.router.navigate(['/login']);
    }
  });
}
// Method to create an empty invoice form
private createEmptyInvoice(): InvoiceForm {
  return {
    serial: '',
    note: '',
    storeId: null,
    customerId: null,
    items: [this.createEmptyItem()]
  };
}
// Method to create an empty invoice item form
private createEmptyItem(): InvoiceItemForm {
  return {
    productId: null,
    quantity: 1,
    price: 0,
    discountPercent: 0,
    taxPercent: 0
  };
}
}
