import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {  Invoice, InvoiceForm, PaginatedInvoices} from '../models/invoice.model';


@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private readonly apiUrl = `${environment.apiBaseUrl}/invoices`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber: number = 1, pageSize: number = 5) {
  return this.http.get<PaginatedInvoices>(`${this.apiUrl}`, {
    params: {
      pageNumber,
      pageSize
    },
    withCredentials: true
  });
}

 getById(id: number) {
  return this.http.get<Invoice>(`${this.apiUrl}/${id}`, {
    withCredentials: true
  });
}

 create(invoice: InvoiceForm) {
  return this.http.post<Invoice>(`${this.apiUrl}`, invoice, {
    withCredentials: true
  });
}

 update(id: number, invoice: InvoiceForm) {
  return this.http.put(`${this.apiUrl}/${id}`, invoice, {
    withCredentials: true
  });
}

delete(id: number) {
  return this.http.delete(`${this.apiUrl}/${id}`, {
    withCredentials: true
  });
}

 
  }

