/*
 * Arduino MEGA – 3 LEDs con Timers (SIN conflictos)
 *
 * LED1 (Pin 13): Timer1 – temporizador interno, 5s ON / 5s OFF
 * LED2 (Pin 12): Timer5 – counter externo Pin 47, toggle cada 10 pulsos
 * LED3 (Pin 9) : Timer3 – librería TimerThree, parpadeo cada N segundos
 *
 * Librería necesaria: "TimerThree" de Paul Stoffregen
 * Instalar: Herramientas → Administrar librerías → buscar "TimerThree"
 */

#include <TimerThree.h>

// ── Pines ──────────────────────────────────────────────────────────────────
#define LED1_PIN  13
#define LED2_PIN  12
#define LED3_PIN   9

// ── N = último dígito diferente de cero de tu matrícula ───────────────────
#define N_SEGUNDOS  8   // <-- CAMBIA ESTE VALOR

// =============================================================================
//  LED 1 – TIMER 1 como TEMPORIZADOR (pulsos internos, modo CTC)
//
//  f_CPU = 16 MHz, prescaler = 1024
//  f_tick = 16.000.000 / 1024 = 15.625 Hz
//  OCR1A = 15.624  →  interrupción cada 1 segundo exacto
//  Se cuentan 5 interrupciones → toggle LED cada 5 segundos
// =============================================================================
volatile uint8_t timer1_segundos = 0;
volatile bool    led1_estado     = false;

void timer1_setup() {
  TCCR1A = 0;
  TCCR1B = 0;
  TCNT1  = 0;

  OCR1A  = 15624;
  TCCR1B = (1 << WGM12) | (1 << CS12) | (1 << CS10); // CTC + prescaler 1024
  TIMSK1 |= (1 << OCIE1A);
}

ISR(TIMER1_COMPA_vect) {
  timer1_segundos++;
  if (timer1_segundos >= 5) {
    timer1_segundos = 0;
    led1_estado = !led1_estado;
    digitalWrite(LED1_PIN, led1_estado ? HIGH : LOW);
  }
}

// =============================================================================
//  LED 2 – TIMER 5 como COUNTER EXTERNO (Pin 47 = T5 en Arduino MEGA)
//
//  CORRECCIONES APLICADAS:
//  1. pinMode(47, INPUT_PULLUP) — evita que el pin flote y genere pulsos falsos
//  2. TCCR5B asignado en una sola instrucción — evita que los bits se pisen
//  3. Debounce en la ISR — evita conteos múltiples por rebote de señal
//
//  CS52=1, CS51=1, CS50=1 → fuente externa pin T5 (47), flanco de subida
//  Modo CTC, OCR5A = 9   → cuenta 0..9 (10 pulsos) y dispara ISR
//  Cada 10 pulsos → toggle LED2
// =============================================================================
volatile bool led2_estado = false;

void timer5_setup() {
  TCCR5A = 0;
  TCCR5B = 0;
  TCNT5  = 0;

  OCR5A = 9; // 10 pulsos externos (cuenta 0, 1, 2 ... 9 → reset)

  // Una sola asignación: CTC + fuente externa pin 47 flanco subida
  TCCR5B = (1 << WGM52) | (1 << CS52) | (1 << CS51) | (1 << CS50);

  TIMSK5 |= (1 << OCIE5A);
}

ISR(TIMER5_COMPA_vect) {
  // Debounce por software: ignora disparos con menos de 50 ms entre sí
  static unsigned long ultimo = 0;
  unsigned long ahora = millis();
  if (ahora - ultimo > 50) {
    ultimo = ahora;
    led2_estado = !led2_estado;
    digitalWrite(LED2_PIN, led2_estado ? HIGH : LOW);
  }
}

// =============================================================================
//  LED 3 – TIMER 3 vía librería TimerThree
//
//  Timer3.initialize(microsegundos) → período de N segundos
//  Pin 9 del MEGA está conectado a OC3A de Timer3
// =============================================================================
void led3_toggle() {
  digitalWrite(LED3_PIN, !digitalRead(LED3_PIN));
}

// =============================================================================
//  SETUP y LOOP
// =============================================================================
void setup() {
  pinMode(LED1_PIN, OUTPUT);
  pinMode(LED2_PIN, OUTPUT);
  pinMode(LED3_PIN, OUTPUT);

  // ── CORRECCIÓN PRINCIPAL: INPUT_PULLUP evita que Pin 47 flote ────────────
  pinMode(47, INPUT_PULLUP);

  digitalWrite(LED1_PIN, LOW);
  digitalWrite(LED2_PIN, LOW);
  digitalWrite(LED3_PIN, LOW);

  timer1_setup(); // LED1: toggle cada 5 segundos
  timer5_setup(); // LED2: toggle cada 10 pulsos externos en Pin 47

  // LED3: parpadeo cada N segundos con TimerThree
  Timer3.initialize((long)N_SEGUNDOS * 1000000L); // microsegundos
  Timer3.attachInterrupt(led3_toggle);

  sei(); // activar interrupciones globales
}

void loop() {
  // Todo ocurre en interrupciones — el loop queda vacío intencionalmente
}
