import { Component ,OnInit  } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink,CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent  {
        email = '';
        password = '';
        errorMessage = '';
        isLoading = false;

      
    constructor(
       private authService: AuthService,
       private router: Router
  ) {}
  
    // login method to handle user login , if successful navigate to dashboard , if failed show error message
    login() {
    this.errorMessage = '';
    this.isLoading = true;

    this.authService.login({
      email: this.email,
      password: this.password
    }).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (res) => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
      this.errorMessage = err.error?.message || 'Login failed. Please try again.';
    }
    });
  }

}
