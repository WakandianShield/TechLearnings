// Scroll suave para navegación interna
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
  anchor.addEventListener('click', function(e) {
    const target = document.querySelector(this.getAttribute('href'));
    if (target) {
      e.preventDefault();
      target.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  });
});

// Fade-in Animation con Intersection Observer
const faders = document.querySelectorAll('.fade-in');
const appearOptions = { threshold: 0.2, rootMargin: "0px 0px -50px 0px" };
const appearOnScroll = new IntersectionObserver(function(entries, observer) {
  entries.forEach(entry => {
    if (!entry.isIntersecting) return;
    entry.target.classList.add('appear');
    observer.unobserve(entry.target);
  });
}, appearOptions);
faders.forEach(fader => { appearOnScroll.observe(fader); });

// Stagger Animation
const staggerElements = document.querySelectorAll('.stagger');
const staggerObserver = new IntersectionObserver((entries) => {
  entries.forEach((entry, i) => {
    if (entry.isIntersecting) {
      setTimeout(() => entry.target.classList.add('appear'), i * 120);
      staggerObserver.unobserve(entry.target);
    }
  });
}, { threshold: 0.2 });
staggerElements.forEach(el => staggerObserver.observe(el));

// Scroll Top Button
const scrollTopBtn = document.createElement('button');
scrollTopBtn.className = 'scroll-top-btn';
scrollTopBtn.setAttribute('aria-label', 'Scroll to top');
scrollTopBtn.innerHTML = '↑';
document.body.appendChild(scrollTopBtn);
window.addEventListener('scroll', () => {
  scrollTopBtn.classList.toggle('show', window.scrollY > 400);
});
scrollTopBtn.addEventListener('click', () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
});

// Animación de números y barras en la sección de estadísticas
function animateNumbers() {
  document.querySelectorAll('.stat-number').forEach((el) => {
    const target = +el.getAttribute('data-count');
    let current = 0;
    const increment = Math.ceil(target / 60);
    function update() {
      current += increment;
      if (current > target) current = target;
      el.textContent = current;
      if (current < target) {
        requestAnimationFrame(update);
      } else {
        // Animar barra
        const bar = el.parentElement.querySelector('.stat-fill');
        if (bar) {
          let percent = target > 100 ? 100 : target;
          bar.style.width = percent + '%';
        }
      }
    }
    update();
  });
}

const statsSection = document.querySelector('.stats');
if (statsSection) {
  const statsObserver = new IntersectionObserver(function(entries, observer) {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        animateNumbers();
        observer.unobserve(entry.target);
      }
    });
  }, { threshold: 0.3 });
  statsObserver.observe(statsSection);
}

// Portfolio Slider
const portfolioGrid = document.querySelector('.portfolio-grid');
const prevBtn = document.querySelector('.portfolio-slider .prev');
const nextBtn = document.querySelector('.portfolio-slider .next');
if (portfolioGrid && prevBtn && nextBtn) {
  prevBtn.addEventListener('click', () => {
    portfolioGrid.scrollBy({ left: -300, behavior: 'smooth' });
  });
  nextBtn.addEventListener('click', () => {
    portfolioGrid.scrollBy({ left: 300, behavior: 'smooth' });
  });
}

// Reviews Slider
const reviewsSlider = document.querySelector('.reviews-slider');
const reviewsCards = reviewsSlider ? reviewsSlider.querySelectorAll('.review-card') : [];
let reviewCurrent = 0;
function showReview(idx) {
  reviewsCards.forEach((card, i) => card.classList.toggle('active', i === idx));
}
showReview(reviewCurrent);

const reviewsPrev = document.querySelector('.reviews-prev');
const reviewsNext = document.querySelector('.reviews-next');
if (reviewsPrev && reviewsNext) {
  reviewsPrev.addEventListener('click', () => {
    reviewCurrent = (reviewCurrent - 1 + reviewsCards.length) % reviewsCards.length;
    showReview(reviewCurrent);
  });
  reviewsNext.addEventListener('click', () => {
    reviewCurrent = (reviewCurrent + 1) % reviewsCards.length;
    showReview(reviewCurrent);
  });
}
// Auto rotación reviews cada 6 segundos
if (reviewsCards.length > 1) {
  setInterval(() => {
    reviewCurrent = (reviewCurrent + 1) % reviewsCards.length;
    showReview(reviewCurrent);
  }, 3000);
}

