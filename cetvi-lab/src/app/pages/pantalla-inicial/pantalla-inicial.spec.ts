import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PantallaInicial } from './pantalla-inicial';

describe('PantallaInicial', () => {
  let component: PantallaInicial;
  let fixture: ComponentFixture<PantallaInicial>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PantallaInicial]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PantallaInicial);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
