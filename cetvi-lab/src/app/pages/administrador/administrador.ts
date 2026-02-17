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
  showSuccessModal = false;
  showRolesModal = false;
  cargando = false;

  usuarioSeleccionado: any = null;
  rolesSeleccionados: number[] = [];

  listaRolesDisponibles = [
    { id: 1, nombre: 'Administrador' },
    { id: 2, nombre: 'Usuario' },
    { id: 3, nombre: 'Supervisor' }
  ];

  usuarioForm = {
    nombre: '',
    apellido: '',
    login: '',
    password: '',
    telefono: '',
    cedula: '',
    idRol: 2
  };

  telefonoValido = false;
  cedulaValida = false;

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  async cargarUsuarios() {
    try {

      const respuesta = await this.authService.getUsuarios();

      if (respuesta?.esExitoso && Array.isArray(respuesta.datos)) {

        this.usuarios = respuesta.datos.map((u: any) => ({
          ...u,
          nombreCompleto: `${u?.nombre ?? ''} ${u?.apellido ?? ''}`.trim(),
          listaRoles: u?.roles
            ? u.roles.split(',').map((r: string) => r.trim())
            : []
        }));

      } else {
        this.usuarios = [];
      }

    } catch (error) {
      console.error('Error al cargar usuarios:', error);
      this.usuarios = [];
    }
  }

  get usuariosFiltrados() {
    if (!this.filtroNombre) return this.usuarios;

    return this.usuarios.filter(u =>
      u.nombreCompleto
        .toLowerCase()
        .includes(this.filtroNombre.toLowerCase())
    );
  }

  openNewUserModal() {
    this.usuarioForm = {
      nombre: '',
      apellido: '',
      login: '',
      password: '',
      telefono: '',
      cedula: '',
      idRol: 2
    };

    this.telefonoValido = false;
    this.cedulaValida = false;
    this.showFormModal = true;
  }

  closeModals() {
    this.showFormModal = false;
  }

  closeSuccessModal() {
    this.showSuccessModal = false;
  }

  openRolesModal(user: any) {

    this.usuarioSeleccionado = user;
    this.rolesSeleccionados = [];

    if (user.listaRoles?.length) {
      this.listaRolesDisponibles.forEach(r => {
        if (user.listaRoles.includes(r.nombre)) {
          this.rolesSeleccionados.push(r.id);
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
      this.rolesSeleccionados.push(id);
    } else {
      this.rolesSeleccionados =
        this.rolesSeleccionados.filter(r => r !== id);
    }
  }

  async guardarRoles() {

  if (!this.usuarioSeleccionado) return;

  try {

    const respuesta = await this.authService
      .actualizarRolesUsuario(
        this.usuarioSeleccionado.idUsuario,
        this.rolesSeleccionados
      )
      .toPromise();

    if (respuesta?.esExitoso) {
      this.closeRolesModal();
      await this.cargarUsuarios();
    }

  } catch (error) {
    console.error('Error al actualizar roles:', error);
  }
}


  soloNumeros(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
  }

  validarTelefono() {
    const telefono = this.usuarioForm.telefono;
    this.telefonoValido = /^\d{10}$/.test(telefono);
  }

  validarCedula() {

    const cedula = this.usuarioForm.cedula;

    if (!/^\d{10}$/.test(cedula)) {
      this.cedulaValida = false;
      return;
    }

    const provincia = parseInt(cedula.substring(0, 2), 10);
    if (provincia < 1 || provincia > 24) {
      this.cedulaValida = false;
      return;
    }

    const digitos = cedula.split('').map(Number);
    const verificador = digitos.pop()!;

    let suma = 0;

    digitos.forEach((num, i) => {
      if (i % 2 === 0) {
        let mult = num * 2;
        if (mult > 9) mult -= 9;
        suma += mult;
      } else {
        suma += num;
      }
    });

    const decena = Math.ceil(suma / 10) * 10;
    const resultado = decena - suma;

    this.cedulaValida =
      (resultado === 10 ? 0 : resultado) === verificador;
  }

  async guardarUsuario() {

    if (!this.formularioValido) return;

    this.cargando = true;

    const payload = {
      nombre: this.usuarioForm.nombre.trim(),
      apellido: this.usuarioForm.apellido.trim(),
      usuario: this.usuarioForm.login.trim(),
      password: this.usuarioForm.password.trim(),
      telefono: this.usuarioForm.telefono.trim(),
      cedula: this.usuarioForm.cedula.trim(),
      idRol: this.usuarioForm.idRol
    };

    try {

      const respuesta =
        await this.authService.registrarUsuario(payload);

      if (respuesta?.esExitoso) {

        this.showFormModal = false;
        this.showSuccessModal = true;
        await this.cargarUsuarios();
      }

    } catch (error) {
      console.error(error);
    }

    this.cargando = false;
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
}
