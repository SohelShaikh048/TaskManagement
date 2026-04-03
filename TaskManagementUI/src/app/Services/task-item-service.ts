import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../Models/response.model';
import { TaskItemDto } from '../Models/taskItem.model';

@Injectable({
  providedIn: 'root',
})
export class TaskItemService {
  private baseUrl = 'https://localhost:7228/api/TaskItem';

  constructor(private http: HttpClient) { }

  GetAll(): Observable<ResponseDto<TaskItemDto[]>> {
    var test = this.http.get<ResponseDto<TaskItemDto[]>>(this.baseUrl);
    return test;
  }

  GetById(id: string): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/${id}`);
  }

  GetByBoardId(boardId: string): Observable<ResponseDto> {
    return this.http.get<ResponseDto>(`${this.baseUrl}/GetByBoard/${boardId}`);
  }

  Create(taskItem: any): Observable<ResponseDto> {
    return this.http.post<ResponseDto>(this.baseUrl, taskItem);
  }

  Update(id:string, taskItem: any): Observable<ResponseDto> {
    return this.http.put<ResponseDto>(`${this.baseUrl}/${id}`, taskItem);
  }

  Delete(id: string): Observable<ResponseDto> {
    return this.http.delete<ResponseDto>(`${this.baseUrl}/${id}`);
  }

}
