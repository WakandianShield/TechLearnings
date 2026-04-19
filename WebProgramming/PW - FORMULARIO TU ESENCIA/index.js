const INPUT_TO_CARD = {
  "i-mbti": "mbti",
  "i-food": "food",
  "i-character": "character",
  "i-color": "color",
  "i-place": "place",
  "i-season": "season",
  "i-hobby": "hobby",
  "i-music": "music",
  "i-career": "career",
  "i-flower": "flower",
  "i-animal": "animal",
  "i-app": "app",
  "i-you": "you",
};

const CARD_IDS = [...new Set(Object.values(INPUT_TO_CARD))];

const previewObjectUrls = new Map();
let colorRequestId = 0;

function normalizeName(value) {
  return value.trim().replace(/\s+/g, " ").toUpperCase();
}

function getCardImageNode(cardId) {
  const card = document.getElementById(cardId);
  return card ? card.querySelector("img") : null;
}

function setCardImage(cardId, source) {
  const card = document.getElementById(cardId);
  const image = getCardImageNode(cardId);
  if (!image || !card) {
    return;
  }

  image.src = source;
  card.classList.toggle("has-image", Boolean(source));
}

function revokePreviewUrl(cardId) {
  const previewUrl = previewObjectUrls.get(cardId);
  if (!previewUrl) {
    return;
  }
  URL.revokeObjectURL(previewUrl);
  previewObjectUrls.delete(cardId);
}

function setCardImageFromFile(cardId, file) {
  revokePreviewUrl(cardId);
  const previewUrl = URL.createObjectURL(file);
  previewObjectUrls.set(cardId, previewUrl);
  setCardImage(cardId, previewUrl);
}

function setFieldError(inputId, message) {
  const errorLabel = document.getElementById(`${inputId}-error`);
  if (!errorLabel) {
    return;
  }

  errorLabel.textContent = message;
  errorLabel.style.display = message ? "inline" : "none";
}

function clearFieldError(inputId) {
  setFieldError(inputId, "");
}

function calculateAge(dateValue) {
  const birthDate = new Date(`${dateValue}T00:00:00`);
  if (Number.isNaN(birthDate.getTime())) {
    return null;
  }

  const today = new Date();
  let age = today.getFullYear() - birthDate.getFullYear();
  const hasBirthdayPassed =
    today.getMonth() > birthDate.getMonth() ||
    (today.getMonth() === birthDate.getMonth() &&
      today.getDate() >= birthDate.getDate());

  if (!hasBirthdayPassed) {
    age -= 1;
  }

  return age;
}

function getZodiacSign(dateValue) {
  const birthDate = new Date(`${dateValue}T00:00:00`);
  if (Number.isNaN(birthDate.getTime())) {
    return "";
  }

  const month = birthDate.getMonth() + 1;
  const day = birthDate.getDate();

  if ((month === 1 && day >= 20) || (month === 2 && day <= 18)) return "AQUARIUS";
  if ((month === 2 && day >= 19) || (month === 3 && day <= 20)) return "PISCIS";
  if ((month === 3 && day >= 21) || (month === 4 && day <= 19)) return "ARIES";
  if ((month === 4 && day >= 20) || (month === 5 && day <= 20)) return "TAURO";
  if ((month === 5 && day >= 21) || (month === 6 && day <= 20)) return "GEMINIS";
  if ((month === 6 && day >= 21) || (month === 7 && day <= 22)) return "CANCER";
  if ((month === 7 && day >= 23) || (month === 8 && day <= 22)) return "LEO";
  if ((month === 8 && day >= 23) || (month === 9 && day <= 22)) return "VIRGO";
  if ((month === 9 && day >= 23) || (month === 10 && day <= 22)) return "LIBRA";
  if ((month === 10 && day >= 23) || (month === 11 && day <= 21)) return "ESCORPION";
  if ((month === 11 && day >= 22) || (month === 12 && day <= 21)) return "SAGITARIO";
  return "CAPRICORNIO";
}

