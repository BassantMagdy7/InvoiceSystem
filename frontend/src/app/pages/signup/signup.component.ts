import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ApiErrorItem } from '../../models/invoice.model';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule, RouterLink,CommonModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
      
       email = '';
       password = '';
       errors: string[] = [];
       successMessage = '';
       isLoading = false;

       constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  signup() {
    this.errors = [];
    this.successMessage = '';
    this.isLoading = true;

    this.authService.signup({
     
      email: this.email,
      password: this.password
    }).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (response) => {
        this.successMessage = response.message;

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1000);
      },
      error: (err) => {
  if (Array.isArray(err.error)) {
    this.errors = err.error.map((e: ApiErrorItem) => e.description);
  } else if (err.error?.errors) {
    this.errors = Object.values(err.error.errors).flat() as string[];
  } else if (err.error?.message) {
    this.errors = [err.error.message];
  } else {
    this.errors = ['Signup failed. Please try again.'];
  }
}
    });
  }
}
