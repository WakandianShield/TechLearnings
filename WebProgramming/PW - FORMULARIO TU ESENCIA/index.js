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
let preferredColor = "#ffffff";
let colorCardIsSolid = false;
let colorImageSource = "";
let colorImageAccent = "";

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

  if (cardId === "color") {
    colorImageSource = previewUrl;
    colorCardIsSolid = false;
  }
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

function hexToGeneralColorName(hexColor) {
  const normalizedHex = `${hexColor || ""}`.trim().toUpperCase();
  const match = /^#?([0-9A-F]{6})$/.exec(normalizedHex);
  if (!match) {
    return "UNDEFINED";
  }

  const raw = match[1];
  const r = Number.parseInt(raw.slice(0, 2), 16);
  const g = Number.parseInt(raw.slice(2, 4), 16);
  const b = Number.parseInt(raw.slice(4, 6), 16);

  const max = Math.max(r, g, b);
  const min = Math.min(r, g, b);
  const diff = max - min;
  const brightness = (max + min) / 2;

  if (max < 30) return "BLACK";
  if (min > 225 && diff < 20) return "WHITE";
  if (diff < 20) return "GRAY";

  if (r > 120 && g > 90 && b < 90) return "BROWN";

  let hue = 0;
  if (diff !== 0) {
    if (max === r) hue = ((g - b) / diff) % 6;
    else if (max === g) hue = (b - r) / diff + 2;
    else hue = (r - g) / diff + 4;
    hue *= 60;
    if (hue < 0) hue += 360;
  }

  if (hue < 15 || hue >= 345) return "RED";
  if (hue < 35) return "ORANGE";
  if (hue < 65) return "YELLOW";
  if (hue < 160) return "GREEN";
  if (hue < 255) return "BLUE";
  if (hue < 290) return "PURPLE";
  if (hue < 345) return brightness > 165 ? "PINK" : "MAGENTA";

  return "UNDEFINED";
}

function validZodiaco(dateValue) {
  const birthDate = new Date(`${dateValue}T00:00:00`);
  if (Number.isNaN(birthDate.getTime())) {
    return "";
  }

  const month = birthDate.getMonth() + 1;
  const day = birthDate.getDate();

  if ((month === 1 && day >= 20) || (month === 2 && day <= 18)) return "AQUARIUS";
  if ((month === 2 && day >= 19) || (month === 3 && day <= 20)) return "PISCES";
  if ((month === 3 && day >= 21) || (month === 4 && day <= 19)) return "ARIES";
  if ((month === 4 && day >= 20) || (month === 5 && day <= 20)) return "TAURUS";
  if ((month === 5 && day >= 21) || (month === 6 && day <= 20)) return "GEMINI";
  if ((month === 6 && day >= 21) || (month === 7 && day <= 22)) return "CANCER";
  if ((month === 7 && day >= 23) || (month === 8 && day <= 22)) return "LEO";
  if ((month === 8 && day >= 23) || (month === 9 && day <= 22)) return "VIRGO";
  if ((month === 9 && day >= 23) || (month === 10 && day <= 22)) return "LIBRA";
  if ((month === 10 && day >= 23) || (month === 11 && day <= 21)) return "SCORPIO";
  if ((month === 11 && day >= 22) || (month === 12 && day <= 21)) return "SAGITTARIUS";
  return "CAPRICORN";
}

function updatePersonalInfo(nameValue, birthDateValue, pronounValue) {
  const nameLabel = document.getElementById("name");
  const ageLabel = document.getElementById("age");
  const pronounLabel = document.getElementById("pronoun");
  const zodiacLabel = document.getElementById("zodiac");

  if (nameLabel) {
    nameLabel.textContent = normalizeName(nameValue);
  }

  const age = calculateAge(birthDateValue);
  if (ageLabel) {
    ageLabel.textContent = age === null ? "AGE: -" : `AGE: ${age} Y/O`;
  }

  if (pronounLabel) {
    pronounLabel.textContent = pronounValue ? `PRONOUN: ${pronounValue}` : "PRONOUN: -";
  }

  if (zodiacLabel) {
    const zodiac = validZodiaco(birthDateValue);
    zodiacLabel.textContent = zodiac ? `ZODIAC: ${zodiac}` : "ZODIAC: -";
  }
}

function resetPersonalInfo() {
  const nameLabel = document.getElementById("name");
  const ageLabel = document.getElementById("age");
  const pronounLabel = document.getElementById("pronoun");
  const zodiacLabel = document.getElementById("zodiac");

  if (nameLabel) nameLabel.textContent = "NAME";
  if (ageLabel) ageLabel.textContent = "AGE: -";
  if (pronounLabel) pronounLabel.textContent = "PRONOUN: -";
  if (zodiacLabel) zodiacLabel.textContent = "ZODIAC: -";
}

function createSolidColorDataUrl(hexColor) {
  const safeColor = `${hexColor || "#000000"}`;
  const svg = `<svg xmlns='http://www.w3.org/2000/svg' width='400' height='400'><rect width='100%' height='100%' fill='${safeColor}'/></svg>`;
  return `data:image/svg+xml;charset=UTF-8,${encodeURIComponent(svg)}`;
}

