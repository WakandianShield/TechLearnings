// ============================================================
// ARDUINO MEGA 2 - ESCLAVO
// Sistema de Control de Iluminación Distribuido vía SPI
// ============================================================
#include <SPI.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>

// --- LCD I2C: dirección 0x27, 16 columnas, 2 filas ---
LiquidCrystal_I2C lcd(0x27, 16, 2);

// --- Pines LEDs con PWM ---
#define LED1  2   // Zona 1
#define LED2  3   // Zona 2
#define LED3  4   // Zona 3
#define LED4  5   // Zona 4

// --- Variables SPI ---
volatile uint8_t zonas  = 0;
volatile uint8_t brillo = 0;
volatile bool    nuevoDato = false;
volatile uint8_t byteContador = 0;  // Contador de bytes recibidos

// ============================================================
// INTERRUPCIÓN SPI - Se ejecuta al recibir cada byte
// El Maestro envía 2 bytes: primero zonas, luego brillo
// ============================================================
ISR(SPI_STC_vect) {
  uint8_t dato = SPDR;
  if (byteContador == 0) {
    zonas = dato;
    byteContador = 1;
  } else {
    brillo = dato;
    byteContador = 0;
    nuevoDato = true;  // Trama completa recibida
  }
}

// ============================================================
// SETUP
// ============================================================
void setup() {
  Serial.begin(9600);
  Serial.println("=== ESCLAVO INICIADO ===");

  // Configurar LEDs
  pinMode(LED1, OUTPUT);
  pinMode(LED2, OUTPUT);
  pinMode(LED3, OUTPUT);
  pinMode(LED4, OUTPUT);

  // Inicializar LCD I2C
  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  lcd.print("Control Luces");
  lcd.setCursor(0, 1);
  lcd.print("Esperando...");

  // Configurar SPI como Esclavo
  pinMode(MISO, OUTPUT);  // Esclavo envía por MISO
  SPCR |= _BV(SPE);       // Habilitar SPI
  SPCR |= _BV(SPIE);      // Habilitar interrupción SPI
  sei();
}

// ============================================================
// Actualiza los 4 LEDs según zonas y brillo
// ============================================================
void actualizarLEDs(uint8_t z, uint8_t b) {
  analogWrite(LED1, (z & 0x01) ? b : 0);
  analogWrite(LED2, (z & 0x02) ? b : 0);
  analogWrite(LED3, (z & 0x04) ? b : 0);
  analogWrite(LED4, (z & 0x08) ? b : 0);
}

// ============================================================
// Actualiza la pantalla LCD
// ============================================================
void actualizarLCD(uint8_t z, uint8_t b) {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Zonas:");
  lcd.print((z & 0x01) ? "1" : "-");
  lcd.print((z & 0x02) ? "2" : "-");
  lcd.print((z & 0x04) ? "3" : "-");
  lcd.print((z & 0x08) ? "4" : "-");
  lcd.setCursor(0, 1);
  lcd.print("Brillo:");
  lcd.print(map(b, 0, 255, 0, 100));
  lcd.print("%  ");
  if (b == 0 && z == 0) {
    lcd.setCursor(9, 1);
    lcd.print("EMERG");
  }
}

// ============================================================
// LOOP PRINCIPAL
// ============================================================
void loop() {
  if (nuevoDato) {
    nuevoDato = false;

    uint8_t z = zonas;
    uint8_t b = brillo;

    // Actualizar LEDs con PWM
    actualizarLEDs(z, b);

    // Actualizar pantalla LCD I2C
    actualizarLCD(z, b);

    // Reporte al Monitor Serial
    Serial.print("[ESCLAVO] Zonas: 0b");
    Serial.print(z, BIN);
    Serial.print(" | Brillo: ");
    Serial.print(map(b, 0, 255, 0, 100));
    Serial.println("%");
  }
}
