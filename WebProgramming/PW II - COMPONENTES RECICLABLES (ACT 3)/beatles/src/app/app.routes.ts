import { Routes } from '@angular/router';
import { Inicio } from './componentes/inicio/inicio';
import { Albumes } from './componentes/albumes/albumes';
import { Frases } from './componentes/frases/frases';
import { Miembros } from './componentes/miembros/miembros';
import { Historia } from './componentes/historia/historia';

export const routes: Routes = [
  { path: '', component: Inicio },
  { path: 'albumes', component: Albumes },
  { path: 'frases', component: Frases },
  { path: 'miembros', component: Miembros },
  { path: 'historia', component: Historia },
];
