import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
})
export class LoginComponent {
  private readonly authService: AuthService = inject(AuthService);
  private readonly router: Router = inject(Router);

  usuarioEncontrado: any = null;

  async login() {
    try {
      console.log('Iniciando sesión...');

      const datos = await this.authService.loginAPI('emily');

      console.log('¡DATO RECIBIDO DE LA API!', datos);

      this.usuarioEncontrado = datos;

      alert(`Bienvenida: ${datos.name} \nEmail: ${datos.email}`);

    } catch (error) {
      console.error('Error al entrar', error);
      alert('Hubo un error al conectar con el servidor');
    }
  }
}
