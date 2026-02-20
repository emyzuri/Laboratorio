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

  private authService = inject(AuthService);
  private router = inject(Router);

  login(usuario: string, password: string): void {

    console.log('Intentando login...');

    if (!usuario || !password) {
      alert('⚠️ Por favor, ingresa usuario y contraseña');
      return;
    }

    this.authService.loginAPI(usuario, password)
      .subscribe({
        next: (respuesta) => {

          console.log('Respuesta backend:', respuesta);

          if (respuesta?.esExitoso && respuesta?.datos) {

            const data = respuesta.datos;

            localStorage.setItem('IdSesion', data.idSesion);
            localStorage.setItem('usuarioLogueado', JSON.stringify(data));
            localStorage.setItem('token', data.token || '');

            this.router.navigateByUrl('/principal');

          } else {
            alert('❌ Credenciales inválidas');
          }
        },
        error: (error) => {
          console.error('Error HTTP:', error);
          alert('🚫 Error de comunicación con el servidor');
        }
      });
  }
}
