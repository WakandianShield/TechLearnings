#include <SPI.h>

// Switches en pines seguros
const int S1 = 22, S2 = 23, S3 = 24, S4 = 25;
const int SS_PIN = 53;

void setup() {
  Serial.begin(9600);

  pinMode(S1, INPUT_PULLUP);
  pinMode(S2, INPUT_PULLUP);
  pinMode(S3, INPUT_PULLUP);
  pinMode(S4, INPUT_PULLUP);

  pinMode(SS_PIN, OUTPUT);
  digitalWrite(SS_PIN, HIGH);

  SPI.begin();
  SPI.setClockDivider(SPI_CLOCK_DIV16);
  SPI.setDataMode(SPI_MODE0);
  SPI.setBitOrder(MSBFIRST);
}

void loop() {
  byte dato = 0;
  dato |= (!digitalRead(S1)) << 0;
  dato |= (!digitalRead(S2)) << 1;
  dato |= (!digitalRead(S3)) << 2;
  dato |= (!digitalRead(S4)) << 3;

  digitalWrite(SS_PIN, LOW);
  SPI.transfer(dato);
  digitalWrite(SS_PIN, HIGH);

  Serial.print("Enviando: 0b");
  Serial.println(dato, BIN);

  delay(200);
}