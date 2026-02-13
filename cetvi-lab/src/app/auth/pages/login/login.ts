import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  async login(usuario: string, password: string) {
    if (!usuario || !password) return alert('⚠️ Por favor, ingresa usuario y contraseña');

    try {
      const respuesta = await this.authService.loginAPI(usuario, password);

      if (respuesta && respuesta.esExitoso === true) {

        const data = respuesta.datos;

        if (data && data.idSesion) {
          localStorage.setItem('IdSesion', data.idSesion);
          localStorage.setItem('IdUsuario', data.idUsuario.toString());

          this.router.navigateByUrl('/principal');
        }
      } else {
        alert('❌ Error de Acceso: ' + (respuesta?.mensaje || 'Credenciales inválidas'));
      }
    } catch (error) {
      console.error('Error de red:', error);
      alert('🚫 Error de comunicación: El servidor no responde. Revisa que no esté pausado en Visual Studio.');
    }
  }
}
