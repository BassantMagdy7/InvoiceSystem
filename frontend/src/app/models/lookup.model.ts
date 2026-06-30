
export interface Store {
  id: number;
  name: string;
}

export interface Customer {
  id: number;
  name: string;
}

export interface Product {
  id: number;
  name: string;
  price?: number;
  unitPrice?: number;
}
