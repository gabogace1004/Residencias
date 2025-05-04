let slideIndex = 1; // Empezamos en 1, no en 0
let slides, dots, timer;

document.addEventListener("DOMContentLoaded", function () {
    slides = document.getElementsByClassName("mySlides");
    dots = document.getElementsByClassName("dot");
    showSlides(slideIndex); // Cargar la primera imagen bien
    autoSlide(); // Arrancar el automático
});

function showSlides(n) {
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }

    // Ocultar todas
    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (let i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    // Mostrar solo la que corresponde
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}

function plusSlides(n) {
    clearTimeout(timer); // Limpiar temporizador anterior
    showSlides(slideIndex += n);
    autoSlide(); // Reiniciar automático
}

function currentSlide(n) {
    clearTimeout(timer);
    showSlides(slideIndex = n);
    autoSlide();
}

function autoSlide() {
    timer = setTimeout(function () {
        plusSlides(1);
    }, 2000);
}


// ⏸ Pausar cuando el mouse entra
function pauseSlide() {
    clearTimeout(timer);
}

// ▶️ Reanudar cuando el mouse sale
function resumeSlide() {
    autoSlide();
}

// Agregar eventos al contenedor
document.addEventListener("DOMContentLoaded", function () {
    const container = document.querySelector(".slideshow-container");
    if (container) {
        container.addEventListener("mouseenter", pauseSlide);
        container.addEventListener("mouseleave", resumeSlide);
    }
});