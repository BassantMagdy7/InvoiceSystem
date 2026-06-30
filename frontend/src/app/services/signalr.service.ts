import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection?: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalrHubUrl)
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

  onInvoiceChanged(callback: () => void) {
    this.hubConnection?.on('InvoiceChanged', callback);
  }

  stopConnection() {
    this.hubConnection?.stop();
  }
}