// Hover 3D en cards
function add3DHover(selector) {
  document.querySelectorAll(selector).forEach(card => {
    card.addEventListener('mousemove', e => {
      const rect = card.getBoundingClientRect();
      const x = e.clientX - rect.left;
      const y = e.clientY - rect.top;
      const centerX = rect.width / 2;
      const centerY = rect.height / 2;
      const rotateX = ((y - centerY) / centerY) * 8;
      const rotateY = ((x - centerX) / centerX) * -8;
      card.style.transform = `scale(1.04) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
    });
    card.addEventListener('mouseleave', () => {
      card.style.transform = '';
    });
  });
}
add3DHover('.portfolio-item');
add3DHover('.plan-item');
add3DHover('.feature-item');
add3DHover('.team-member');
add3DHover('.blog-post');
add3DHover('.tech-card');

// Technologies Interactive
document.querySelectorAll('.tech-card').forEach(card => {
  card.addEventListener('mouseenter', () => {
    card.classList.add('show-desc');
  });
  card.addEventListener('mouseleave', () => {
    card.classList.remove('show-desc');
  });
  card.addEventListener('click', () => {
    card.classList.toggle('show-desc');
  });
});

// Hero Canvas Particles (simple effect)
const canvas = document.getElementById('hero-particles');
if (canvas) {
  const ctx = canvas.getContext('2d');
  let w = window.innerWidth, h = document.querySelector('.hero').offsetHeight;
  canvas.width = w; canvas.height = h;
  window.addEventListener('resize', () => {
    w = window.innerWidth;
    h = document.querySelector('.hero').offsetHeight;
    canvas.width = w; canvas.height = h;
  });
  const particles = Array.from({length: 40}, () => ({
    x: Math.random() * w,
    y: Math.random() * h,
    r: Math.random() * 3 + 2,
    dx: (Math.random() - 0.5) * 0.7,
    dy: (Math.random() - 0.5) * 0.7
  }));
  function drawParticles() {
    ctx.clearRect(0,0,w,h);
    particles.forEach(p => {
      ctx.beginPath();
      ctx.arc(p.x, p.y, p.r, 0, Math.PI*2);
      ctx.fillStyle = 'rgba(99,91,255,0.18)';
      ctx.fill();
      p.x += p.dx; p.y += p.dy;
      if (p.x < 0 || p.x > w) p.dx *= -1;
      if (p.y < 0 || p.y > h) p.dy *= -1;
    });
    requestAnimationFrame(drawParticles);
  }
  drawParticles();
}

// Footer animación de entrada
const footer = document.querySelector('.footer');
if (footer) {
  footer.style.opacity = 0;
  footer.style.transform = 'translateY(40px)';
  window.addEventListener('load', () => {
    setTimeout(() => {
      footer.style.transition = 'opacity 1s cubic-bezier(.4,0,.2,1), transform 1s cubic-bezier(.4,0,.2,1)';
      footer.style.opacity = 1;
      footer.style.transform = 'none';
    }, 400);
  });
}

// Planes: click en card abre Stripe link
document.querySelectorAll('.plan-item').forEach(card => {
  card.addEventListener('click', () => {
    const link = card.getAttribute('data-link');
    if (link) window.open(link, '_blank');
  });
});

// Purchase: redirige según método (si tienes formulario purchaseForm)
const purchaseForm = document.getElementById('purchaseForm');
if (purchaseForm) {
  purchaseForm.addEventListener('submit', function(e) {
    e.preventDefault();
    const plan = this.plan.value;
    const payment = this.payment.value;
    let url = '';
    if (payment === 'card' || payment === 'applepay') {
      if (plan === 'essential') url = 'https://buy.stripe.com/test_essential';
      if (plan === 'professional') url = 'https://buy.stripe.com/test_professional';
      if (plan === 'premium') url = 'https://buy.stripe.com/test_premium';
    }
    if (payment === 'paypal') {
      url = 'https://www.paypal.com/paypalme/credaweb';
    }
    if (url) window.open(url, '_blank');
  });
}

// Loading screen fade out tras 3 segundos
window.addEventListener('load', () => {
  const loadingScreen = document.getElementById('loading-screen');
  if (loadingScreen) {
    setTimeout(() => {
      loadingScreen.style.transition = 'opacity 0.5s ease';
      loadingScreen.style.opacity = '0';
      setTimeout(() => {
        loadingScreen.style.display = 'none';
      }, 500);
    }, 1000);
  }
});

// Menú hamburguesa: cerrar menú al hacer click en enlace (móvil)
const burgerCheckbox = document.getElementById('burger-toggle');
const navList = document.querySelector('.nav-list');

if (burgerCheckbox && navList) {
  navList.querySelectorAll('a').forEach(link => {
    link.addEventListener('click', () => {
      if (window.innerWidth <= 900 && burgerCheckbox.checked) {
        burgerCheckbox.checked = false;
      }
    });
  });

  // Cerrar menú si se redimensiona a escritorio (evita menú abierto visible)
  window.addEventListener('resize', () => {
    if (window.innerWidth > 900 && burgerCheckbox.checked) {
      burgerCheckbox.checked = false;
    }
  });
}
