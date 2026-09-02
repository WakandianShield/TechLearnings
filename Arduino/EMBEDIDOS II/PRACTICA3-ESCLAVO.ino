#include <Wire.h>
#include <LiquidCrystal_I2C.h>

#define RX_PIN 16
#define TX_PIN 17

#define SDA_LCD 21
#define SCL_LCD 22

LiquidCrystal_I2C lcd(0x27, 16, 2); 

String textoMostrado = "";

void setup() {
  Serial.begin(115200);
  delay(1500);
  Serial.println("\nIniciando ESP32 B (Esclavo)...");

  Serial2.begin(9600, SERIAL_8N1, RX_PIN, TX_PIN);

  Wire.begin(SDA_LCD, SCL_LCD);
  lcd.init(); 
  lcd.backlight();
  
  lcd.setCursor(0, 0);
  lcd.print("Tecla: ");

  Serial.println("=== ESP32 B listo y escuchando ===");
}

void loop() {
  if (Serial2.available() > 0) {
    char c = Serial2.read();

    Serial.print("Dato recibido por Serial2: ");
    Serial.println(c);

    if (c == '#') {
      textoMostrado = "";             
      lcd.setCursor(0, 0);
      lcd.print("Tecla:          "); 
    } 
    else if (c == '*') {
      String textoTemporal = textoMostrado;

      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print("EDGAR-SANTI");
      lcd.setCursor(0, 1);
      lcd.print("Pausa de 5s...");

      delay(5000); 

      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print("Tecla: ");
      lcd.print(textoTemporal);

      textoMostrado = textoTemporal;
    } 
    else {
      if (textoMostrado.length() < 10) { 
        textoMostrado += c;
      }
      
      lcd.setCursor(0, 0);
      lcd.print("Tecla:          "); 
      lcd.setCursor(7, 0);          
      lcd.print(textoMostrado);     
    }
  }
}