import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  async login(usuario: string) {
  if (!usuario) return alert('Ingresa un usuario');

  try {
    const datos = await this.authService.loginAPI(usuario);

    if (datos && datos.idUsuario) {
      localStorage.setItem('IdSesion', datos.idSesion);

      this.router.navigateByUrl('/principal');
    } else {
      alert('Usuario no encontrado en la base de datos');
    }

  } catch (error: any) {
    console.error('Error en la base de datos:', error);
    alert('Error al validar: ' + (error.message || 'Servidor desconectado'));
  }
}
}
