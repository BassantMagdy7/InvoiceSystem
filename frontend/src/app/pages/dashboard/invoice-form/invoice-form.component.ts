import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {  InvoiceForm, InvoiceItemForm } from '../../../models/invoice.model';
import { Customer, Product, Store } from '../../../models/lookup.model';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice-form.component.html',
  styleUrl: './invoice-form.component.css'
})
export class InvoiceFormComponent {
  @Input({ required: true }) invoice!: InvoiceForm;
  @Input({ required: true }) stores: Store[] = [];
  @Input({ required: true }) customers: Customer[] = [];
  @Input({ required: true }) products: Product[] = [];
  @Input() editingInvoiceId: number | null = null;
  @Input() isSaving = false;

  @Output() save = new EventEmitter<void>();
  @Output() clear = new EventEmitter<void>();

  addItem() {
    this.invoice.items.push(this.createEmptyItem());
  }

  removeItem(index: number) {
    if (this.invoice.items.length > 1) {
      this.invoice.items.splice(index, 1);
    }
  }

  onProductChange(item: InvoiceItemForm) {
    const product = this.products.find((p) => p.id === item.productId);
    item.price = product ? (product.price ?? product.unitPrice ?? 0) : 0;
  }

  getItemTotal(item: InvoiceItemForm) {
    const subtotal = item.quantity * item.price;
    const discount = subtotal * item.discountPercent / 100;
    const afterDiscount = subtotal - discount;
    const tax = afterDiscount * item.taxPercent / 100;

    return afterDiscount + tax;
  }

  getInvoiceTotal() {
    return this.invoice.items.reduce((sum, item) => sum + this.getItemTotal(item), 0);
  }

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
