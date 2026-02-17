import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BuscarEnsayosComponent } from './buscar-ensayos';

describe('BuscarEnsayos', () => {
  let component: BuscarEnsayosComponent;
  let fixture: ComponentFixture<BuscarEnsayosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuscarEnsayosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuscarEnsayosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
