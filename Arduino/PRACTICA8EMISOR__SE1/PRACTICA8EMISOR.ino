// EMISOR - Switches en pines 8, 9, 10, 11
#include <Wire.h>

const int S1 = 8, S2 = 9, S3 = 10, S4 = 11;
#define SLAVE_ADDR 0x08

void setup() {
  Wire.begin();
  Serial.begin(9600);
  pinMode(S1, INPUT_PULLUP);
  pinMode(S2, INPUT_PULLUP);
  pinMode(S3, INPUT_PULLUP);
  pinMode(S4, INPUT_PULLUP);
}

void loop() {
  byte dato = 0;
  dato |= (!digitalRead(S1)) << 0;
  dato |= (!digitalRead(S2)) << 1;
  dato |= (!digitalRead(S3)) << 2;
  dato |= (!digitalRead(S4)) << 3;

  Wire.beginTransmission(SLAVE_ADDR);
  Wire.write(dato);
  Wire.endTransmission();

  Serial.print("Enviando: 0b");
  Serial.println(dato, BIN);
  delay(200);
}