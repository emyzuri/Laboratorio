import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { lastValueFrom, firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private readonly URL_BASE = 'http://localhost:5243/api/v1';

  private obtenerHeaders(): HttpHeaders {
    return new HttpHeaders().set('IdSesion', localStorage.getItem('IdSesion') || '');
  }

  async loginAPI(usuario: string, password: string): Promise<any> {
    const url = `${this.URL_BASE}/Usuario?usuario=${usuario}&password=${password}`;
    return await firstValueFrom(this.http.get<any>(url));
  }

  async getMenu(): Promise<any[]> {
    return await lastValueFrom(
      this.http.get<any[]>(`${this.URL_BASE}/Menu`, { headers: this.obtenerHeaders() })
    );
  }

  async getClientes(): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Clientes`;
    return await firstValueFrom(this.http.get<any>(url));
  }

  async getUsuarios(): Promise<any> {
    const url = `${this.URL_BASE}/Usuario/ListarUsuarios`;
    return await firstValueFrom(
      this.http.get<any>(url, { headers: this.obtenerHeaders() })
    );
  }

  async insertarCliente(cliente: any): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Insertar`;
    return await firstValueFrom(this.http.post<any>(url, cliente));
  }

  async actualizarCliente(cliente: any): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Actualizar`;
    return await firstValueFrom(this.http.put<any>(url, cliente));
  }

  async eliminarCliente(id: number): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Eliminar`;
    const headers = new HttpHeaders().set('idCliente', id.toString());
    return await firstValueFrom(this.http.delete<any>(url, { headers }));
  }
  async insertarEnsayo(datos: any): Promise<any> {
  const url = `${this.URL_BASE}/Ensayo`;
  return await firstValueFrom(this.http.post<any>(url, datos, { headers: this.obtenerHeaders() }));
  }
  async getEnsayos(): Promise<any> {
    const url = `${this.URL_BASE}/Ensayo/Deudores`;
    return await firstValueFrom(
      this.http.get<any>(url, { headers: this.obtenerHeaders() })
    );
  }
  async getCatalogoEnsayos(): Promise<any> {
    const url = `${this.URL_BASE}/Ensayo/Catalogo`;
    return await firstValueFrom(
      this.http.get<any>(url, { headers: this.obtenerHeaders() })
    );
  }
}
