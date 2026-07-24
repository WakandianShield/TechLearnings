#include <LiquidCrystal.h>

#include <Wire.h>
#include <LiquidCrystal_I2C.h>

// LCD en dirección 0x27, 16 columnas, 2 filas
LiquidCrystal_I2C lcd(0x27, 16, 2);

// ── Flags de interrupciones (volatile para compartir con ISR) ──────────
volatile bool flagInt1 = false;
volatile bool flagInt2 = false;
volatile bool flagInt3 = false;

// ── Variables del contador principal ───────────────────────────────────
int contador = 0;
unsigned long ultimoTiempo = 0;
const unsigned long INTERVALO = 500; // ms entre números

// ── ISR Interrupción 1 — Botón en Pin 2 (INT0) ─────────────────────────
void ISR_Int1() {
  flagInt1 = true;
}

// ── ISR Interrupción 2 — Timer1 (cada 30 segundos) ─────────────────────
ISR(TIMER1_COMPA_vect) {
  flagInt2 = true;
}

// ── ISR Interrupción 3 — Botón en Pin 3 (INT1) ─────────────────────────
void ISR_Int3() {
  flagInt3 = true;
}

// ── Configuración del Timer1 para 30 segundos ──────────────────────────
void configurarTimer1() {
  noInterrupts();
  TCCR1A = 0;
  TCCR1B = 0;
  TCNT1  = 0;

  // Con prescaler 1024 y 16 MHz:
  // OCR1A = (16,000,000 / 1024 / 0.5) - 1 = 31249  →  2 segundos por ciclo
  // Para 30 s: se cuentan 15 disparos del timer (30/2 = 15)
  // Alternativamente, usamos directamente 30 s:
  // OCR1A = (16,000,000 / 1024) * 30 - 1 = 468749
  // Timer1 es de 16 bits (máx 65535), por lo que se necesita un contador por software
  // Usamos 2 s por interrupción y contamos 15 veces en loop() para llegar a 30 s.

  OCR1A = 31249;           // 2 segundos por disparo (prescaler 1024)
  TCCR1B |= (1 << WGM12); // Modo CTC
  TCCR1B |= (1 << CS12) | (1 << CS10); // Prescaler 1024
  TIMSK1 |= (1 << OCIE1A); // Habilitar interrupción por comparación
  interrupts();
}

// ── Contador auxiliar para llegar a 30 segundos ────────────────────────
int contadorTimer = 0;
const int DISPAROS_30S = 15; // 15 × 2s = 30 s

// ── Setup ───────────────────────────────────────────────────────────────
void setup() {
  lcd.init();
  lcd.backlight();

  lcd.setCursor(0, 0);
  lcd.print("  CETI  ");
  lcd.setCursor(0, 1);
  lcd.print("  Iniciando...  ");
  delay(1500);
  lcd.clear();

  // Pines de botones con pull-up interno
  // Botón conecta el pin a GND al presionar
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);

  // Interrupciones externas
  attachInterrupt(digitalPinToInterrupt(2), ISR_Int1, FALLING);
  attachInterrupt(digitalPinToInterrupt(3), ISR_Int3, FALLING);

  // Timer1
  configurarTimer1();
}

// ── Loop principal ──────────────────────────────────────────────────────
void loop() {

  // ── Atender Interrupción 1 ───────────────────────────────────────────
  if (flagInt1) {
    flagInt1 = false;
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print("  AÑA  ");
    lcd.setCursor(0, 1);
    lcd.print("INT 1 activada  ");
    delay(2000);
    lcd.clear();
    return; // Reinicia loop para actualizar contador limpio
  }

  // ── Atender Interrupción 2 (Timer cada 30 s) ─────────────────────────
  if (flagInt2) {
    contadorTimer++;
    flagInt2 = false;

    if (contadorTimer >= DISPAROS_30S) {
      contadorTimer = 0;
      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print("Buen dia a todos");
      lcd.setCursor(0, 1);
      lcd.print("INT 2 - Timer1  ");
      delay(2000);
      lcd.clear();
      return;
    }
  }

  // ── Atender Interrupción 3 ───────────────────────────────────────────
  if (flagInt3) {
    flagInt3 = false;
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print("     CETI       ");
    lcd.setCursor(0, 1);
    lcd.print("  INT 3 activa  ");
    delay(2000);
    lcd.clear();
    return;
  }

  // ── Programa principal: mostrar 0–99 cada 500 ms ─────────────────────
  unsigned long ahora = millis();
  if (ahora - ultimoTiempo >= INTERVALO) {
    ultimoTiempo = ahora;

    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print("Contador:");
    lcd.setCursor(0, 1);
    lcd.print(contador);

    contador++;
    if (contador > 99) contador = 0;
  }
}