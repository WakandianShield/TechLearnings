import { Component } from '@angular/core';
import { ImagenCard } from './imagen-card/imagen-card';

@Component({
  selector: 'app-albumes',
  standalone: true,
  imports: [ImagenCard],
  templateUrl: './albumes.html',
  styleUrl: './albumes.css',
})
export class Albumes {
  archivos = [
    { imagen: 'https://m.media-amazon.com/images/I/819oEzz9axL._UF1000,1000_QL80_.jpg', titulo: 'Please Please Me', subtitulo: '1963 - El comienzo de todo' },
    { imagen: 'https://m.media-amazon.com/images/I/91Nxj5UJZYL.jpg', titulo: 'With The Beatles', subtitulo: '1963 - La Beatlemania explota' },
    { imagen: 'https://m.media-amazon.com/images/I/81eaT-zvdbL.jpg', titulo: "A Hard Day's Night", subtitulo: '1964 - Su primer film y album' },
    { imagen: 'https://m.media-amazon.com/images/I/61txVCHZ5KL._UF1000,1000_QL80_.jpg', titulo: 'Beatles For Sale', subtitulo: '1964 - Folk rock y Navidad' },
    { imagen: 'https://m.media-amazon.com/images/I/71Lo3XMoSlL.jpg', titulo: 'Help!', subtitulo: '1965 - El film y la evolucion' },
    { imagen: 'https://m.media-amazon.com/images/I/91ym3sMcvRL.jpg', titulo: 'Rubber Soul', subtitulo: '1965 - El giro folk-psicodelico' },
    { imagen: 'https://m.media-amazon.com/images/I/91cQm9wh9wL._UF1000,1000_QL80_.jpg', titulo: 'Revolver', subtitulo: '1966 - La revolucion sonora' },
    { imagen: 'https://m.media-amazon.com/images/I/81ZVwENsT-L.jpg', titulo: "Sgt. Pepper's", subtitulo: '1967 - La obra maestra definitiva' },
    { imagen: 'https://m.media-amazon.com/images/I/91Sx7bFxVmL.jpg', titulo: 'Magical Mystery Tour', subtitulo: '1967 - El viaje psicodelico' },
    { imagen: 'https://upload.wikimedia.org/wikipedia/commons/2/20/TheBeatles68LP.jpg', titulo: 'The White Album', subtitulo: '1968 - Doble LP, maximalismo' },
    { imagen: 'https://upload.wikimedia.org/wikipedia/commons/a/a4/The_Beatles_Abbey_Road_album_cover.jpg', titulo: 'Abbey Road', subtitulo: '1969 - El cruce mas famoso' },
    { imagen: 'https://upload.wikimedia.org/wikipedia/commons/7/7a/The_Beatles_-_Let_It_Be.png', titulo: 'Let It Be', subtitulo: '1970 - La despedida final' },
  ];
}
