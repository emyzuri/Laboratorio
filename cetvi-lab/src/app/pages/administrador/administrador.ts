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
  listaRolesDisponibles: any[] = [];
  filtroNombre = '';
  showFormModal = false;
  showRolesModal = false;
  cargando = false;

  usuarioSeleccionado: any = null;
  rolesSeleccionados: number[] = [];

  // Objeto inicializado vacío
  usuarioForm = {
    nombre: '',
    apellido: '',
    login: '',
    password: '',
    telefono: '',
    cedula: ''
  };

  telefonoValido = false;
  cedulaValida = false;

  ngOnInit(): void {
    this.cargarUsuarios();
    this.cargarCatalogoRoles();
  }

  toUpperCase(campo: keyof typeof this.usuarioForm) {
    if (this.usuarioForm[campo]) {
      this.usuarioForm[campo] = this.usuarioForm[campo].toUpperCase();
    }
  }

  async cargarUsuarios() {
    try {
      const respuesta = await this.authService.getUsuarios();
      if (respuesta?.esExitoso && Array.isArray(respuesta.datos)) {
        this.usuarios = respuesta.datos.map((u: any) => ({
          ...u,
          nombreCompleto: `${u?.nombre ?? ''} ${u?.apellido ?? ''}`.trim().toUpperCase(),
          listaRoles: u?.roles ? u.roles.split(',').map((r: string) => r.trim()) : []
        }));
      } else {
        this.usuarios = [];
      }
    } catch (error) {
      console.error('Error al cargar usuarios:', error);
      this.usuarios = [];
    }
  }

  async cargarCatalogoRoles() {
    try {
      const respuesta = await this.authService.getRoles();
      if (respuesta?.esExitoso && Array.isArray(respuesta.datos)) {
        this.listaRolesDisponibles = respuesta.datos.map((r: any) => ({
          id: r.idRol || r.id,
          nombre: r.nombreRol || r.nombre
        }));
      }
    } catch (error) {
      console.error('Error al cargar catálogo de roles:', error);
    }
  }

  get usuariosFiltrados() {
    let lista = this.usuarios;
    if (this.filtroNombre.trim()) {
      const busqueda = this.filtroNombre.toLowerCase();
      lista = lista.filter(u =>
        u.nombreCompleto.toLowerCase().includes(busqueda) || u.cedula?.includes(busqueda)
      );
    }
    return lista.sort((a, b) => a.nombreCompleto.localeCompare(b.nombreCompleto));
  }

  // --- GESTIÓN DE ROLES MÚLTIPLES ---
  openRolesModal(user: any) {
    this.usuarioSeleccionado = user;
    this.rolesSeleccionados = this.listaRolesDisponibles
      .filter(rol => user.listaRoles.includes(rol.nombre))
      .map(rol => rol.id);
    this.showRolesModal = true;
  }

  isRolSelected(rolId: number): boolean {
    return this.rolesSeleccionados.includes(rolId);
  }

  onRolChange(event: any, rolId: number) {
    if (event.target.checked) {
      if (!this.rolesSeleccionados.includes(rolId)) this.rolesSeleccionados.push(rolId);
    } else {
      this.rolesSeleccionados = this.rolesSeleccionados.filter(id => id !== rolId);
    }
  }

  async actualizarRoles() {
    if (!this.usuarioSeleccionado) return;
    this.cargando = true;
    try {
      const resp = await this.authService.actualizarRolesUsuario(
        this.usuarioSeleccionado.idUsuario,
        this.rolesSeleccionados
      );
      if (resp?.esExitoso || resp === true) {
        this.closeRolesModal();
        await this.cargarUsuarios();
      }
    } catch (error) {
      console.error('Error al actualizar roles:', error);
    } finally {
      this.cargando = false;
    }
  }

  // --- NUEVO USUARIO (SIN AUTOLLENADO) ---
  openNewUserModal() {
    // Forzamos el vaciado total para evitar que queden rastros
    this.usuarioForm = {
      nombre: '',
      apellido: '',
      login: '',
      password: '',
      telefono: '',
      cedula: ''
    };
    this.telefonoValido = false;
    this.cedulaValida = false;
    this.showFormModal = true;
  }

  async guardarUsuario() {
    if (!this.formularioValido) return;
    this.cargando = true;
    const payload = {
      Nombre: this.usuarioForm.nombre.trim().toUpperCase(),
      Apellido: this.usuarioForm.apellido.trim().toUpperCase(),
      Usuario: this.usuarioForm.login.trim().toUpperCase(),
      Password: this.usuarioForm.password.trim(),
      Telefono: this.usuarioForm.telefono.trim(),
      Cedula: this.usuarioForm.cedula.trim(),
      Roles: [2]
    };
    try {
      const respuesta = await this.authService.registrarUsuario(payload);
      if (respuesta === true || respuesta?.esExitoso) {
        this.showFormModal = false;
        await this.cargarUsuarios();
      }
    } catch (error) {
      console.error('Error al registrar usuario:', error);
    } finally {
      this.cargando = false;
    }
  }

  get formularioValido(): boolean {
    return !!(
      this.usuarioForm.nombre?.trim() &&
      this.usuarioForm.apellido?.trim() &&
      this.usuarioForm.login?.trim() &&
      this.usuarioForm.password?.trim() &&
      this.telefonoValido &&
      this.cedulaValida
    );
  }

  soloNumeros(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
  }

  validarTelefono() {
    this.telefonoValido = /^\d{10}$/.test(this.usuarioForm.telefono);
  }

  validarCedula() {
    const cedula = this.usuarioForm.cedula;
    this.cedulaValida = /^\d{10}$/.test(cedula);
  }

  closeModals() {
    this.showFormModal = false;
    this.showRolesModal = false;
  }

  closeRolesModal() {
    this.showRolesModal = false;
    this.usuarioSeleccionado = null;
    this.rolesSeleccionados = [];
  }
}
