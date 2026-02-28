// scripts.js

document.addEventListener('DOMContentLoaded', () => {
  // ===== Burger Menu =====
  const burger = document.querySelector('.burger');
  const navLinks = document.querySelector('.nav-links');
  const navMenu = document.getElementById('nav-menu');

  burger.addEventListener('click', () => {
    const expanded = burger.getAttribute('aria-expanded') === 'true' || false;
    burger.setAttribute('aria-expanded', !expanded);

    if (!expanded) {
      navLinks.classList.add('open');
      navMenu.setAttribute('aria-hidden', 'false');
      // Trap focus inside menu
      navLinks.querySelectorAll('a').forEach(link => {
        link.setAttribute('tabindex', '0');
      });
      // Focus first menu item
      navLinks.querySelector('a').focus();
    } else {
      navLinks.classList.remove('open');
      navMenu.setAttribute('aria-hidden', 'true');
      // Remove tabindex to hide from tab order when closed
      navLinks.querySelectorAll('a').forEach(link => {
        link.setAttribute('tabindex', '-1');
      });
      burger.focus();
    }
  });

  // Close menu when link clicked (mobile)
  navLinks.querySelectorAll('a').forEach(link => {
    link.addEventListener('click', () => {
      if (navLinks.classList.contains('open')) {
        navLinks.classList.remove('open');
        navMenu.setAttribute('aria-hidden', 'true');
        burger.setAttribute('aria-expanded', false);
        navLinks.querySelectorAll('a').forEach(link => {
          link.setAttribute('tabindex', '-1');
        });
        burger.focus();
      }
    });
  });

  // ===== Smooth Scroll =====
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
      e.preventDefault();
      const target = document.querySelector(this.getAttribute('href'));
      if (target) {
        target.scrollIntoView({ behavior: 'smooth' });
      }
    });
  });

  // ===== Reservation Form Submission Simulation =====
  const form = document.querySelector('.reservation-form');
  form.addEventListener('submit', e => {
    e.preventDefault();

    // Simulate POST (fake)
    setTimeout(() => {
      alert('Thank you! Your reservation has been received.');
      form.reset();
    }, 600);
  });

  // ===== Hero Canvas Particle Animation (Complex) =====
  const canvas = document.getElementById('hero-canvas');
  if (!canvas) return;
  const ctx = canvas.getContext('2d');
  let width, height, particles;

  class Particle {
    constructor(x, y, radius, speedX, speedY) {
      this.x = x;
      this.y = y;
      this.radius = radius;
      this.speedX = speedX;
      this.speedY = speedY;
      this.baseX = x;
      this.baseY = y;
      this.angle = Math.random() * 2 * Math.PI;
      this.amplitude = Math.random() * 20 + 10;
      this.angleSpeed = 0.02 + Math.random() * 0.02;
    }

    update() {
      // Oscillate with sine wave (parallax-like effect)
      this.angle += this.angleSpeed;
      this.x = this.baseX + Math.cos(this.angle) * this.amplitude;
      this.y = this.baseY + Math.sin(this.angle) * this.amplitude;
    }

    draw() {
      ctx.beginPath();
      ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2);
      ctx.fillStyle = 'rgba(255, 255, 255, 0.85)';
      ctx.shadowColor = 'rgba(255, 165, 0, 0.6)';
      ctx.shadowBlur = 8;
      ctx.fill();
      ctx.closePath();
    }
  }

  function initCanvas() {
    width = canvas.width = window.innerWidth;
    height = canvas.height = window.innerHeight > 700 ? 700 : window.innerHeight;
    particles = [];
    const numParticles = Math.floor(width / 15);

    for (let i = 0; i < numParticles; i++) {
      const x = Math.random() * width;
      const y = Math.random() * height;
      const radius = Math.random() * 2 + 1.5;
      const speedX = (Math.random() - 0.5) * 0.1;
      const speedY = (Math.random() - 0.5) * 0.1;
      particles.push(new Particle(x, y, radius, speedX, speedY));
    }
  }

  function drawLines() {
    const maxDistance = 140;
    for (let i = 0; i < particles.length; i++) {
      for (let j = i + 1; j < particles.length; j++) {
        const dx = particles[i].x - particles[j].x;
        const dy = particles[i].y - particles[j].y;
        const dist = Math.sqrt(dx * dx + dy * dy);
        if (dist < maxDistance) {
          ctx.beginPath();
          ctx.strokeStyle = `rgba(255,165,0,${(maxDistance - dist) / maxDistance * 0.4})`;
          ctx.lineWidth = 1;
          ctx.moveTo(particles[i].x, particles[i].y);
          ctx.lineTo(particles[j].x, particles[j].y);
          ctx.shadowColor = 'rgba(255, 165, 0, 0.3)';
          ctx.shadowBlur = 4;
          ctx.stroke();
          ctx.closePath();
        }
      }
    }
  }

  function animate() {
    ctx.clearRect(0, 0, width, height);

    particles.forEach(p => {
      p.update();
      p.draw();
    });

    drawLines();

    requestAnimationFrame(animate);
  }

  window.addEventListener('resize', () => {
    initCanvas();
  });

  initCanvas();
  animate();
});

document.addEventListener('DOMContentLoaded', () => {
  const burger = document.getElementById('burger-btn');
  const navLinks = document.querySelector('.nav-links');
  const navMenu = document.getElementById('nav-menu');

  burger.addEventListener('click', () => {
    const expanded = burger.getAttribute('aria-expanded') === 'true' || false;
    burger.setAttribute('aria-expanded', !expanded);
    if (!expanded) {
      navLinks.classList.add('open');
      navMenu.setAttribute('aria-hidden', 'false');
      // Enable tab on links
      navLinks.querySelectorAll('a').forEach(link => {
        link.setAttribute('tabindex', '0');
      });
      // Focus first link for accessibility
      navLinks.querySelector('a').focus();
    } else {
      navLinks.classList.remove('open');
      navMenu.setAttribute('aria-hidden', 'true');
      // Disable tab on links
      navLinks.querySelectorAll('a').forEach(link => {
        link.setAttribute('tabindex', '-1');
      });
      burger.focus();
    }
  });

  // Close menu on link click (mobile)
  navLinks.querySelectorAll('a').forEach(link => {
    link.addEventListener('click', () => {
      if (navLinks.classList.contains('open')) {
        navLinks.classList.remove('open');
        navMenu.setAttribute('aria-hidden', 'true');
        burger.setAttribute('aria-expanded', false);
        navLinks.querySelectorAll('a').forEach(link => {
          link.setAttribute('tabindex', '-1');
        });
        burger.focus();
      }
    });
  });

  // Smooth scroll for all anchors
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', e => {
      e.preventDefault();
      const target = document.querySelector(anchor.getAttribute('href'));
      if (target) target.scrollIntoView({ behavior: 'smooth' });
    });
  });

  // Initialize tabIndex for links as -1 by default on mobile
  function initTabIndex() {
    if (window.innerWidth <= 900) {
      navLinks.querySelectorAll('a').forEach(link => link.setAttribute('tabindex', '-1'));
      burger.setAttribute('aria-expanded', false);
      navLinks.classList.remove('open');
      navMenu.setAttribute('aria-hidden', 'true');
    } else {
      navLinks.querySelectorAll('a').forEach(link => link.setAttribute('tabindex', '0'));
      burger.setAttribute('aria-expanded', true);
      navLinks.classList.remove('open');
      navMenu.setAttribute('aria-hidden', 'false');
    }
  }

  initTabIndex();
  window.addEventListener('resize', initTabIndex);

  // Aquí iría tu código de fondo animado o el resto de scripts...
});
