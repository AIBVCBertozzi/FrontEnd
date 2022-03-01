import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModificaTorneoComponent } from './modifica-torneo.component';

describe('ModificaTorneoComponent', () => {
  let component: ModificaTorneoComponent;
  let fixture: ComponentFixture<ModificaTorneoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModificaTorneoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModificaTorneoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
