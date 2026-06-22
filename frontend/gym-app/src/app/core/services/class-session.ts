import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClassSession } from '../../shared/models/gym';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ClassSessionService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/classsessions`;

  getUpcoming(): Observable<ClassSession[]> {
    return this.http.get<ClassSession[]>(`${this.apiUrl}/upcoming`);
  }

  getAll(): Observable<ClassSession[]> {
    return this.http.get<ClassSession[]>(this.apiUrl);
  }

  getById(id: number): Observable<ClassSession> {
    return this.http.get<ClassSession>(`${this.apiUrl}/${id}`);
  }
}
