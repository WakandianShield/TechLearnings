const datos = [
    {
        tipo: "plant",
        nombre: "BR BR PATAPIM",
        imagen: "plant.png",
        vida: 100,
        ataque: 20,
        defensa: 10
    },
    {
        tipo: "fire",
        nombre: "TUNG TUNG TUNG SAHUR",
        imagen: "fire.png",
        vida: 100,
        ataque: 25,
        defensa: 15
    },
    {
        tipo: "water",
        nombre: "TRALALERO TRALALA",
        imagen: "water.png",
        vida: 100,
        ataque: 15,
        defensa: 20
    }
];

let jugadorActual = null;
let enemigoActual = null;
let turnoJugador = true;

function getRandomEnemyIndex(excludeIndex) {
    let idx;
    do {
        idx = Math.floor(Math.random() * datos.length);
    } while (idx === excludeIndex);
    return idx;
}

function calcularResultado(playerType, enemyType) {
    if (playerType === enemyType) return 1;
    if (
        (playerType === "fire" && enemyType === "plant") ||
        (playerType === "plant" && enemyType === "water") ||
        (playerType === "water" && enemyType === "fire")
    ) {
        return 1.5;
    }
    return 0.5;
}

function atacar(atacante, defensor, tipoAtaque) {
    const multiplicador = calcularResultado(tipoAtaque, defensor.tipo);
    let daño = atacante.ataque * multiplicador - defensor.defensa * 0.3;
    if (daño < 1) daño = 1;
    defensor.vida -= daño;
    if (defensor.vida < 0) defensor.vida = 0;
    return Math.floor(daño);
}

function turnoEnemigo() {
    const vsp = document.getElementById("vs");
    const Resultados = document.getElementById("results");
    const ResultScreen = document.getElementById("ResultsScreen");
    const CombatScreen = document.getElementById("CombatScreen");

    ResultScreen.style.display = "none";

    if (enemigoActual.vida <= 0 || jugadorActual.vida <= 0) return;

    const tipos = ["fire", "water", "plant"];
    const ataqueRandom = tipos[Math.floor(Math.random() * tipos.length)];

    const daño = atacar(enemigoActual, jugadorActual, ataqueRandom);

    vsp.textContent = `Enemigo usó ${ataqueRandom.toUpperCase()} e hizo ${daño} de daño. Tu vida: ${jugadorActual.vida}`;

    if (jugadorActual.vida <= 0) {
        CombatScreen.style.display = "none";
        Resultados.textContent = "PERDISTE NUB DAS UN CHINGO DE ASCO";
        ResultScreen.style.display = "flex";
        ResultScreen.querySelector("img").src = "defeat.webp";
        return;
    }

    turnoJugador = true;
}

function realizarAtaque(tipoAtaque) {
    const vsp = document.getElementById("vs");
    const Resultados = document.getElementById("results");
    const ResultScreen = document.getElementById("ResultsScreen");
    const CombatScreen = document.getElementById("CombatScreen");
    const resultImg = ResultScreen.querySelector("img");

    ResultScreen.style.display = "none";

    if (!turnoJugador) return;

    const daño = atacar(jugadorActual, enemigoActual, tipoAtaque);

    vsp.textContent = `Usaste ${tipoAtaque.toUpperCase()} e hiciste ${daño} de daño. Vida enemigo: ${enemigoActual.vida}`;

    if (enemigoActual.vida <= 0) {
        CombatScreen.style.display = "none";
        Resultados.textContent = "GANASTE PRO PLAYER SIGMA CHAD";
        ResultScreen.style.display = "flex";
        resultImg.src = "victory.webp";
        return;
        
    }

    turnoJugador = false;
    setTimeout(turnoEnemigo, 1000);
}

function curar() {
    const vsp = document.getElementById("vs");

    if (!turnoJugador) return;

    jugadorActual.vida += 20;
    if (jugadorActual.vida > 100) jugadorActual.vida = 100;

    vsp.textContent = `Te curaste. Vida actual: ${jugadorActual.vida}`;

    turnoJugador = false;
    setTimeout(turnoEnemigo, 1000);
}

function loadCards() {
    const card = document.querySelectorAll("article");
    const texto = document.querySelectorAll("article p");
    const ChooseScreen = document.getElementById("ChooseChar");
    const CombatScreen = document.getElementById("CombatScreen");
    const ResultsScreen = document.getElementById("ResultsScreen");

    const enemyImg = document.querySelector("#CombatScreen #enemy img");
    const enemyName = document.querySelector("#CombatScreen #enemy p");
    const playerImg = document.querySelector("#CombatScreen #selected img");
    const playerName = document.querySelector("#CombatScreen #selected p");

    CombatScreen.style.display = "none";
    ResultsScreen.style.display = "none";

    card.forEach((card, index) => {
        card.addEventListener("click", () => {
            
            ChooseScreen.style.display = "none";
            CombatScreen.style.display = "grid";

            const enemyIndex = getRandomEnemyIndex(index);
            const enemigo = datos[enemyIndex];
            const personaje = datos[index];

            jugadorActual = { ...personaje };
            enemigoActual = { ...enemigo };

            enemyImg.src = enemigo.imagen;
            enemyImg.alt = enemigo.nombre;
            enemyName.textContent = enemigo.nombre;

            playerImg.src = personaje.imagen;
            playerImg.alt = personaje.nombre;
            playerName.textContent = personaje.nombre;

            turnoJugador = true;
        });
    });
}

function actions() {
    const restartBtn = document.getElementById("restart");
    restartBtn.addEventListener("click", restartGame);
    const fireBtn = document.getElementById("actions--fire");
    const waterBtn = document.getElementById("actions--water");
    const plantBtn = document.getElementById("actions--plant");
    const healBtn = document.getElementById("actions--heal");

    fireBtn.addEventListener("click", () => realizarAtaque("fire"));
    waterBtn.addEventListener("click", () => realizarAtaque("water"));
    plantBtn.addEventListener("click", () => realizarAtaque("plant"));
    healBtn.addEventListener("click", () => curar());
}

function restartGame() {
    const ChooseScreen = document.getElementById("ChooseChar");
    const CombatScreen = document.getElementById("CombatScreen");
    const ResultScreen = document.getElementById("ResultsScreen");
    const texto = document.querySelectorAll("article p");
    const vsp = document.getElementById("vs");

    jugadorActual = null;
    enemigoActual = null;
    turnoJugador = true;

    ResultScreen.style.display = "none";
    CombatScreen.style.display = "none";
    ChooseScreen.style.display = "flex";
}

loadCards();
actions();