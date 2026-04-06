import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../Models/response.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private baseUrl = 'https://localhost:7228/api/Project';

  constructor(private http: HttpClient) { }

  GetAll(): Observable<ResponseDto> {
    var test = this.http.get<ResponseDto>(this.baseUrl);
    return test;
  }

  GetByUser(): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/GetByUser`);
  }

  GetById(id: string): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/${id}`);
  }

  Create(project: any): Observable<ResponseDto> {
    return this.http.post<ResponseDto>(this.baseUrl, project);
  }

  Delete(id: string): Observable<ResponseDto> {
    return this.http.delete<ResponseDto>(`${this.baseUrl}/${id}`);
  }

}
