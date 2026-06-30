import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, LoginResponse, SignupRequest ,CurrentUser ,SignupResponse} from '../models/auth.models';
import { environment } from '../../environments/environment';
import { catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = `${environment.apiBaseUrl}/Auth`;
  // inject HttpClient in the constructor to make HTTP requests to the backend API
  constructor(private http: HttpClient)  { }

//login method that takes a LoginRequest object and returns a LoginResponse (email , message) from the backend API 
  login(data: LoginRequest) {
  return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data, {
    withCredentials: true
  });
}
// signup method send a post request to the /register endpoint and returns a SignupResponse from the backend API 
  signup(data: SignupRequest) {
  return this.http.post<SignupResponse>(`${this.apiUrl}/register`, data, {
    withCredentials: true
  });
}
 // isAuthenticated method that checks if the user is authenticated by sending a GET request to the /check endpoint and returns an observable of boolean value
 isAuthenticated() {
  return this.http.get<CurrentUser>(
    `${this.apiUrl}/check`,
    { withCredentials: true }
  ).pipe(
    map(user => ({ loggedIn: true, user })),
    catchError(() => of({ loggedIn: false, user: null }))
  );
}

// logout method that sends a POST request to the /logout endpoint and returns an observable of void
  logout() {
  return this.http.post<void>(`${this.apiUrl}/logout`, {}, {
    withCredentials: true
  });
}
}
