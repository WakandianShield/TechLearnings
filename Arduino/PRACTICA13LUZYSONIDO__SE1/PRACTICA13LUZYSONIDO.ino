#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <DHT.h>

// ── Pines ──────────────────────────────────────────
#define PIN_LUZ_DO        22
#define PIN_SONIDO_AO     A1
#define PIN_RELAY1        7
#define PIN_RELAY2        8
#define PIN_LED_VERDE     3
#define PIN_LED_AMARILLO  4
#define PIN_LED_ROJO      5

// DHT declarado pero no usado en P2 — solo para que compile
#define DHT_PIN           2
#define DHT_TYPE          DHT11

// ── Umbrales sonido ────────────────────────────────
#define UMBRAL_SONIDO_MEDIO  300
#define UMBRAL_SONIDO_ALTO   600

// ── Objetos ────────────────────────────────────────
LiquidCrystal_I2C lcd(0x27, 16, 2);
DHT dht(DHT_PIN, DHT_TYPE);   // Declarado para no dejar el pin flotando

// ── Variables ──────────────────────────────────────
bool foco1On     = false;
bool foco2On     = false;
bool foco2Activo = false;
unsigned long tiempoFoco2    = 0;
unsigned long anteriorSensor = 0;
unsigned long anteriorLCD    = 0;

const long intervaloSensor = 100;
const long intervaloLCD    = 300;
const long duracionFoco2   = 10000;

enum NivelSonido { BAJO, MEDIO, ALTO };
NivelSonido nivelActual = BAJO;

// ──────────────────────────────────────────────────
void setup() {
  pinMode(PIN_LUZ_DO,       INPUT);
  pinMode(PIN_RELAY1,       OUTPUT);
  pinMode(PIN_RELAY2,       OUTPUT);
  pinMode(PIN_LED_VERDE,    OUTPUT);
  pinMode(PIN_LED_AMARILLO, OUTPUT);
  pinMode(PIN_LED_ROJO,     OUTPUT);

  // Todo apagado al inicio — relays activo-LOW
  digitalWrite(PIN_RELAY1,       HIGH);
  digitalWrite(PIN_RELAY2,       HIGH);
  digitalWrite(PIN_LED_VERDE,    LOW);
  digitalWrite(PIN_LED_AMARILLO, LOW);
  digitalWrite(PIN_LED_ROJO,     LOW);

  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  lcd.print(" Practica  2    ");
  lcd.setCursor(0, 1);
  lcd.print(" Luz / Sonido   ");
  delay(2000);
  lcd.clear();
}

// ──────────────────────────────────────────────────
void loop() {
  unsigned long ahora = millis();

  // ── Leer sensores ────────────────────────────────
  if (ahora - anteriorSensor >= intervaloSensor) {
    anteriorSensor = ahora;

    // ── Control Luz (Relay 1) ────────────────────
    bool oscuro = (digitalRead(PIN_LUZ_DO) == LOW);
    if (oscuro) {
      digitalWrite(PIN_RELAY1, LOW);
      foco1On = true;
    } else {
      digitalWrite(PIN_RELAY1, HIGH);
      foco1On = false;
    }

    // ── Control Sonido ───────────────────────────
    int valorSonido = analogRead(PIN_SONIDO_AO);

    if (valorSonido < UMBRAL_SONIDO_MEDIO) {
      nivelActual = BAJO;
      digitalWrite(PIN_LED_VERDE,    HIGH);
      digitalWrite(PIN_LED_AMARILLO, LOW);
      digitalWrite(PIN_LED_ROJO,     LOW);

    } else if (valorSonido < UMBRAL_SONIDO_ALTO) {
      nivelActual = MEDIO;
      digitalWrite(PIN_LED_VERDE,    LOW);
      digitalWrite(PIN_LED_AMARILLO, HIGH);
      digitalWrite(PIN_LED_ROJO,     LOW);

    } else {
      nivelActual = ALTO;
      digitalWrite(PIN_LED_VERDE,    LOW);
      digitalWrite(PIN_LED_AMARILLO, LOW);
      digitalWrite(PIN_LED_ROJO,     HIGH);

      if (!foco2Activo) {
        digitalWrite(PIN_RELAY2, LOW);
        foco2On     = true;
        foco2Activo = true;
        tiempoFoco2 = ahora;
      }
    }

    // ── Timer foco 2 — apagar tras 10s ──────────
    if (foco2Activo && (ahora - tiempoFoco2 >= duracionFoco2)) {
      digitalWrite(PIN_RELAY2, HIGH);
      foco2On     = false;
      foco2Activo = false;
    }
  }

  // ── Actualizar LCD ───────────────────────────────
  if (ahora - anteriorLCD >= intervaloLCD) {
    anteriorLCD = ahora;

    if (foco2On) {
      lcd.setCursor(0, 0);
      lcd.print("QUE FINALICE    ");
      lcd.setCursor(0, 1);
      lcd.print("  LA FIESTA!    ");
    } else {
      lcd.setCursor(0, 0);
      lcd.print(foco1On ? "Act1: ENCENDIDO " : "Act1: APAGADO   ");
      lcd.setCursor(0, 1);
      lcd.print("Act2: APAGADO   ");
    }
  }
}