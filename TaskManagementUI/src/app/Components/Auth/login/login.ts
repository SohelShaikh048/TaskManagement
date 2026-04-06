import { Component } from '@angular/core';
import { AuthService } from '../../../Services/auth-service';
import { Router, RouterLink } from '@angular/router';
import { LoginRequestDto } from '../../../Models/auth.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  formUserName = '';
  formPassword = '';
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router){}

  onLogin() {
    if (!this.formUserName.trim()) { this.errorMessage = 'Username is required.'; return; }
    if (!this.formPassword.trim()) { this.errorMessage = 'Password is required.'; return; }

    this.isLoading = true;
    this.errorMessage = '';

    const dto: LoginRequestDto = {
      userName: this.formUserName.trim(),
      password: this.formPassword.trim()
    };

    this.authService.Login(dto).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.authService.saveToken(response.result.token);
          this.router.navigate(['/Projects']);
        } else {
          this.errorMessage = response.message || 'Login failed.';
        }
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Invalid username or password.';
        this.isLoading = false;
      }
    });
  }

}
