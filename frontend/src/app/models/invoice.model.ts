export interface PaginatedInvoices {
  items: Invoice[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
}


export interface InvoiceItemForm {
  productId: number | null;
  quantity: number;
  price: number;
  discountPercent: number;
  taxPercent: number;
}

export interface InvoiceForm {
  serial: string;
  note: string;
  storeId: number | null;
  customerId: number | null;
  items: InvoiceItemForm[];
}

export interface InvoiceItem {
  productId: number;
  quantity: number;
  price: number;
  discountPercent: number;
  taxPercent: number;
}

export interface Invoice {
  id: number;
  serial: string;
  note: string;
  storeId: number;
  customerId: number;
  customerName?: string;
  storeName?: string;
  invoiceDate?: string;
  totalPrice?: number;
  items: InvoiceItem[];
}



export interface ApiErrorItem {
  description: string;
}
