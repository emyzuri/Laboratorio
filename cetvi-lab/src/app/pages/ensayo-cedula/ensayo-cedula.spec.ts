import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnsayoCedula } from './ensayo-cedula';

describe('EnsayoCedula', () => {
  let component: EnsayoCedula;
  let fixture: ComponentFixture<EnsayoCedula>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnsayoCedula]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnsayoCedula);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