function toggleColorCardView() {
  if (colorCardIsSolid && colorImageSource) {
    setCardImage("color", colorImageSource);
    if (colorImageAccent) {
      applyDominantColor(colorImageAccent);
    }
    colorCardIsSolid = false;
    return;
  }

  setCardImage("color", createSolidColorDataUrl(preferredColor));
  applyDominantColor(preferredColor);
  colorCardIsSolid = true;
}

function applyDominantColor(hexColor) {
  const colorCard = document.getElementById("color");
  if (!colorCard) {
    return;
  }

  colorCard.style.boxShadow = `inset 0 0 0 3px ${hexColor}`;
  const label = colorCard.querySelector("span");
  if (label) {
    label.textContent = `COLOR ${hexColor.toUpperCase()}`;
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
    label.textContent = "COLOR";
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
        reject(new Error("The image could not be read."));
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
      reject(new Error("The image could not be loaded."));
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
    colorImageAccent = dominantColor;
    applyDominantColor(dominantColor);
  } catch (error) {
    colorImageAccent = "";
    clearDominantColor();
  }
}

function resetCards() {
  CARD_IDS.forEach((cardId) => {
    revokePreviewUrl(cardId);
    setCardImage(cardId, "");
  });

  colorImageSource = "";
  colorImageAccent = "";
  colorCardIsSolid = false;
  clearDominantColor();
}

function initializeDefaultCards() {
  resetCards();
  resetPersonalInfo();
  resetInfoOutputs();
  preferredColor = "#ffffff";
}

function updateInfoOutputs(formData) {
  const favoriteColorInfo = document.getElementById("favorite-color-info");
  const heightInfo = document.getElementById("height-info");

  const selectedColorHex = `${formData.get("favoriteColor") || preferredColor}`;
  const selectedColorName = hexToGeneralColorName(selectedColorHex);
  const selectedHeight = `${formData.get("height") || "170"}`;

  if (favoriteColorInfo) {
    favoriteColorInfo.textContent = `COLOR: ${selectedColorName}`;
  }

  if (heightInfo) {
    heightInfo.textContent = `HEIGHT: ${selectedHeight} CM`;
  }
}

function resetInfoOutputs() {
  const favoriteColorInfo = document.getElementById("favorite-color-info");
  const heightInfo = document.getElementById("height-info");

  if (favoriteColorInfo) favoriteColorInfo.textContent = "COLOR: -";
  if (heightInfo) heightInfo.textContent = "HEIGHT: -";
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
    nameInput.setCustomValidity("Enter a valid name.");
    setFieldError("i-name", "Enter a valid name.");
    isValid = false;
  } else {
    nameInput.setCustomValidity("");
    clearFieldError("i-name");
  }

  if (!ageValue || Number.isNaN(birthDate.getTime())) {
    ageInput.setCustomValidity("Select a valid date.");
    setFieldError("i-age", "Select a valid date.");
    isValid = false;
  } else if (birthDate > new Date()) {
    ageInput.setCustomValidity("Date cannot be in the future.");
    setFieldError("i-age", "Date cannot be in the future.");
    isValid = false;
  } else {
    ageInput.setCustomValidity("");
    clearFieldError("i-age");
  }

  return isValid;
}

