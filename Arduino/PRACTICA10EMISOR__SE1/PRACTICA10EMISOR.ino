// ============================================================
// ARDUINO MEGA 1 - MAESTRO
// Sistema de Control de Iluminación Distribuido vía SPI
// Práctica Integradora: TIMER, INT, ADC, PWM, SERIAL, SPI
// ============================================================
#include <SPI.h>

// --- Pines ---
#define PIN_POT     A0   // Potenciómetro (ADC)
#define PIN_DIP1    22   // DIP switch bit 0
#define PIN_DIP2    23   // DIP switch bit 1
#define PIN_DIP3    24   // DIP switch bit 2
#define PIN_DIP4    25   // DIP switch bit 3
#define PIN_BTN      2   // Botón (INT0 - interrupción externa)
#define PIN_SS      53   // Slave Select SPI

// --- Variables globales ---
volatile bool modoEmergencia = false;  // Bandera de interrupción
volatile bool enviarDatos    = false;  // Bandera del Timer
uint8_t  brillo  = 0;                 // Valor PWM (0-255)
uint8_t  zonas   = 0;                 // Bits activos del DIP

// ============================================================
// INTERRUPCIÓN EXTERNA INT0 - Botón en pin 2
// Se activa con flanco de bajada (botón presionado)
// ============================================================
void ISR_boton() {
  modoEmergencia = !modoEmergencia;  // Alterna modo emergencia
}

// ============================================================
// INTERRUPCIÓN TIMER1 - Cada 1 segundo
// Configura Timer1 en modo CTC con prescaler 256
// ============================================================
ISR(TIMER1_COMPA_vect) {
  enviarDatos = true;  // Señaliza al loop que debe enviar
}

void configurarTimer1() {
  TCCR1A = 0;
  TCCR1B = 0;
  TCNT1  = 0;
  OCR1A  = 62499;           // 16MHz / 256 / 1Hz - 1
  TCCR1B |= (1 << WGM12);  // Modo CTC
  TCCR1B |= (1 << CS12);   // Prescaler 256
  TIMSK1 |= (1 << OCIE1A); // Habilita interrupción
}

// ============================================================
// SETUP
// ============================================================
void setup() {
  Serial.begin(9600);
  Serial.println("=== MAESTRO INICIADO ===");

  // Configurar pines DIP switch como entrada con pull-up
  pinMode(PIN_DIP1, INPUT_PULLUP);
  pinMode(PIN_DIP2, INPUT_PULLUP);
  pinMode(PIN_DIP3, INPUT_PULLUP);
  pinMode(PIN_DIP4, INPUT_PULLUP);

  // Configurar botón con interrupción externa
  pinMode(PIN_BTN, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(PIN_BTN), ISR_boton, FALLING);

  // Inicializar SPI como Maestro
  pinMode(PIN_SS, OUTPUT);
  digitalWrite(PIN_SS, HIGH);
  SPI.begin();
  SPI.setClockDivider(SPI_CLOCK_DIV16);

  // Iniciar Timer1
  configurarTimer1();
  sei(); // Habilitar interrupciones globales
}

// ============================================================
// LOOP PRINCIPAL
// ============================================================
void loop() {
  if (enviarDatos) {
    enviarDatos = false;

    // 1. Leer potenciómetro (ADC 10 bits -> 8 bits para PWM)
    uint16_t lectura = analogRead(PIN_POT);
    brillo = map(lectura, 0, 1023, 0, 255);

    // 2. Leer DIP switch (4 bits = 4 zonas)
    zonas  = 0;
    zonas |= (!digitalRead(PIN_DIP1)) << 0;
    zonas |= (!digitalRead(PIN_DIP2)) << 1;
    zonas |= (!digitalRead(PIN_DIP3)) << 2;
    zonas |= (!digitalRead(PIN_DIP4)) << 3;

    // 3. Si hay emergencia, apagar todo
    if (modoEmergencia) {
      brillo = 0;
      zonas  = 0;
    }

    // 4. Enviar datos al esclavo por SPI (2 bytes: zonas + brillo)
    digitalWrite(PIN_SS, LOW);
    SPI.transfer(zonas);
    SPI.transfer(brillo);
    digitalWrite(PIN_SS, HIGH);

    // 5. Reporte al Monitor Serial
    Serial.print("[MAESTRO] Zonas: 0b");
    Serial.print(zonas, BIN);
    Serial.print(" | Brillo: ");
    Serial.print(map(brillo, 0, 255, 0, 100));
    Serial.print("%");
    if (modoEmergencia) Serial.print(" | *** EMERGENCIA ***");
    Serial.println();
  }
}