function updatePersonalInfo(nameValue, birthDateValue) {
  const nameLabel = document.getElementById("name");
  const ageLabel = document.getElementById("age");
  const pronounLabel = document.getElementById("pronoun");
  const zodiacLabel = document.getElementById("zodiac");

  if (nameLabel) {
    nameLabel.textContent = normalizeName(nameValue);
  }

  const age = calculateAge(birthDateValue);
  if (ageLabel) {
    ageLabel.textContent = age === null ? "AGE" : `${age} YO`;
  }

  if (zodiacLabel) {
    zodiacLabel.textContent = `${getZodiacSign(birthDateValue)}`;
  }
}

function resetPersonalInfo() {
  const nameLabel = document.getElementById("name");
  const ageLabel = document.getElementById("age");
  const zodiacLabel = document.getElementById("zodiac");

  if (nameLabel) nameLabel.textContent = "NAME";
  if (ageLabel) ageLabel.textContent = "AGE";
  if (zodiacLabel) zodiacLabel.textContent = "ZODIAC";
}

function applyDominantColor(hexColor) {
  const colorCard = document.getElementById("color");
  if (!colorCard) {
    return;
  }

  colorCard.style.boxShadow = `inset 0 0 0 3px ${hexColor}`;
  const label = colorCard.querySelector("span");
  if (label) {
    label.textContent = `Color ${hexColor.toUpperCase()}`;
  }
}

function clearDominantColor() {
  const colorCard = document.getElementById("color");
  if (!colorCard) {
    return;
  }

  colorCard.style.boxShadow = "";
  const label = colorCard.querySelector("span");
  if (label) {
    label.textContent = "Color";
  }
}

function getDominantColorFromImage(file) {
  return new Promise((resolve, reject) => {
    const image = new Image();
    const imageUrl = URL.createObjectURL(file);

    image.onload = () => {
      const canvas = document.createElement("canvas");
      const context = canvas.getContext("2d");

      if (!context) {
        URL.revokeObjectURL(imageUrl);
        reject(new Error("No se pudo leer la imagen."));
        return;
      }

      canvas.width = image.width;
      canvas.height = image.height;
      context.drawImage(image, 0, 0);

      const { data } = context.getImageData(0, 0, canvas.width, canvas.height);
      const colorCount = {};
      const tolerance = 24;
      const sampleStep = 6;

      for (let i = 0; i < data.length; i += 4 * sampleStep) {
        const r = Math.floor(data[i] / tolerance) * tolerance;
        const g = Math.floor(data[i + 1] / tolerance) * tolerance;
        const b = Math.floor(data[i + 2] / tolerance) * tolerance;

        const colorKey = `#${r.toString(16).padStart(2, "0")}${g
          .toString(16)
          .padStart(2, "0")}${b.toString(16).padStart(2, "0")}`;

        colorCount[colorKey] = (colorCount[colorKey] || 0) + 1;
      }

      let dominantColor = "#000000";
      let maxCount = 0;

      Object.entries(colorCount).forEach(([color, count]) => {
        if (count > maxCount) {
          maxCount = count;
          dominantColor = color;
        }
      });

      URL.revokeObjectURL(imageUrl);
      resolve(dominantColor);
    };

    image.onerror = () => {
      URL.revokeObjectURL(imageUrl);
      reject(new Error("No se pudo cargar la imagen."));
    };

    image.src = imageUrl;
  });
}

async function updateColorCardFromFile(file) {
  const currentRequest = ++colorRequestId;

  try {
    const dominantColor = await getDominantColorFromImage(file);
    if (currentRequest !== colorRequestId) {
      return;
    }
    applyDominantColor(dominantColor);
  } catch (error) {
    clearDominantColor();
  }
}

function resetCards() {
  CARD_IDS.forEach((cardId) => {
    revokePreviewUrl(cardId);
    setCardImage(cardId, "");
  });

  clearDominantColor();
}

function initializeDefaultCards() {
  resetCards();
  resetPersonalInfo();
}

