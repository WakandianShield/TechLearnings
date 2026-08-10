#include <Wire.h>
#include <LiquidCrystal_I2C.h>

// ─── Sensor ultrasónico HC-SR04 ──────────────────────────
#define TRIG_PIN  10
#define ECHO_PIN  11

// ─── LEDs (mismos pines) ─────────────────────────────────
#define LED_ROJO      30   // Muy cerca  (< 10 cm)
#define LED_AMARILLO  33   // Cerca      (10 - 25 cm)
#define LED_VERDE     31   // Lejos      (25 - 50 cm)
#define LED_AZUL      32   // Muy lejos  (> 50 cm)

// ─── LCD I2C ─────────────────────────────────────────────
LiquidCrystal_I2C lcd(0x27, 16, 2);

// ─── Variables ───────────────────────────────────────────
float    distancia     = 0;
String   nivelActual   = "";
String   nivelAnterior = "";

// ─── Timers ──────────────────────────────────────────────
unsigned long anteriorSensor = 0;
unsigned long anteriorLCD    = 0;
const long    intervaloSensor = 200;
const long    intervaloLCD    = 300;

// ─── Prototipos ──────────────────────────────────────────
float  leerDistancia();
String obtenerNivel(float cm);
void   apagarLEDs();
void   encenderLED(const String& nivel);
void   mostrarPantalla();

// ─────────────────────────────────────────────────────────
void setup() {
  Serial.begin(9600);

  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);

  pinMode(LED_ROJO,     OUTPUT);
  pinMode(LED_AMARILLO, OUTPUT);
  pinMode(LED_VERDE,    OUTPUT);
  pinMode(LED_AZUL,     OUTPUT);
  apagarLEDs();

  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  lcd.print("Sensor");
  lcd.setCursor(0, 1);
  lcd.print("Distancia v1.0");
  delay(2000);
  lcd.clear();
}

// ─────────────────────────────────────────────────────────
void loop() {
  unsigned long ahora = millis();

  // ── Leer sensor cada 200 ms ───────────────────────────
  if (ahora - anteriorSensor >= intervaloSensor) {
    anteriorSensor = ahora;

    float nuevaDist = leerDistancia();

    // Filtrar lecturas inválidas del HC-SR04
    if (nuevaDist > 0 && nuevaDist <= 400) {
      distancia = nuevaDist;
    }

    nivelActual = obtenerNivel(distancia);

    // Solo cambiar LED si el nivel cambió
    if (nivelActual != nivelAnterior) {
      nivelAnterior = nivelActual;
      apagarLEDs();
      encenderLED(nivelActual);
    }
  }

  // ── Actualizar LCD cada 300 ms ────────────────────────
  if (ahora - anteriorLCD >= intervaloLCD) {
    anteriorLCD = ahora;
    mostrarPantalla();
  }
}

// ─────────────────────────────────────────────────────────
float leerDistancia() {
  digitalWrite(TRIG_PIN, LOW);
  delayMicroseconds(2);
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);

  long duracion = pulseIn(ECHO_PIN, HIGH, 30000UL); // Timeout 30 ms
  return duracion * 0.01716;  // cm = us * (0.0343 / 2)
}

// ─────────────────────────────────────────────────────────
//  Rangos de distancia:
//  Muy cerca  → < 10 cm      → LED ROJO
//  Cerca      → 10 - 25 cm   → LED AMARILLO
//  Lejos      → 25 - 50 cm   → LED VERDE
//  Muy lejos  → > 50 cm      → LED AZUL
// ─────────────────────────────────────────────────────────
String obtenerNivel(float cm) {
  if (cm < 10)              return "MUY CERCA";
  else if (cm < 25)         return "CERCA";
  else if (cm <= 50)        return "LEJOS";
  else                      return "MUY LEJOS";
}

// ─────────────────────────────────────────────────────────
void mostrarPantalla() {
  // Línea 0: distancia en cm
  lcd.setCursor(0, 0);
  lcd.print("Dist: ");
  lcd.print(distancia, 1);
  lcd.print(" cm   ");   // Espacios borran dígitos anteriores

  // Línea 1: nivel
  lcd.setCursor(0, 1);
  lcd.print(nivelActual);
  lcd.print("          ");
}

// ─────────────────────────────────────────────────────────
void apagarLEDs() {
  digitalWrite(LED_ROJO,     LOW);
  digitalWrite(LED_AMARILLO, LOW);
  digitalWrite(LED_VERDE,    LOW);
  digitalWrite(LED_AZUL,     LOW);
}

void encenderLED(const String& nivel) {
  if      (nivel == "MUY CERCA") digitalWrite(LED_ROJO,     HIGH);
  else if (nivel == "CERCA")     digitalWrite(LED_AMARILLO, HIGH);
  else if (nivel == "LEJOS")     digitalWrite(LED_VERDE,    HIGH);
  else if (nivel == "MUY LEJOS") digitalWrite(LED_AZUL,     HIGH);
}