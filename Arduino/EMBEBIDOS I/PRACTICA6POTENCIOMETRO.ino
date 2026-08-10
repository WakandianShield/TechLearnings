/*
  =====================================================
  Práctica: ADC + PWM + Comunicación Serial
  Tarjeta: Arduino MEGA
  =====================================================
  - Potenciómetro en A0  → lectura analógica (ADC)
  - LED en Pin 9 (~PWM)  → control de intensidad
  - Serial Monitor       → visualización de valores
  =====================================================
*/

// ── Definición de pines ──────────────────────────────
const int PIN_POT = A0;   // Entrada analógica del potenciómetro
const int PIN_LED = 9;    // Salida PWM para el LED (~9 en MEGA)

// ── Variables ────────────────────────────────────────
int  valorADC  = 0;       // Valor crudo del ADC  (0 – 1023)
int  valorPWM  = 0;       // Valor mapeado para PWM (0 – 255)
float voltaje  = 0.0;     // Voltaje equivalente en la entrada

void setup() {
  // Configura la comunicación serial a 9600 baudios
  Serial.begin(9600);

  // El pin PWM como salida
  pinMode(PIN_LED, OUTPUT);

  // Mensaje de bienvenida en el Monitor Serial
  Serial.println("========================================");
  Serial.println("  ADC + PWM + Serial - Arduino MEGA");
  Serial.println("========================================");
  Serial.println("  ADC (0-1023) | Voltaje (V) | PWM (0-255)");
  Serial.println("----------------------------------------");
}

void loop() {
  // 1. ADQUISICIÓN ANALÓGICA (ADC 10 bits → 0 a 1023)
  valorADC = analogRead(PIN_POT);

  // 2. CONVERSIÓN: mapear ADC (0-1023) → PWM (0-255)
  valorPWM = map(valorADC, 0, 1023, 0, 255);

  // 3. CALCULAR voltaje equivalente (referencia = 5V)
  voltaje = (valorADC * 5.0) / 1023.0;

  // 4. CONTROL PWM → intensidad del LED
  analogWrite(PIN_LED, valorPWM);

  // 5. TRANSMISIÓN SERIAL → Monitor Serial del IDE
  Serial.print("  ADC: ");
  Serial.print(valorADC);
  Serial.print("\t\t| V: ");
  Serial.print(voltaje, 2);   // 2 decimales
  Serial.print(" V\t\t| PWM: ");
  Serial.println(valorPWM);

  // Pequeña pausa para no saturar el puerto serial
  delay(200);
}