function validateMainFields() {
  const nameInput = document.getElementById("i-name");
  const ageInput = document.getElementById("i-age");
  if (!nameInput || !ageInput) {
    return false;
  }

  let isValid = true;
  const cleanedName = nameInput.value.trim();
  const ageValue = ageInput.value;
  const birthDate = new Date(`${ageValue}T00:00:00`);

  if (cleanedName.length < 2) {
    setFieldError("i-name", "Escribe un nombre valido.");
    isValid = false;
  } else {
    clearFieldError("i-name");
  }

  if (!ageValue || Number.isNaN(birthDate.getTime())) {
    setFieldError("i-age", "Selecciona una fecha valida.");
    isValid = false;
  } else if (birthDate > new Date()) {
    setFieldError("i-age", "La fecha no puede ser futura.");
    isValid = false;
  } else {
    clearFieldError("i-age");
  }

  return isValid;
}

function validateImageFile(inputElement) {
  const [file] = inputElement.files || [];
  if (!file) {
    clearFieldError(inputElement.id);
    return true;
  }

  if (!file.type.startsWith("image/")) {
    setFieldError(inputElement.id, "Solo se permiten imagenes.");
    return false;
  }

  clearFieldError(inputElement.id);
  return true;
}

async function handleFileInputChange(event) {
  const input = event.target;
  if (!(input instanceof HTMLInputElement)) {
    return;
  }

  if (!INPUT_TO_CARD[input.id]) {
    return;
  }

  if (!validateImageFile(input)) {
    input.value = "";
    return;
  }
}

async function applySelectedFilesToCards() {
  for (const [inputId, cardId] of Object.entries(INPUT_TO_CARD)) {
    const input = document.getElementById(inputId);
    if (!(input instanceof HTMLInputElement)) {
      continue;
    }

    if (!validateImageFile(input)) {
      input.value = "";
      continue;
    }

    const [file] = input.files || [];
    if (!file) {
      continue;
    }

    setCardImageFromFile(cardId, file);

    if (cardId === "color") {
      await updateColorCardFromFile(file);
    }
  }
}

async function valid(event) {
  event.preventDefault();

  if (!validateMainFields()) {
    return false;
  }

  const nameInput = document.getElementById("i-name");
  const ageInput = document.getElementById("i-age");
  if (!nameInput || !ageInput) {
    return false;
  }

  const normalizedName = normalizeName(nameInput.value);
  nameInput.value = normalizedName;
  updatePersonalInfo(normalizedName, ageInput.value);
  await applySelectedFilesToCards();

  const yourEssence = document.getElementById("yourEssence");
  if (yourEssence) {
    yourEssence.classList.add("snap");
  }

  window.scrollTo({ top: 0, behavior: "smooth" });

  return false;
}

window.valid = valid;

window.addEventListener("beforeunload", () => {
  previewObjectUrls.forEach((url) => URL.revokeObjectURL(url));
  previewObjectUrls.clear();
});

window.addEventListener("load", () => {
  initializeDefaultCards();

  const nameInput = document.getElementById("i-name");
  if (nameInput instanceof HTMLInputElement) {
    nameInput.addEventListener("input", () => {
      nameInput.value = nameInput.value.toUpperCase();
    });
  }

  const fileInputs = document.querySelectorAll("#Form form input[type='file']");
  fileInputs.forEach((input) => {
    if (input instanceof HTMLInputElement) {
      input.accept = "image/*";
      input.addEventListener("change", handleFileInputChange);
    }
  });

  const form = document.querySelector("#Form form");
  if (form) {
    form.addEventListener("reset", () => {
      setTimeout(() => {
        colorRequestId += 1;
        initializeDefaultCards();
        Object.keys(INPUT_TO_CARD).forEach((inputId) => clearFieldError(inputId));
        clearFieldError("i-name");
        clearFieldError("i-age");
        const yourEssence = document.getElementById("yourEssence");
        if (yourEssence) {
          yourEssence.classList.remove("snap");
        }
      }, 0);
    });
  }
});