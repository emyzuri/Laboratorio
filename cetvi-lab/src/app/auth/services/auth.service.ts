import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private readonly URL_BASE = 'http://localhost:5243/api/v1';

  async loginAPI(usuario: string): Promise<any> {
    const headers = new HttpHeaders().set('usuario', usuario).set('password', '123')
      .set('IdSesion', '83b5a7e2-eac8-4c8c-b74d-cb0be7e8b496');
    return await lastValueFrom(this.http.get(`${this.URL_BASE}/Usuario`, { headers }));
  }

  async getClientes(): Promise<any[]> {
    const headers = new HttpHeaders().set('IdSesion', localStorage.getItem('IdSesion') || '');
    return await lastValueFrom(this.http.get<any[]>(`${this.URL_BASE}/Cliente`, { headers }));
  }
}
