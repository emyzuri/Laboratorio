import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private http = inject(HttpClient);
  private readonly URL_BASE = 'http://localhost:5243/api/v1';

  // ==============================
  // HEADERS
  // ==============================
  private obtenerHeaders(): HttpHeaders {

    const usuarioData = JSON.parse(
      localStorage.getItem('usuarioLogueado') || '{}'
    );

    return new HttpHeaders({
      'Content-Type': 'application/json',
      'IdSesion': localStorage.getItem('IdSesion') || '',
      'usuario': usuarioData.usuario || '',
      'idCliente': (usuarioData.idCliente || usuarioData.idRol || 0).toString(),
      'Authorization': `Bearer ${localStorage.getItem('token') || ''}`
    });
  }

  // ==============================
  // LOGIN
  // ==============================
  loginAPI(usuario: string, password: string) {
    const url = `${this.URL_BASE}/Usuario?usuario=${usuario}&password=${password}`;
    return this.http.get<any>(url);
  }
  // ==============================
  // MENÚ
  // ==============================
  async getMenu(): Promise<any> {
  try {

    return await firstValueFrom(
      this.http.get<any>(
        `${this.URL_BASE}/Menu/Menus`,
        { headers: this.obtenerHeaders() }
      )
    );

  } catch (error) {

    console.error('Error al obtener menú:', error);

    return {
      esExitoso: false,
      datos: []
    };
  }
}


  // ==============================
  // CLIENTES
  // ==============================
  async getClientes(): Promise<any> {

    return await firstValueFrom(
      this.http.get<any>(
        `${this.URL_BASE}/Cliente/Clientes`,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  async insertarCliente(cliente: any): Promise<any> {

    return await firstValueFrom(
      this.http.post<any>(
        `${this.URL_BASE}/Cliente/Insertar`,
        cliente,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  async actualizarCliente(cliente: any): Promise<any> {

    return await firstValueFrom(
      this.http.put<any>(
        `${this.URL_BASE}/Cliente/Actualizar`,
        cliente,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  async eliminarCliente(id: number): Promise<any> {

    const headers = this.obtenerHeaders().set('idCliente', id.toString());

    return await firstValueFrom(
      this.http.delete<any>(
        `${this.URL_BASE}/Cliente/Eliminar`,
        { headers }
      )
    );
  }

  // ==============================
  // ENSAYOS
  // ==============================
  async insertarEnsayo(datos: any): Promise<any> {

    return await firstValueFrom(
      this.http.post<any>(
        `${this.URL_BASE}/Ensayo`,
        datos,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  async getEnsayosDeudores(): Promise<any> {
  return await firstValueFrom(
    this.http.get<any>(
      `${this.URL_BASE}/Ensayo/Deudores`,
      { headers: this.obtenerHeaders() }
    )
  );
}

  async getCatalogoEnsayos(): Promise<any> {

    return await firstValueFrom(
      this.http.get<any>(
        `${this.URL_BASE}/Ensayo/Catalogo`,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  async insertarAbono(abono: any): Promise<any> {

    return await firstValueFrom(
      this.http.post<any>(
        `${this.URL_BASE}/Ensayo/InsertarAbono`,
        abono,
        { headers: this.obtenerHeaders() }
      )
    );
  }

  private idPruebaActual: number | null = null;

  setIdPrueba(id: number) {
    this.idPruebaActual = id;
  }

  getIdPrueba(): number | null {
    return this.idPruebaActual;
  }

  async getEnsayosDeudoresFechas(): Promise<any> {
    return await firstValueFrom(
      this.http.get<any>(
        `${this.URL_BASE}/Ensayo/Deudores`,
        { headers: this.obtenerHeaders() }
      )
    );
  }
  // ==============================
  // USUARIOS
  // ==============================
  async getUsuarios(): Promise<any> {

    try {

      return await firstValueFrom(
        this.http.get<any>(
          `${this.URL_BASE}/Usuario/ListarUsuarios`,
          { headers: this.obtenerHeaders() }
        )
      );

    } catch (error) {

      console.error('Error al cargar usuarios:', error);

      return { esExitoso: false, datos: [] };
    }
  }

  async getRoles(): Promise<any> {

    try {

      return await firstValueFrom(
        this.http.get<any>(
          `${this.URL_BASE}/Usuario/ListarRoles`,
          { headers: this.obtenerHeaders() }
        )
      );

    } catch (error) {

      console.error('Error al obtener roles:', error);

      return { esExitoso: false, datos: [] };
    }
  }
  registrarUsuario(usuario: any): Promise<any> {
  return firstValueFrom(
    this.http.post<any>(
      `${this.URL_BASE}/Usuario/Registrar`,
      usuario,
      { headers: this.obtenerHeaders() }
    )
  );
}


  async actualizarRolesUsuario(
    idUsuario: number,
    roles: number[]
  ): Promise<any> {

    const body = {
      IdUsuario: idUsuario,
      Roles: roles
    };

    return await firstValueFrom(
      this.http.put<any>(
        `${this.URL_BASE}/Usuario/actualizar-roles`,
        body,
        { headers: this.obtenerHeaders() }
      )
    );
  }
}
