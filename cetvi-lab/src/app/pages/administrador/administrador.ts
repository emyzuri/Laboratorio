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

  filtroNombre: string = '';
  showFormModal = false;
  showRolesModal = false;
  cargando = false;

  usuarioSeleccionado: any = null;
  rolesSeleccionados: number[] = [];

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

  // ==============================
  // CARGAR USUARIOS
  // ==============================
  async cargarUsuarios() {
    try {
      const respuesta = await this.authService.getUsuarios();
      if (respuesta?.esExitoso && Array.isArray(respuesta.datos)) {
        this.usuarios = respuesta.datos
          .map((u: any) => ({
            ...u,
            nombreCompleto: `${u?.nombre ?? ''} ${u?.apellido ?? ''}`.trim(),
            listaRoles: u?.roles
              ? u.roles.split(',').map((r: string) => r.trim())
              : []
          }))
          .sort((a: any, b: any) => a.nombreCompleto.localeCompare(b.nombreCompleto));
      } else {
        this.usuarios = [];
      }
    } catch (error) {
      console.error('Error al cargar usuarios:', error);
      this.usuarios = [];
    }
  }

  // ==============================
  // CARGAR CATÁLOGO ROLES
  // ==============================
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

  // ==============================
  // FILTRO USUARIOS
  // ==============================
  get usuariosFiltrados() {
    let lista = this.usuarios;
    if (this.filtroNombre.trim()) {
      const busqueda = this.filtroNombre.toLowerCase();
      lista = lista.filter(u =>
        u.nombreCompleto.toLowerCase().includes(busqueda) ||
        u.cedula?.includes(busqueda)
      );
    }
    return lista.sort((a: any, b: any) => a.nombreCompleto.localeCompare(b.nombreCompleto));
  }

  // ==============================
  // MODAL ROLES
  // ==============================
  openRolesModal(user: any) {
    this.usuarioSeleccionado = user;
    this.rolesSeleccionados = [];

    if (user.listaRoles?.length) {
      this.listaRolesDisponibles.forEach(rolCatalogo => {
        if (user.listaRoles.includes(rolCatalogo.nombre)) {
          this.rolesSeleccionados.push(rolCatalogo.id);
        }
      });
    }

    this.showRolesModal = true;
  }

  closeRolesModal() {
    this.showRolesModal = false;
    this.usuarioSeleccionado = null;
    this.rolesSeleccionados = [];
  }

  rolSeleccionado(id: number): boolean {
    return this.rolesSeleccionados.includes(id);
  }

  toggleRol(id: number, event: any) {
    if (event.target.checked) {
      if (!this.rolesSeleccionados.includes(id)) this.rolesSeleccionados.push(id);
    } else {
      this.rolesSeleccionados = this.rolesSeleccionados.filter(r => r !== id);
    }
  }

  async guardarRoles() {
    if (!this.usuarioSeleccionado || this.rolesSeleccionados.length === 0) return;
    this.cargando = true;
    try {
      const respuesta = await this.authService.actualizarRolesUsuario(
        this.usuarioSeleccionado.idUsuario,
        this.rolesSeleccionados
      );
      if (respuesta?.esExitoso) {
        this.closeRolesModal();
        await this.cargarUsuarios();
      }
    } catch (error) {
      console.error('Error al actualizar roles:', error);
    } finally {
      this.cargando = false;
    }
  }

  // ==============================
  // NUEVO USUARIO
  // ==============================
  openNewUserModal() {
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
      Nombre: this.usuarioForm.nombre.trim(),
      Apellido: this.usuarioForm.apellido.trim(),
      Usuario: this.usuarioForm.login.trim(),
      Password: this.usuarioForm.password.trim(),
      Telefono: this.usuarioForm.telefono.trim(),
      Cedula: this.usuarioForm.cedula.trim(),
      Roles: [2] // siempre rol de usuario
    };

    try {
      const respuesta = await this.authService.registrarUsuario(payload);

      if (respuesta === true || respuesta?.esExitoso) {
        this.showFormModal = false;  // cerrar modal
        await this.cargarUsuarios();  // recargar lista de usuarios
      }

    } catch (error) {
      console.error('Error al registrar usuario:', error);
    } finally {
      this.cargando = false;
    }
  }

  // ==============================
  // VALIDACIONES
  // ==============================
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
    if (!/^\d{10}$/.test(cedula)) { this.cedulaValida = false; return; }
    const provincia = parseInt(cedula.substring(0, 2), 10);
    if (provincia < 1 || provincia > 24) { this.cedulaValida = false; return; }

    const digitos = cedula.split('').map(Number);
    const verificador = digitos.pop()!;
    let suma = 0;
    digitos.forEach((num, i) => {
      if (i % 2 === 0) {
        let mult = num * 2; if (mult > 9) mult -= 9; suma += mult;
      } else { suma += num; }
    });
    const decena = Math.ceil(suma / 10) * 10;
    this.cedulaValida = ((decena - suma === 10 ? 0 : decena - suma) === verificador);
  }

  closeModals() { this.showFormModal = false; }
}
