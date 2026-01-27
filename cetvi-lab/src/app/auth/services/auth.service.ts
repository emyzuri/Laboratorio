import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  constructor() { }

  async loginAPI(usuario: string): Promise<any> {
    try {
      const url = 'http://localhost:5243/api/v1/Usuario/Usuarios';

      console.log('📡 Conectando con la API...', url);

      const respuesta = await lastValueFrom(this.http.get(url));

      return respuesta;
    } catch (error) {
      console.error('Error en la API:', error);
      throw error;
    }
  }

  async signInWithAzure() {
  }
}
