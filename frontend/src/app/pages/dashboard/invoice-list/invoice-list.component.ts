import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Invoice } from '../../../models/invoice.model';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice-list.component.html',
  styleUrl: './invoice-list.component.css'
})
export class InvoiceListComponent {
  @Input({ required: true }) invoices: Invoice[] = [];
  @Input() pageNumber = 1;
  @Input() totalPages = 1;
  @Input() isLoadingInvoices = false;

  @Output() edit = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();
  @Output() previousPage = new EventEmitter<void>();
  @Output() nextPage = new EventEmitter<void>();

  searchText = '';

  get filteredInvoices() {
    const text = this.searchText.toLowerCase();

    return this.invoices.filter(invoice =>
      invoice.serial?.toLowerCase().includes(text) ||
      invoice.customerName?.toLowerCase().includes(text) ||
      invoice.storeName?.toLowerCase().includes(text)
    );
  }

  formatInvoiceDate(invoiceDate?: string) {
    if (!invoiceDate) {
      return '';
    }

    const hasTimeZone = /z$|[+-]\d{2}:\d{2}$/i.test(invoiceDate);
    const date = new Date(hasTimeZone ? invoiceDate : `${invoiceDate}Z`);

    return new Intl.DateTimeFormat('en-US', {
      month: 'numeric',
      day: 'numeric',
      year: '2-digit'
    }).format(date);
  }
}
