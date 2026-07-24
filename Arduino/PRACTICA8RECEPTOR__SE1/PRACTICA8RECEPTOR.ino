// RECEPTOR - LEDs en pines 8, 9, 10, 11
#include <Wire.h>

const int LED1 = 8, LED2 = 9, LED3 = 10, LED4 = 11;
#define MY_ADDR 0x08

volatile byte datoRecibido = 0;
volatile bool hayDato = false;

void setup() {
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

  Wire.begin(MY_ADDR);
  Wire.onReceive(recibirDato);
  Serial.begin(9600);
}

void loop() {
  if (hayDato) {
    hayDato = false;
    byte dato = datoRecibido;

    Serial.print("Recibido: 0b");
    Serial.println(dato, BIN);
    Serial.print("LED1="); Serial.print((dato >> 0) & 1);
    Serial.print(" LED2="); Serial.print((dato >> 1) & 1);
    Serial.print(" LED3="); Serial.print((dato >> 2) & 1);
    Serial.print(" LED4="); Serial.println((dato >> 3) & 1);
  }
}

void recibirDato(int n) {
  while (Wire.available()) {
    datoRecibido = Wire.read();
    digitalWrite(LED1, (datoRecibido >> 0) & 1);
    digitalWrite(LED2, (datoRecibido >> 1) & 1);
    digitalWrite(LED3, (datoRecibido >> 2) & 1);
    digitalWrite(LED4, (datoRecibido >> 3) & 1);
    hayDato = true;
  }
}