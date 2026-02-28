// Animación del loading screen con GSAP
class App {
  constructor() {
    const tl = new TimelineMax({ repeat: -1 });
    tl.to(['.pizzaOutline', '.pizzaMask'], 7, {
      rotation: 360,
      svgOrigin: '61 61',
      ease: Linear.easeNone
    })
    .to('.whole', 7, {
      rotation: -45,
      svgOrigin: '61 61',
      ease: Linear.easeNone
    }, 0);
  }
}

TweenMax.set('svg', { visibility: 'visible' });
var app = new App();
TweenMax.globalTimeScale(4);

window.addEventListener('DOMContentLoaded', function() {
  setTimeout(function() {
    const loading = document.getElementById('loading-screen');
    loading.style.opacity = 0;
    setTimeout(() => { loading.style.display = 'none'; }, 700);
  }, 2500);

  animatePizzaTracker();
  blockScrollMenu();
});

// --- Pizza tracker animación de recorrido y escala ---
function animatePizzaTracker() {
  const pizza = document.getElementById('pizza-tracker');
  const pizzaImg = pizza.querySelector('img');
  const section1 = document.getElementById('section1');
  const section2 = document.getElementById('section2');
  const section3 = document.getElementById('section3');
  const docHeight = document.body.scrollHeight - window.innerHeight;

  // Recorrido: centro (hero), luego bordes y zonas libres
  const path = [
    {x: 50, y: 50, w: 45},   // centro pantalla (hero)
    {x: 80, y: 20, w: 20},   // arriba derecha
    {x: 20, y: 80, w: 12},   // abajo izquierda
    {x: 80, y: 80, w: 10},   // abajo derecha
    {x: 20, y: 20, w: 10},   // arriba izquierda
    {x: 90, y: 50, w: 10},   // derecha centro
    {x: 10, y: 50, w: 10},   // izquierda centro
    {x: 50, y: 10, w: 10}    // arriba centro
  ];

  window.addEventListener('scroll', () => {
    const scrollY = window.scrollY;
    const progress = Math.min(scrollY / docHeight, 1);
    // Calcular en qué segmento del path estamos
    const seg = Math.floor(progress * (path.length - 1));
    const segProgress = (progress * (path.length - 1)) - seg;
    const p0 = path[seg];
    const p1 = path[Math.min(seg + 1, path.length - 1)];
    // Interpolación lineal
    const x = p0.x + (p1.x - p0.x) * segProgress;
    const y = p0.y + (p1.y - p0.y) * segProgress;
    const w = p0.w + (p1.w - p0.w) * segProgress;
    pizza.style.left = x + '%';
    pizza.style.top = y + 'vh';
    pizza.style.transform = 'translate(-50%, -50%)';
    pizzaImg.style.width = w + 'vw';
    pizzaImg.style.minWidth = (w * 4) + 'px';
    pizzaImg.style.maxWidth = (w * 16) + 'px';
  });
  // Inicializar posición y tamaño
  pizza.style.left = '50%';
  pizza.style.top = '50%';
  pizza.style.transform = 'translate(-50%, -50%)';
  pizzaImg.style.width = '45vw';
  pizzaImg.style.minWidth = '180px';
  pizzaImg.style.maxWidth = '600px';
}

// --- Scroll horizontal bloqueante SOLO cuando la sección menú está centrada ---
function blockScrollMenu() {
  const section = document.getElementById('section3');
  const menu = section.querySelector('.menu-scroll');
  let isBlocking = false;
  let lastScrollY = 0;
  let lastTouchX = null;

  function onScroll(e) {
    const rect = section.getBoundingClientRect();
    const windowHeight = window.innerHeight;
    const sectionCenter = rect.top + rect.height / 2;
    const viewportCenter = windowHeight / 2;
    // Solo activar scroll horizontal si la sección está centrada
    if (Math.abs(sectionCenter - viewportCenter) < rect.height / 4) {
      // Bloquear scroll vertical
      if (!isBlocking) {
        isBlocking = true;
        lastScrollY = window.scrollY;
        document.body.style.overflow = 'hidden';
        window.scrollTo(0, lastScrollY); // Fijar scroll
        window.addEventListener('wheel', horizontalScroll, { passive: false });
        window.addEventListener('touchstart', onTouchStart, { passive: false });
        window.addEventListener('touchmove', onTouchMove, { passive: false });
      }
    } else {
      // Fuera de la zona de bloqueo
      if (isBlocking) {
        isBlocking = false;
        document.body.style.overflow = '';
        window.removeEventListener('wheel', horizontalScroll);
        window.removeEventListener('touchstart', onTouchStart);
        window.removeEventListener('touchmove', onTouchMove);
      }
    }
  }

  function horizontalScroll(e) {
    e.preventDefault();
    const maxScroll = menu.scrollWidth - menu.clientWidth;
    if (e.deltaY > 0) {
      menu.scrollLeft += 40;
    } else if (e.deltaY < 0) {
      menu.scrollLeft -= 40;
    }
    // Si llegó al final, desbloquear scroll vertical
    if (menu.scrollLeft >= maxScroll) {
      document.body.style.overflow = '';
      window.removeEventListener('wheel', horizontalScroll);
      window.removeEventListener('touchstart', onTouchStart);
      window.removeEventListener('touchmove', onTouchMove);
      isBlocking = false;
    }
  }

  function onTouchStart(e) {
    if (e.touches && e.touches.length === 1) {
      lastTouchX = e.touches[0].clientX;
    }
  }
  function onTouchMove(e) {
    if (e.touches && e.touches.length === 1 && lastTouchX !== null) {
      const deltaX = lastTouchX - e.touches[0].clientX;
      menu.scrollLeft += deltaX;
      lastTouchX = e.touches[0].clientX;
      e.preventDefault();
      // Si llegó al final, desbloquear scroll vertical
      const maxScroll = menu.scrollWidth - menu.clientWidth;
      if (menu.scrollLeft >= maxScroll) {
        document.body.style.overflow = '';
        window.removeEventListener('wheel', horizontalScroll);
        window.removeEventListener('touchstart', onTouchStart);
        window.removeEventListener('touchmove', onTouchMove);
        isBlocking = false;
      }
    }
  }

  window.addEventListener('scroll', onScroll);
}
