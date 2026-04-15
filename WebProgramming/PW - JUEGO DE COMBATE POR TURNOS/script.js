const datos = [
    {
        tipo: "plant",
        nombre: "BR BR PATAPIM",
        imagen: "assets/plant.png",
        vida: 100,
        ataque: 20,
        defensa: 10
    },
    {
        tipo: "fire",
        nombre: "TUNG TUNG TUNG SAHUR",
        imagen: "assets/fire.png",
        vida: 100,
        ataque: 25,
        defensa: 15
    },
    {
        tipo: "water",
        nombre: "TRALALERO TRALALA",
        imagen: "assets/water.png",
        vida: 100,
        ataque: 15,
        defensa: 20
    }
];

const acciones = [
    { tipo: "fire",  emoji: "🔥", label: "Fire"  },
    { tipo: "water", emoji: "💧", label: "Water" },
    { tipo: "plant", emoji: "🌿", label: "Plant" },
    { tipo: "heal",  emoji: "💊", label: "Heal"  }
];

let jugadorActual = null;
let enemigoActual  = null;
let turnoJugador   = true;
let victorias      = 0;


function actualizarHP(quien, vida) {
    const porcentaje = Math.max(0, vida);
    const barra  = document.getElementById(`${quien}-hp-bar`);
    const texto  = document.getElementById(`${quien}-hp-text`);

    barra.style.width = porcentaje + "%";

    barra.classList.remove("medium", "low");
    if (porcentaje <= 25) {
        barra.classList.add("low");
    } else if (porcentaje <= 50) {
        barra.classList.add("medium");
    }

    texto.textContent = porcentaje;
}

function animarDano(quien) {
    const img = document.getElementById(`${quien}-img`);
    img.classList.remove("shake");
    void img.offsetWidth;
    img.classList.add("shake");
}

function actualizarContadorVictorias() {
    const contador = document.getElementById("win-counter-text");
    contador.textContent = `Victorias: ${victorias}`;
}

function generarTarjetas() {
    const ChooseScreen = document.getElementById("ChooseChar");

    const tarjetasViejas = ChooseScreen.querySelectorAll("article");
    tarjetasViejas.forEach(t => t.remove());

    datos.forEach((personaje, index) => {
        const tarjeta = document.createElement("article");
        tarjeta.classList.add(personaje.tipo);

        const imagen = document.createElement("img");
        imagen.src = personaje.imagen;
        imagen.alt = `Imagen de ${personaje.nombre}`;

        const nombre = document.createElement("p");
        nombre.textContent = personaje.nombre;

        tarjeta.appendChild(imagen);
        tarjeta.appendChild(nombre);

        tarjeta.addEventListener("click", () => seleccionarPersonaje(index));

        ChooseScreen.appendChild(tarjeta);
    });
}

function generarBotonesAccion() {
    const contenedor = document.getElementById("actions");

    contenedor.innerHTML = "";

    acciones.forEach(accion => {
        const boton = document.createElement("button");
        boton.id = `actions--${accion.tipo}`;
        boton.classList.add("action-btn", accion.tipo);
        boton.textContent = accion.emoji;
        boton.title = accion.label;

        if (accion.tipo === "heal") {
            boton.addEventListener("click", curar);
        } else {
            boton.addEventListener("click", () => realizarAtaque(accion.tipo));
        }

        contenedor.appendChild(boton);
    });
}

function calcularMultiplicador(tipoAtacante, tipoDefensor) {
    if (tipoAtacante === tipoDefensor) return 1;
    if (
        (tipoAtacante === "fire"  && tipoDefensor === "plant") ||
        (tipoAtacante === "plant" && tipoDefensor === "water") ||
        (tipoAtacante === "water" && tipoDefensor === "fire")
    ) {
        return 1.5; 
    }
    return 0.5; 
}

function atacar(atacante, defensor, tipoAtaque) {
    const multiplicador = calcularMultiplicador(tipoAtaque, defensor.tipo);
    let daño = atacante.ataque * multiplicador - defensor.defensa * 0.3;
    if (daño < 1) daño = 1;
    defensor.vida -= daño;
    if (defensor.vida < 0) defensor.vida = 0;
    return Math.floor(daño);
}

