import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-administrador',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './administrador.html',
  styleUrls: ['./administrador.scss']
})
export class AdministradorComponent implements OnInit {
  private readonly authService = inject(AuthService);
  usuarios: any[] = [];
  filtroNombre: string = '';
  showFormModal = false;
  isEditing = false;

  usuarioForm = { idUsuario: 0, nombre: '', apellido: '', login: '', password: '', idRol: 1, activo: 1 };

  ngOnInit() {
    this.cargarUsuarios();
  }

  async cargarUsuarios() {
    try {
      const respuesta = await this.authService.getUsuarios();
      if (respuesta?.esExitoso) {
        // Cargamos los datos directos del backend
        this.usuarios = respuesta.datos || [];
      }
    } catch (error) {
      console.error('Error al cargar usuarios:', error);
    }
  }

  openNewUserModal() {
    this.isEditing = false;
    this.resetForm();
    this.showFormModal = true;
  }

  resetForm() {
    this.usuarioForm = { idUsuario: 0, nombre: '', apellido: '', login: '', password: '', idRol: 1, activo: 1 };
  }

  closeModals() { this.showFormModal = false; }
}
