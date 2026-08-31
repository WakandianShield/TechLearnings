import { Component, input } from '@angular/core';

@Component({
  selector: 'app-imagen-card',
  standalone: true,
  imports: [],
  templateUrl: './imagen-card.html',
  styleUrl: './imagen-card.css',
})
export class ImagenCard {
  imagen = input.required<string>();
  titulo = input.required<string>();
  subtitulo = input<string>('');
}
