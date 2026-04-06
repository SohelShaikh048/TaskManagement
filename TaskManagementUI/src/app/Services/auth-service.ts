import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequestDto, RegistrationRequestDto } from '../Models/auth.model';
import { Observable } from 'rxjs';
import { ResponseDto } from '../Models/response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'https://localhost:7228/api/Auth';

  constructor(private http: HttpClient) { }

  Register(registerDto: RegistrationRequestDto): Observable<ResponseDto> {
    return this.http.post<ResponseDto>(`${this.baseUrl}/Register`, registerDto);
  }

  Login(loginDto: LoginRequestDto): Observable<ResponseDto> {
    return this.http.post<ResponseDto>(`${this.baseUrl}/Login`, loginDto);
  }

  saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }
  
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  logout(): void {
    localStorage.removeItem('authToken');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = JSON.parse(atob(token.split('.')[1]));
    // console.log("TOKEN PAYLOAD:", payload);

    // return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Admin";
    // const roleClaim = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const role = payload.role;

    if (role === 'Admin') return true;

    if (Array.isArray(role) && role.includes("Admin")) return true;

    return false;
  }

  getLoggedInUserEmail(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.email ?? null;
  }

}
