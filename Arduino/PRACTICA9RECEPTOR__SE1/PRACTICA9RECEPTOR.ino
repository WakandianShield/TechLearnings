#include <SPI.h>

const int LED1 = 22, LED2 = 23, LED3 = 24, LED4 = 25;

volatile byte datoRecibido = 0;
volatile bool hayDato = false;

void setup() {
  Serial.begin(9600);

  pinMode(LED1, OUTPUT);
  pinMode(LED2, OUTPUT);
  pinMode(LED3, OUTPUT);
  pinMode(LED4, OUTPUT);

  // Test visual al arrancar
  digitalWrite(LED1, HIGH); digitalWrite(LED2, HIGH);
  digitalWrite(LED3, HIGH); digitalWrite(LED4, HIGH);
  delay(1000);
  digitalWrite(LED1, LOW);  digitalWrite(LED2, LOW);
  digitalWrite(LED3, LOW);  digitalWrite(LED4, LOW);

  // Configurar como esclavo SPI
  pinMode(MISO, OUTPUT);   // Esclavo controla MISO
  pinMode(MOSI, INPUT);
  pinMode(SCK,  INPUT);
  pinMode(SS,   INPUT);

  SPCR |= _BV(SPE);        // Habilitar SPI
  SPCR &= ~_BV(MSTR);      // Modo esclavo
  SPCR |= _BV(SPIE);       // Habilitar interrupción SPI

  sei(); // Habilitar interrupciones globales
}

// ISR - Se ejecuta cuando llega un byte completo por SPI
ISR(SPI_STC_vect) {
  datoRecibido = SPDR;  // Leer registro de datos SPI
  hayDato = true;
}

void loop() {
  if (hayDato) {
    hayDato = false;
    byte dato = datoRecibido;

    // Actualizar LEDs
    digitalWrite(LED1, (dato >> 0) & 1);
    digitalWrite(LED2, (dato >> 1) & 1);
    digitalWrite(LED3, (dato >> 2) & 1);
    digitalWrite(LED4, (dato >> 3) & 1);

    Serial.print("Recibido: 0b");
    Serial.println(dato, BIN);
    Serial.print("LED1="); Serial.print((dato >> 0) & 1);
    Serial.print(" LED2="); Serial.print((dato >> 1) & 1);
    Serial.print(" LED3="); Serial.print((dato >> 2) & 1);
    Serial.print(" LED4="); Serial.println((dato >> 3) & 1);
  }
}