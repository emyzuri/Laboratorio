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

  // Datos para la tabla
  usuarios: any[] = [];
  listaRolesDisponibles: any[] = [];
  filtroNombre = '';

  // Estados de Modales y Carga
  showFormModal = false;
  showRolesModal = false;
  showConfirmDelete = false;
  cargando = false;
  isEditMode = false; // Controla si el modal es para Nuevo o Editar

  // Variables de Selección
  usuarioSeleccionado: any = null;
  usuarioABorrar: any = null;
  rolesSeleccionados: number[] = [];

  // Formulario de Usuario
  usuarioForm = {
    nombre: '',
    apellido: '',
    login: '',
    password: '',
    telefono: '',
    cedula: ''
  };

  // Estados de Validación
  telefonoValido = false;
  cedulaValida = false;

  ngOnInit(): void {
    this.cargarUsuarios();
    this.cargarCatalogoRoles();
  }

  // --- MÉTODOS DE CARGA ---

  async cargarUsuarios() {
    try {
      const respuesta = await this.authService.getUsuarios();
      if (respuesta?.esExitoso && Array.isArray(respuesta.datos)) {
        this.usuarios = respuesta.datos.map((u: any) => ({
          ...u,
          // Mapeo flexible para nombres de columnas (C# vs SQL)
          idUsuario: u.idUsuario || u.us_id,
          nombreCompleto: `${u?.nombre ?? u?.us_nombre ?? ''} ${u?.apellido ?? u?.us_apellido ?? ''}`.trim().toUpperCase(),
          listaRoles: u?.roles ? u.roles.split(',').map((r: string) => r.trim()) : []
        }));
      }
    } catch (error) {
      console.error('Error al cargar usuarios:', error);
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

  // --- FILTRADO Y FORMATO ---

  get usuariosFiltrados() {
    let lista = this.usuarios;
    if (this.filtroNombre.trim()) {
      const busqueda = this.filtroNombre.toLowerCase();
      lista = lista.filter(u =>
        u.nombreCompleto.toLowerCase().includes(busqueda) ||
        (u.cedula && u.cedula.includes(busqueda))
      );
    }
    return lista.sort((a, b) => a.nombreCompleto.localeCompare(b.nombreCompleto));
  }

  toUpperCase(campo: keyof typeof this.usuarioForm) {
    if (this.usuarioForm[campo]) {
      this.usuarioForm[campo] = this.usuarioForm[campo].toUpperCase();
    }
  }

  soloNumeros(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
    if (event.target.name === 'no-autocomplete-ced') this.usuarioForm.cedula = event.target.value;
    if (event.target.name === 'no-autocomplete-tel') this.usuarioForm.telefono = event.target.value;
  }

  // --- VALIDACIONES ---

  validarTelefono() {
    const tel = this.usuarioForm.telefono;
    this.telefonoValido = /^(09)\d{8}$/.test(tel);
  }

  validarCedula() {
    const cedula = this.usuarioForm.cedula;
    if (!cedula || cedula.length !== 10) {
      this.cedulaValida = false;
      return;
    }
    const provincia = parseInt(cedula.substring(0, 2), 10);
    if (!((provincia > 0 && provincia <= 24) || provincia === 30)) {
      this.cedulaValida = false;
      return;
    }
    const digitos = cedula.split('').map(Number);
    const ultimoDigito = digitos.pop();
    const suma = digitos.reduce((acc, valor, indice) => {
      if (indice % 2 === 0) {
        let producto = valor * 2;
        if (producto > 9) producto -= 9;
        return acc + producto;
      }
      return acc + valor;
    }, 0);
    const dv = suma % 10 === 0 ? 0 : 10 - (suma % 10);
    this.cedulaValida = (dv === ultimoDigito);
  }

  get formularioValido(): boolean {
    return !!(
      this.usuarioForm.nombre?.trim() &&
      this.usuarioForm.apellido?.trim() &&
      this.usuarioForm.login?.trim() &&
      (this.isEditMode || this.usuarioForm.password?.trim()) && // Password solo obligatorio en Nuevo
      this.telefonoValido &&
      this.cedulaValida
    );
  }

  // --- ACCIONES: GUARDAR (INSERT/UPDATE) ---

  async guardarUsuario() {
    if (!this.formularioValido) return;
    this.cargando = true;

    const payload = {
      IdUsuario: this.isEditMode ? this.usuarioSeleccionado.idUsuario : 0,
      Nombre: this.usuarioForm.nombre.trim().toUpperCase(),
      Apellido: this.usuarioForm.apellido.trim().toUpperCase(),
      Usuario: this.usuarioForm.login.trim().toUpperCase(),
      Password: this.isEditMode ? '' : this.usuarioForm.password.trim(), // API no actualiza pass aquí
      Telefono: this.usuarioForm.telefono.trim(),
      Cedula: this.usuarioForm.cedula.trim(),
      Roles: [2]
    };

    try {
      const respuesta = this.isEditMode
        ? await this.authService.actualizarUsuario(payload)
        : await this.authService.registrarUsuario(payload);

      if (respuesta === true || respuesta?.esExitoso) {
        this.closeModals();
        await this.cargarUsuarios();
      }
    } catch (error) {
      console.error('Error en operación de usuario:', error);
    } finally {
      this.cargando = false;
    }
  }

  // --- ACCIONES: EDITAR Y ELIMINAR ---

  editarUsuario(user: any) {
    this.isEditMode = true;
    this.usuarioSeleccionado = user;
    this.usuarioForm = {
      nombre: user.nombre || user.us_nombre || '',
      apellido: user.apellido || user.us_apellido || '',
      login: user.usuario || user.us_login || '',
      password: '***', // Placeholder visual
      telefono: user.telefono || user.us_telefono || '',
      cedula: user.cedula || user.us_cedula || ''
    };
    this.validarTelefono();
    this.validarCedula();
    this.showFormModal = true;
  }

  borrarUsuario(user: any) {
    this.usuarioABorrar = user;
    this.showConfirmDelete = true;
  }

  async confirmarEliminacion() {
    if (!this.usuarioABorrar) return;
    this.cargando = true;
    const id = this.usuarioABorrar.idUsuario;
    try {
      const resp = await this.authService.eliminarUsuario(id);
      if (resp?.esExitoso || resp === true) {
        await this.cargarUsuarios();
        this.cancelarEliminacion();
      }
    } catch (error) {
      console.error('Error al eliminar:', error);
    } finally {
      this.cargando = false;
    }
  }

  cancelarEliminacion() {
    this.showConfirmDelete = false;
    this.usuarioABorrar = null;
  }

  // --- GESTIÓN DE ROLES ---

  openRolesModal(user: any) {
    this.usuarioSeleccionado = user;
    this.rolesSeleccionados = this.listaRolesDisponibles
      .filter(rol => user.listaRoles.includes(rol.nombre))
      .map(rol => rol.id);
    this.showRolesModal = true;
  }

  async actualizarRoles() {
    if (!this.usuarioSeleccionado) return;
    this.cargando = true;
    try {
      const id = this.usuarioSeleccionado.idUsuario;
      const resp = await this.authService.actualizarRolesUsuario(id, this.rolesSeleccionados);
      if (resp?.esExitoso || resp === true) {
        this.closeRolesModal();
        await this.cargarUsuarios();
      }
    } finally {
      this.cargando = false;
    }
  }

  // --- CONTROL DE MODALES ---

  openNewUserModal() {
    this.isEditMode = false;
    this.usuarioForm = { nombre: '', apellido: '', login: '', password: '', telefono: '', cedula: '' };
    this.telefonoValido = false;
    this.cedulaValida = false;
    this.showFormModal = true;
  }

  closeModals() {
    this.showFormModal = false;
    this.showRolesModal = false;
    this.showConfirmDelete = false;
  }

  closeRolesModal() {
    this.showRolesModal = false;
    this.usuarioSeleccionado = null;
  }

  isRolSelected(rolId: number): boolean { return this.rolesSeleccionados.includes(rolId); }
  onRolChange(event: any, rolId: number) {
    if (event.target.checked) {
      if (!this.rolesSeleccionados.includes(rolId)) this.rolesSeleccionados.push(rolId);
    } else {
      this.rolesSeleccionados = this.rolesSeleccionados.filter(id => id !== rolId);
    }
  }
}
