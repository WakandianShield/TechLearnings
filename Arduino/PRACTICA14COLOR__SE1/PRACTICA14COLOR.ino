#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <Adafruit_TCS34725.h>

// ─── LEDs de color (pines 30-33) ────────────────────────
#define LED_ROJO      30
#define LED_VERDE     31
#define LED_AZUL      32
#define LED_AMARILLO  33

// ─── Objeto TCS34725 ─────────────────────────────────────
Adafruit_TCS34725 tcs = Adafruit_TCS34725(
  TCS34725_INTEGRATIONTIME_50MS,
  TCS34725_GAIN_4X
);

// ─── Objeto LCD ──────────────────────────────────────────
LiquidCrystal_I2C lcd(0x27, 16, 2);

// ─── Variables sensor de color ───────────────────────────
String colorActual   = "---";
String colorAnterior = "";

// ─── Timer (millis, sin delay) ───────────────────────────
unsigned long anteriorColor = 0;
const long intervaloColor   = 500;

// ─── Prototipos ──────────────────────────────────────────
String identificarColor(int r, int g, int b);
void   apagarLEDs();
void   encenderLED(const String& color);
void   mostrarPantalla();

// ─────────────────────────────────────────────────────────
void setup() {
  Serial.begin(9600);

  // LEDs
  pinMode(LED_ROJO,     OUTPUT);
  pinMode(LED_VERDE,    OUTPUT);
  pinMode(LED_AZUL,     OUTPUT);
  pinMode(LED_AMARILLO, OUTPUT);

  // LCD
  lcd.init();
  lcd.backlight();

  // TCS34725
  if (!tcs.begin()) {
    lcd.setCursor(0, 0);
    lcd.print("TCS34725 ERROR");
    lcd.setCursor(0, 1);
    lcd.print("Revisa conexion");
    Serial.println("ERROR: TCS34725 no encontrado");
    while (1);  // Detiene ejecución
  }

  lcd.setCursor(0, 0);
  lcd.print("Detector");
  lcd.setCursor(0, 1);
  lcd.print("de Color v2.0");
  delay(2000);
  lcd.clear();
}

// ─────────────────────────────────────────────────────────
void loop() {
  unsigned long ahora = millis();

  if (ahora - anteriorColor >= intervaloColor) {
    anteriorColor = ahora;

    // Leer RGBC del TCS34725
    uint16_t r16, g16, b16, c16;
    tcs.getRawData(&r16, &g16, &b16, &c16);

    // Normalizar a 0-255 usando el canal claro (C) como referencia
    int r = 0, g = 0, b = 0;
    if (c16 > 0) {
      r = constrain((int)((float)r16 / c16 * 255), 0, 255);
      g = constrain((int)((float)g16 / c16 * 255), 0, 255);
      b = constrain((int)((float)b16 / c16 * 255), 0, 255);
    }

    colorActual = identificarColor(r, g, b);

    // Solo actuar si el color cambió (evita parpadeos)
    if (colorActual != colorAnterior) {
      colorAnterior = colorActual;
      apagarLEDs();
      encenderLED(colorActual);
      mostrarPantalla();

      Serial.print("R:"); Serial.print(r);
      Serial.print(" G:"); Serial.print(g);
      Serial.print(" B:"); Serial.print(b);
      Serial.print(" C:"); Serial.print(c16);
      Serial.print(" → "); Serial.println(colorActual);
    }
  }
}

// ─────────────────────────────────────────────────────────
void mostrarPantalla() {
  lcd.setCursor(0, 0);
  lcd.print("Color detectado:");

  lcd.setCursor(0, 1);
  lcd.print(colorActual);
  lcd.print("          ");  // Borra residuos de textos anteriores
}

// ─────────────────────────────────────────────────────────
String identificarColor(int r, int g, int b) {
  const int umbral = 80;
  const int margen = 60;

  bool rDom    = (r > umbral) && (r - g > margen) && (r - b > margen);
  bool gDom    = (g > umbral) && (g - r > margen) && (g - b > margen);
  bool bDom    = (b > umbral) && (b - r > margen) && (b - g > margen);
  bool amarillo = (r > umbral) && (g > umbral) && (b < umbral)
                  && (abs(r - g) < margen);

  if (amarillo) return "AMARILLO";
  if (rDom)     return "ROJO";
  if (gDom)     return "VERDE";
  if (bDom)     return "AZUL";
  return "---";
}

// ─────────────────────────────────────────────────────────
void apagarLEDs() {
  digitalWrite(LED_ROJO,     LOW);
  digitalWrite(LED_VERDE,    LOW);
  digitalWrite(LED_AZUL,     LOW);
  digitalWrite(LED_AMARILLO, LOW);
}

void encenderLED(const String& color) {
  if      (color == "ROJO")     digitalWrite(LED_ROJO,     HIGH);
  else if (color == "VERDE")    digitalWrite(LED_VERDE,    HIGH);
  else if (color == "AZUL")     digitalWrite(LED_AZUL,     HIGH);
  else if (color == "AMARILLO") digitalWrite(LED_AMARILLO, HIGH);
}