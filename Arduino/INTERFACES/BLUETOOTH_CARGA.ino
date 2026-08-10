#include <SoftwareSerial.h>

const int rele1 = 11;
const int rele2 = 10; 
const int rele3 = 9;
const int rele4 = 8;

SoftwareSerial bt(2, 3); // RX, TX

void setup() {
  Serial.begin(9600);
  bt.begin(9600);
  
  pinMode(rele1, OUTPUT);
  pinMode(rele2, OUTPUT);
  pinMode(rele3, OUTPUT);
  pinMode(rele4, OUTPUT);
  
  // Iniciar apagados
  digitalWrite(rele1, HIGH);
  digitalWrite(rele2, HIGH);
  digitalWrite(rele3, HIGH);
  digitalWrite(rele4, HIGH);
  
  Serial.println("SISTEMA LISTO");
}

void loop() {
  if (bt.available()) {
    char c = bt.read();
    
    // Tus comandos originales
    if (c == '1') digitalWrite(rele1, LOW);
    if (c == '2') digitalWrite(rele2, LOW);
    if (c == '3') digitalWrite(rele3, LOW);
    if (c == '4') digitalWrite(rele4, LOW);
    
    if (c == 'a') digitalWrite(rele1, HIGH);
    if (c == 'b') digitalWrite(rele2, HIGH);
    if (c == 'c') digitalWrite(rele3, HIGH);
    if (c == 'd') digitalWrite(rele4, HIGH);
    
    if (c == '0') {
      digitalWrite(rele1, LOW);
      digitalWrite(rele2, LOW);
      digitalWrite(rele3, LOW);
      digitalWrite(rele4, LOW);
    }
    
    if (c == 'z') {
      digitalWrite(rele1, HIGH);
      digitalWrite(rele2, HIGH);
      digitalWrite(rele3, HIGH);
      digitalWrite(rele4, HIGH);
    }
  }
}