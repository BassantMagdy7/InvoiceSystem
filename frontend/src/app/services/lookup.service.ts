import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Customer, Product, Store } from '../models/lookup.model';
@Injectable({
  providedIn: 'root'
})
export class LookupService {

  private readonly apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getStores() {
    return this.http.get<Store[]>(`${this.apiUrl}/Stores`, {
      withCredentials: true
    });
  }

  getCustomers() {
    return this.http.get<Customer[]>(`${this.apiUrl}/Customers`, {
      withCredentials: true
    });
  }

  getProducts() {
    return this.http.get<Product[]>(`${this.apiUrl}/Products`, {
      withCredentials: true
    });
  }
}
