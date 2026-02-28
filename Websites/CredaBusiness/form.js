// Canvas animated background waves
const canvas = document.getElementById('background-canvas');
const ctx = canvas.getContext('2d');
let width, height;
let waves = [];

function resize() {
  width = window.innerWidth;
  height = window.innerHeight;
  canvas.width = width * devicePixelRatio;
  canvas.height = height * devicePixelRatio;
  canvas.style.width = width + 'px';
  canvas.style.height = height + 'px';
  ctx.setTransform(1, 0, 0, 1, 0, 0);
  ctx.scale(devicePixelRatio, devicePixelRatio);
}

class Wave {
  constructor(y, amplitude, wavelength, speed, color, phase) {
    this.y = y;
    this.amplitude = amplitude;
    this.wavelength = wavelength;
    this.speed = speed;
    this.color = color;
    this.phase = phase;
  }
  draw(time) {
    ctx.beginPath();
    ctx.moveTo(0, this.y);
    for (let x = 0; x <= width; x++) {
      const y =
        this.y +
        this.amplitude *
          Math.sin((x / this.wavelength) * 2 * Math.PI + this.phase + time * this.speed);
      ctx.lineTo(x, y);
    }
    ctx.lineTo(width, height);
    ctx.lineTo(0, height);
    ctx.closePath();
    ctx.fillStyle = this.color;
    ctx.fill();
  }
}

function initWaves() {
  waves = [
    new Wave(height * 0.7, 20, 200, 0.001, 'rgba(99, 91, 255, 0.2)', 0),
    new Wave(height * 0.75, 15, 150, 0.002, 'rgba(99, 91, 255, 0.3)', Math.PI / 2),
    new Wave(height * 0.8, 10, 100, 0.003, 'rgba(99, 91, 255, 0.4)', Math.PI),
  ];
}

function animate(time = 0) {
  ctx.clearRect(0, 0, width, height);
  waves.forEach(wave => wave.draw(time));
  requestAnimationFrame(animate);
}

window.addEventListener('resize', () => {
  resize();
  initWaves();
});
resize();
initWaves();
requestAnimationFrame(animate);

// Play ambient audio on page load (try-catch for autoplay restrictions)
window.addEventListener('load', () => {
  const audio = document.getElementById('ambient-audio');
  audio.volume = 0.15; // low volume
  audio.play().catch(() => {
    // Autoplay blocked, no problem
  });
  
  // Form submission handling with fetch
  const form = document.getElementById('contact-form');
  form.addEventListener('submit', e => {
    e.preventDefault();
    fetch(form.action, {
      method: 'POST',
      headers: { 'Accept': 'application/json' },
      body: new FormData(form),
    }).then(response => {
      if (response.ok) {
        form.reset();
        alert('Thank you for contacting us! You will be redirected to payment.');
        window.location.href = 'https://buy.stripe.com/test_4gw9B05M9cKu8i4cMM';
      } else {
        alert('Oops! There was a problem submitting your form.');
      }
    }).catch(() => {
      alert('Oops! There was a problem submitting your form.');
    });
  });
});




window.addEventListener('load', () => {
  const audio = document.getElementById('ambient-audio');
  const btn = document.getElementById('start-sound-btn');

  btn.addEventListener('click', () => {
    audio.play().catch((e) => {
      console.error('Playback failed:', e);
    });
    btn.style.display = 'none'; // Oculta botón después de click
  });

  // Opcional: intenta autoplay (probablemente bloqueado)
  audio.play().catch(() => {
    // Si autoplay bloqueado, dejamos visible el botón
    console.log('Autoplay prevented, waiting for user to start audio');
  });
});