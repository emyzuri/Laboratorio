import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { lastValueFrom, firstValueFrom, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private readonly URL_BASE = 'http://localhost:5243/api/v1';

  /**
   * Genera los headers necesarios para las peticiones.
   * Incluye IdSesion y el Usuario dinámico para el seguimiento en base de datos.
   */
  private obtenerHeaders(): HttpHeaders {
    return new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('IdSesion', localStorage.getItem('IdSesion') || '')
      .set('Usuario', localStorage.getItem('usuario') || 'emy')
      .set('Authorization', `Bearer ${localStorage.getItem('token') || ''}`);
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
    return await firstValueFrom(this.http.get<any>(url, { headers: this.obtenerHeaders() }));
  }
  async getUsuarios(): Promise<any> {
  const url = `${this.URL_BASE}/Usuario/ListarUsuarios`;

  try {
    return await firstValueFrom(
      this.http.get<any>(url, { headers: this.obtenerHeaders() })
    );
  } catch (error) {
    console.error('Error al conectar con el listado de usuarios:', error);
    return null;
  }
}

  async insertarCliente(cliente: any): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Insertar`;
    return await firstValueFrom(this.http.post<any>(url, cliente, { headers: this.obtenerHeaders() }));
  }

  async actualizarCliente(cliente: any): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Actualizar`;
    return await firstValueFrom(this.http.post<any>(url, cliente, { headers: this.obtenerHeaders() }));
  }

  async eliminarCliente(id: number): Promise<any> {
    const url = `${this.URL_BASE}/Cliente/Eliminar`;
    const headers = this.obtenerHeaders().set('idCliente', id.toString());
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

  async insertarAbono(abono: any): Promise<any> {
    const url = `${this.URL_BASE}/Ensayo/InsertarAbono`;
    return await firstValueFrom(
      this.http.post<any>(url, abono, { headers: this.obtenerHeaders() })
    );
  }
  async getEnsayosDetallados(): Promise<any> {
    const url = `${this.URL_BASE}/Ensayo/Detallados`;
    try {
      return await firstValueFrom(
        this.http.get<any>(url, { headers: this.obtenerHeaders() })
      );
    } catch (error) {
      console.error('Error al obtener ensayos detallados:', error);
      return {
        esExitoso: false,
        datos: [],
        mensaje: 'No se pudo conectar con el servidor.'
      };
    }
  }
  async getRoles(): Promise<any> {
    const url = `${this.URL_BASE}/Permisos/Listar`; // URL de tu Swagger
    return await firstValueFrom(
      this.http.get<any>(url, { headers: this.obtenerHeaders() })
    );
  }
  async registrarUsuario(usuario: any): Promise<any> {
    const url = `${this.URL_BASE}/Usuario/Registrar`;

    return await firstValueFrom(
      this.http.post<any>(url, usuario, { headers: this.obtenerHeaders() })
    );
  }
  actualizarRolesUsuario(idUsuario: number, roles: number[]): Observable<any> {

    const body = {
      IdUsuario: idUsuario,
      Roles: roles
    };

    return this.http.post(
      `${this.URL_BASE}/Usuario/actualizar-roles`,
      body
    );
  }

}
