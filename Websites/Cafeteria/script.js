document.addEventListener('DOMContentLoaded', () => {
  // Page load animations
  const tl = gsap.timeline();

  // Animate header elements
  tl.from('header .logo', { duration: 1, y: -50, opacity: 0, ease: 'power2.out' });
  tl.from('header .navbar a', { duration: 0.8, y: -30, opacity: 0, stagger: 0.2, ease: 'power2.out' }, '-=0.6');
  tl.from('header .header-icon .bx', { duration: 0.8, y: -30, opacity: 0, stagger: 0.2, ease: 'power2.out' }, '-=0.6');

  // Animate home section text and image
  tl.from('.home-text h1', { duration: 1, x: -100, opacity: 0, ease: 'power2.out' }, '-=0.4');
  tl.from('.home-text p', { duration: 1, x: -100, opacity: 0, ease: 'power2.out' }, '-=0.8');
  tl.from('.home-text .btn', { duration: 1, scale: 0, opacity: 0, ease: 'back.out(1.7)' }, '-=0.8');
  tl.from('.home-img img', { duration: 1.2, scale: 0.8, opacity: 0, ease: 'power2.out' }, '-=1');

  // Scroll-triggered animations
  gsap.utils.toArray('section').forEach(section => {
    gsap.from(section, {
      scrollTrigger: {
        trigger: section,
        start: 'top 80%',
        toggleActions: 'play reverse play reverse',
      },
      y: 50,
      opacity: 0,
      duration: 1,
      ease: 'power2.out',
      stagger: 0.3,
    });
  });

  // Animate product boxes with stagger
  gsap.from('.products-container .box', {
    scrollTrigger: {
      trigger: '.products-container',
      start: 'top 80%',
      toggleActions: 'play reverse play reverse',
    },
    y: 50,
    opacity: 0,
    duration: 1,
    ease: 'power2.out',
    stagger: 0.2,
  });

  // Animate customer boxes with stagger
  gsap.from('.customers-container .box', {
    scrollTrigger: {
      trigger: '.customers-container',
      start: 'top 80%',
      toggleActions: 'play reverse play reverse',
    },
    y: 50,
    opacity: 0,
    duration: 1,
    ease: 'power2.out',
    stagger: 0.2,
  });

  // Animate footer boxes with stagger
  gsap.from('.footer-box', {
    scrollTrigger: {
      trigger: '.footer',
      start: 'top 80%',
      toggleActions: 'play reverse play reverse',
    },
    y: 50,
    opacity: 0,
    duration: 1,
    ease: 'power2.out',
    stagger: 0.2,
  });

  // Mobile menu toggle with GSAP animation
  const menuIcon = document.getElementById('menu-icon');
  const navbar = document.querySelector('.navbar');

  let menuOpen = false;

  menuIcon.addEventListener('click', () => {
    console.log('Menu icon clicked. Current menuOpen:', menuOpen);
    if (!menuOpen) {
      console.log('Opening menu...');
      navbar.style.display = 'flex';
      gsap.to(navbar, { y: 0, autoAlpha: 1, duration: 0.5, ease: 'power2.out' });
      menuIcon.classList.add('active');
      menuOpen = true;
      console.log('Menu opened');
    } else {
      console.log('Closing menu...');
      gsap.to(navbar, { y: '-100%', autoAlpha: 0, duration: 0.5, ease: 'power2.in', onComplete: () => {
        navbar.style.display = 'none';
      }});
      menuIcon.classList.remove('active');
      menuOpen = false;
      console.log('Menu closed');
    }
  });

  // Initialize navbar offscreen and hidden for mobile
  function setMenuState() {
    if (window.innerWidth <= 768) {
      gsap.set(navbar, { y: '-100%', autoAlpha: 0, display: 'none' });
      menuIcon.classList.remove('active');
      menuOpen = false;
    } else {
      gsap.set(navbar, { y: 0, autoAlpha: 1, display: 'flex' });
      menuIcon.classList.remove('active');
      menuOpen = false;
    }
  }

  setMenuState();

  window.addEventListener('resize', () => {
    setMenuState();
  });

  // Hover animation for main image
  const homeImg = document.querySelector('.home-img img');

  homeImg.addEventListener('mouseenter', () => {
    gsap.to(homeImg, { scale: 1.1, rotation: 5, duration: 0.5, ease: 'power2.out' });
  });

  homeImg.addEventListener('mouseleave', () => {
    gsap.to(homeImg, { scale: 1, rotation: 0, duration: 0.5, ease: 'power2.out' });
  });

  // Scroll-triggered animation for main image
  gsap.fromTo(homeImg, 
    { scale: 0.8, opacity: 0 }, 
    { 
      scale: 1, 
      opacity: 1, 
      duration: 1, 
      ease: 'power2.out',
      scrollTrigger: {
        trigger: '.home',
        start: 'top 80%',
        end: 'bottom 20%',
        toggleActions: 'play reverse play reverse',
      }
    }
  );
});
