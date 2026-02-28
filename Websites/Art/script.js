// ======= TOGGLE BURGER MENU =======
const navToggle = document.getElementById('nav-toggle');
const navMenu = document.getElementById('nav-menu');

navToggle.addEventListener('click', () => {
  document.body.classList.toggle('nav-open');
  const expanded = navToggle.getAttribute('aria-expanded') === 'true' || false;
  navToggle.setAttribute('aria-expanded', !expanded);
});

// Close nav menu on link click (mobile)
document.querySelectorAll('.nav-link').forEach(link => {
  link.addEventListener('click', () => {
    document.body.classList.remove('nav-open');
    navToggle.setAttribute('aria-expanded', false);
  });
});


// ======= TESTIMONIAL CAROUSEL =======
let testimonialIndex = 0;
const testimonials = document.querySelectorAll('.testimonial-item');

function showTestimonial(index) {
  testimonials.forEach(t => t.classList.remove('active'));
  testimonials[index].classList.add('active');
}

function nextTestimonial() {
  testimonialIndex = (testimonialIndex + 1) % testimonials.length;
  showTestimonial(testimonialIndex);
}

// Auto-rotate every 5 seconds
setInterval(nextTestimonial, 5000);


// ======= SCROLL ANIMATIONS =======
const scrollElements = document.querySelectorAll(
  '.fade-in-up, .fade-in-left, .fade-in-right'
);

const scrollInView = el => {
  const rect = el.getBoundingClientRect();
  return (
    rect.top <= window.innerHeight - 100
  );
};

function handleScroll() {
  scrollElements.forEach(el => {
    if (scrollInView(el)) {
      el.classList.add('show');
    }
  });
}

window.addEventListener('scroll', handleScroll);
window.addEventListener('load', handleScroll);





// ======= GOOGLE MAP INIT =======
function initMap() {
  const studioLocation = { lat: 19.432608, lng: -99.133209 }; // CDMX ejemplo

  const map = new google.maps.Map(document.getElementById("map"), {
    zoom: 14,
    center: studioLocation,
    disableDefaultUI: true,
    styles: [
      {
        featureType: "all",
        elementType: "geometry.fill",
        stylers: [{ color: "#f2f2f2" }]
      },
      {
        featureType: "all",
        elementType: "labels.text.fill",
        stylers: [{ color: "#3f3cbb" }]
      }
    ]
  });

  const marker = new google.maps.Marker({
    position: studioLocation,
    map,
    title: "Artiscope Studio"
  });
}