function realizarAtaque(tipoAtaque) {
    if (!turnoJugador) return;

    const daño = atacar(jugadorActual, enemigoActual, tipoAtaque);
    animarDano("enemy");
    actualizarHP("enemy", enemigoActual.vida);

    const log = document.getElementById("battle-log");
    log.textContent = `Usaste ${tipoAtaque.toUpperCase()} → ${daño} dmg al enemigo (HP: ${enemigoActual.vida})`;

    if (enemigoActual.vida <= 0) {
        terminarCombate(true);
        return;
    }

    turnoJugador = false;
    setTimeout(turnoEnemigo, 1000);
}

function curar() {
    if (!turnoJugador) return;

    jugadorActual.vida += 20;
    if (jugadorActual.vida > 100) jugadorActual.vida = 100;

    actualizarHP("player", jugadorActual.vida);

    const log = document.getElementById("battle-log");
    log.textContent = `Te curaste → HP actual: ${jugadorActual.vida}`;

    turnoJugador = false;
    setTimeout(turnoEnemigo, 1000);
}

function turnoEnemigo() {
    if (enemigoActual.vida <= 0 || jugadorActual.vida <= 0) return;

    const tipos = ["fire", "water", "plant"];
    const ataqueRandom = tipos[Math.floor(Math.random() * tipos.length)];

    const daño = atacar(enemigoActual, jugadorActual, ataqueRandom);
    animarDano("player");
    actualizarHP("player", jugadorActual.vida);

    const log = document.getElementById("battle-log");
    log.textContent = `Enemigo usó ${ataqueRandom.toUpperCase()} → ${daño} dmg a ti (HP: ${jugadorActual.vida})`;

    if (jugadorActual.vida <= 0) {
        terminarCombate(false);
        return;
    }

    turnoJugador = true;
}

function seleccionarPersonaje(index) {
    const ChooseScreen  = document.getElementById("ChooseChar");
    const CombatScreen  = document.getElementById("CombatScreen");

    let enemyIndex;
    do {
        enemyIndex = Math.floor(Math.random() * datos.length);
    } while (enemyIndex === index);

    jugadorActual = { ...datos[index] };
    enemigoActual  = { ...datos[enemyIndex] };

    document.getElementById("player-img").src  = jugadorActual.imagen;
    document.getElementById("player-img").alt  = jugadorActual.nombre;
    document.getElementById("player-name").textContent = jugadorActual.nombre;

    document.getElementById("enemy-img").src   = enemigoActual.imagen;
    document.getElementById("enemy-img").alt   = enemigoActual.nombre;
    document.getElementById("enemy-name").textContent = enemigoActual.nombre;

    actualizarHP("player", jugadorActual.vida);
    actualizarHP("enemy",  enemigoActual.vida);

    document.getElementById("battle-log").textContent = "¡Que empiece el combate!";

    turnoJugador = true;

    ChooseScreen.style.display  = "none";
    CombatScreen.style.display  = "grid";
}

function terminarCombate(gano) {
    const CombatScreen  = document.getElementById("CombatScreen");
    const ResultsScreen = document.getElementById("ResultsScreen");
    const resultados    = document.getElementById("results");
    const resultImg     = document.getElementById("result-img");

    CombatScreen.style.display = "none";

    if (gano) {
        victorias++;
        actualizarContadorVictorias();
        resultados.textContent = "GANASTE PRO PLAYER SIGMA CHAD";
        resultImg.src = "assets/victory.webp";
    } else {
        resultados.textContent = "PERDISTE NUB DAS UN CHINGO DE ASCO";
        resultImg.src = "assets/defeat.webp";
    }

    ResultsScreen.style.display = "flex";
}

function reiniciarJuego() {
    const ChooseScreen  = document.getElementById("ChooseChar");
    const CombatScreen  = document.getElementById("CombatScreen");
    const ResultsScreen = document.getElementById("ResultsScreen");

    jugadorActual = null;
    enemigoActual  = null;
    turnoJugador   = true;

    ResultsScreen.style.display = "none";
    CombatScreen.style.display  = "none";
    ChooseScreen.style.display  = "flex";
}

function init() {
    generarTarjetas();
    generarBotonesAccion();
    document.getElementById("CombatScreen").style.display  = "none";
    document.getElementById("ResultsScreen").style.display = "none";
    document.getElementById("restart").addEventListener("click", reiniciarJuego);
}

document.addEventListener("DOMContentLoaded", init);