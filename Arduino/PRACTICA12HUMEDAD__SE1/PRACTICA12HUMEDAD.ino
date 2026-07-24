#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <DHT.h>

#define DHT_PIN      2
#define DHT_TYPE     DHT11
#define RELAY_PIN    7
#define TEMP_UMBRAL  30.0

DHT dht(DHT_PIN, DHT_TYPE);
LiquidCrystal_I2C lcd(0x27, 16, 2);

float temperatura = 0;
float humedad     = 0;

unsigned long anteriorSensor = 0;
unsigned long anteriorLCD    = 0;

const long intervaloSensor = 1000;  // Sensor cada 1 segundo (mínimo del DHT11)
const long intervaloLCD    = 200;   // LCD cada 200ms — se ve fluido

void setup() {
  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, LOW);

  dht.begin();

  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  lcd.print(" Sistema de    ");
  lcd.setCursor(0, 1);
  lcd.print(" Temperatura   ");
  delay(2000);
  lcd.clear();
}

void loop() {
  unsigned long ahora = millis();

  // ── Leer sensor cada 1 segundo ─────────────────
  if (ahora - anteriorSensor >= intervaloSensor) {
    anteriorSensor = ahora;

    float tempNueva = dht.readTemperature();
    float humNueva  = dht.readHumidity();

    // Solo actualiza si la lectura es válida
    if (!isnan(tempNueva) && !isnan(humNueva)) {
      temperatura = tempNueva;
      humedad     = humNueva;
    }

    // Control del relay basado en la última lectura válida
    if (temperatura >= TEMP_UMBRAL) {
      digitalWrite(RELAY_PIN, HIGH);   // Enciende
    } else {
      digitalWrite(RELAY_PIN, LOW);  // Apaga
    }
  }

  // ── Actualizar LCD cada 200ms ───────────────────
  if (ahora - anteriorLCD >= intervaloLCD) {
    anteriorLCD = ahora;

    lcd.setCursor(0, 0);
    lcd.print("Temp: ");
    lcd.print(temperatura, 1);
    lcd.print((char)223);
    lcd.print("C  ");

    lcd.setCursor(0, 1);
    lcd.print("Hum:  ");
    lcd.print(humedad, 1);
    lcd.print("%  ");

    // Indicador de relay en esquina superior derecha
    lcd.setCursor(13, 0);
    if (temperatura >= TEMP_UMBRAL) {
      lcd.print("ON ");
    } else {
      lcd.print("OFF");
    }
  }
}