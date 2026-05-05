import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { ParseResponse } from '../models/parse-response';

@Injectable({
  providedIn: 'root'
})
export class ParserService {

  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/api/parser/parse`;

  parseText(input: string): Observable<ParseResponse> {
    return this.http.post<ParseResponse>(
      this.apiUrl,
      input,
      {
        headers: {
          'Content-Type': 'text/plain'
        }
      }
    );
  }
}