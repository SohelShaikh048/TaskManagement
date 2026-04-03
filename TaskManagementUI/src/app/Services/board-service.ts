import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../Models/response.model';
import { BoardDto } from '../Models/board.model';

@Injectable({
  providedIn: 'root',
})
export class BoardService {
  private baseUrl = 'https://localhost:7228/api/Boards';

  constructor(private http: HttpClient) { }

  GetAll(): Observable<ResponseDto> {
    var test = this.http.get<ResponseDto>(this.baseUrl);
    return test;
  }

  GetByProjectId(projectId: string): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/GetByProject/${projectId}`);
  }

  GetById(id: string): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/${id}`);
  }

  Create(board: BoardDto): Observable<ResponseDto<BoardDto>> {
    return this.http.post<ResponseDto<BoardDto>>(this.baseUrl, board);
  }

  Update(id: string, board: BoardDto): Observable<ResponseDto<BoardDto>> {
    return this.http.put<ResponseDto<BoardDto>>(`${this.baseUrl}/${id}`, board);
  }

  Delete(id: string): Observable<ResponseDto> {
    return this.http.delete<ResponseDto>(`${this.baseUrl}/${id}`);
  }
}