function validateExtraFields() {
  const pronounInput = document.getElementById("i-pronoun");
  const favoriteColorInput = document.getElementById("i-favorite-color");
  const heightInput = document.getElementById("i-height");

  let isValid = true;

  if (pronounInput instanceof HTMLSelectElement) {
    if (!pronounInput.value) {
      pronounInput.setCustomValidity("Select a pronoun.");
      setFieldError("i-pronoun", pronounInput.validationMessage);
      isValid = false;
    } else {
      pronounInput.setCustomValidity("");
      clearFieldError("i-pronoun");
    }
  }

  if (favoriteColorInput instanceof HTMLInputElement) {
    const isHexColor = /^#[0-9a-fA-F]{6}$/.test(favoriteColorInput.value);
    if (!isHexColor) {
      favoriteColorInput.setCustomValidity("Select a valid color.");
      setFieldError("i-favorite-color", favoriteColorInput.validationMessage);
      isValid = false;
    } else {
      favoriteColorInput.setCustomValidity("");
      clearFieldError("i-favorite-color");
    }
  }

  if (heightInput instanceof HTMLInputElement) {
    const heightValue = Number(heightInput.value);
    if (Number.isNaN(heightValue) || heightValue < 120 || heightValue > 220) {
      heightInput.setCustomValidity("Height must be between 120 and 220 cm.");
      setFieldError("i-height", heightInput.validationMessage);
      isValid = false;
    } else {
      heightInput.setCustomValidity("");
      clearFieldError("i-height");
    }
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
    setFieldError(inputElement.id, "Only image files are allowed.");
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

function toBase64(file) {
  return new Promise((resolve, reject) => {
    if (!(file instanceof File)) {
      reject(new Error("Invalid file."));
      return;
    }

    const reader = new FileReader();
    reader.onload = () => resolve(`${reader.result || ""}`);
    reader.onerror = () => reject(new Error("File conversion failed."));
    reader.readAsDataURL(file);
  });
}

async function download() {
  const yourEssence = document.getElementById("yourEssence");
  if (!yourEssence || typeof window.html2canvas !== "function") {
    return;
  }

  yourEssence.classList.add("is-exporting");
  try {
    const sourceCanvas = await window.html2canvas(yourEssence, {
      backgroundColor: null,
      scale: 2,
      useCORS: true,
    });

    const exportWidth = 1080;
    const exportHeight = 1920;
    const exportCanvas = document.createElement("canvas");
    exportCanvas.width = exportWidth;
    exportCanvas.height = exportHeight;

    const context = exportCanvas.getContext("2d");
    if (!context) {
      return;
    }

    const rootStyles = window.getComputedStyle(document.documentElement);
    const bg0 = rootStyles.getPropertyValue("--bg-0").trim() || "#000000";
    const bg1 = rootStyles.getPropertyValue("--bg-1").trim() || "#1f1f1f";
    const gradient = context.createLinearGradient(0, 0, exportWidth, 0);
    gradient.addColorStop(0, bg0);
    gradient.addColorStop(1, bg1);
    context.fillStyle = gradient;
    context.fillRect(0, 0, exportWidth, exportHeight);

    const scale = Math.max(
      exportWidth / sourceCanvas.width,
      exportHeight / sourceCanvas.height,
    ) * 0.55;

    const drawWidth = sourceCanvas.width * scale;
    const drawHeight = sourceCanvas.height * scale;
    const drawX = (exportWidth - drawWidth) / 2;
    const drawY = (exportHeight - drawHeight) / 2;

    context.drawImage(sourceCanvas, drawX, drawY, drawWidth, drawHeight);

    const blob = await new Promise((resolve) => {
      exportCanvas.toBlob((result) => resolve(result), "image/png");
    });

    let url = exportCanvas.toDataURL("image/png");
    if (blob instanceof Blob) {
      const imageFile = new File([blob], "your-essence.png", { type: "image/png" });
      url = await toBase64(imageFile);
    }

    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = `your-essence-${Date.now()}.png`;
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
  } finally {
    yourEssence.classList.remove("is-exporting");
  }
}

async function valid(event) {
  event.preventDefault();

  const form = event.currentTarget;
  if (!(form instanceof HTMLFormElement)) {
    return false;
  }

  if (!validateMainFields() || !validateExtraFields()) {
    return false;
  }

  const nameInput = document.getElementById("i-name");
  const ageInput = document.getElementById("i-age");
  const pronounInput = document.getElementById("i-pronoun");
  if (!nameInput || !ageInput || !pronounInput) {
    return false;
  }

  const formData = new FormData(form);
  preferredColor = `${formData.get("favoriteColor") || preferredColor}`;
  const normalizedName = normalizeName(nameInput.value);
  nameInput.value = normalizedName;
  updatePersonalInfo(normalizedName, ageInput.value, `${formData.get("pronoun") || ""}`);
  updateInfoOutputs(formData);
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
    const pronounInput = document.getElementById("i-pronoun");
    if (pronounInput instanceof HTMLSelectElement) {
      pronounInput.addEventListener("input", validateExtraFields);
    }

    const favoriteColorInput = document.getElementById("i-favorite-color");
    const favoriteColorValue = document.getElementById("i-favorite-color-value");
    if (favoriteColorInput instanceof HTMLInputElement) {
      favoriteColorInput.addEventListener("input", () => {
        preferredColor = favoriteColorInput.value;
        if (favoriteColorValue) {
          favoriteColorValue.textContent = favoriteColorInput.value.toUpperCase();
        }
        validateExtraFields();
      });
    }

    const heightInput = document.getElementById("i-height");
    const heightValue = document.getElementById("i-height-value");
    if (heightInput instanceof HTMLInputElement) {
      heightInput.addEventListener("input", () => {
        if (heightValue) {
          heightValue.textContent = `${heightInput.value} cm`;
        }
        validateExtraFields();
      });
    }

    const colorCard = document.getElementById("color");
    if (colorCard) {
      colorCard.addEventListener("click", toggleColorCardView);
    }

    const downloadButton = document.getElementById("download-grid");
    if (downloadButton) {
      downloadButton.addEventListener("click", () => {
        download();
      });
    }

    form.addEventListener("reset", () => {
      setTimeout(() => {
        colorRequestId += 1;
        initializeDefaultCards();
        Object.keys(INPUT_TO_CARD).forEach((inputId) => clearFieldError(inputId));
        clearFieldError("i-name");
        clearFieldError("i-age");
        clearFieldError("i-pronoun");
        clearFieldError("i-favorite-color");
        clearFieldError("i-height");
        if (heightValue) {
          heightValue.textContent = "170 cm";
        }
        if (favoriteColorValue) {
          favoriteColorValue.textContent = "#FFFFFF";
        }
        resetInfoOutputs();
        const yourEssence = document.getElementById("yourEssence");
        if (yourEssence) {
          yourEssence.classList.remove("snap");
        }
      }, 0);
    });
  }
});