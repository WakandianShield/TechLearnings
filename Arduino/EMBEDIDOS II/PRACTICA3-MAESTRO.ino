#include <Keypad.h>
#include <Wire.h> 

#define RX_PIN 16
#define TX_PIN 17

#define I2C_SLAVE_TARGET_ADDR 0x08 

const byte FILAS = 4;
const byte COLUMNAS = 4;

char teclas[FILAS][COLUMNAS] = {
  {'1','2','3','A'},
  {'4','5','6','B'},
  {'7','8','9','C'},
  {'*','0','#','D'}
};

byte pinesFilas[FILAS]     = {13, 12, 14, 27}; 
byte pinesColumnas[COLUMNAS] = {26, 25, 33, 32}; 

Keypad teclado = Keypad(makeKeymap(teclas), pinesFilas, pinesColumnas, FILAS, COLUMNAS);

hw_timer_t *timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED;
volatile bool banderaEscaneo = false;
volatile unsigned long contadorInterrupciones = 0;

void IRAM_ATTR onTimer() {
  portENTER_CRITICAL_ISR(&timerMux);
  banderaEscaneo = true;
  contadorInterrupciones++;
  portEXIT_CRITICAL_ISR(&timerMux);
}

void setup() {
  Serial.begin(115200);
  delay(500);

  Serial2.begin(9600, SERIAL_8N1, RX_PIN, TX_PIN);

  Wire.begin(21, 22); 
  
  timer = timerBegin(1000000); 
  timerAttachInterrupt(timer, &onTimer);
  timerAlarm(timer, 50000, true, 0); 

  Serial.println("=== ESP32 A: Maestro I2C/Serial Activo ===");
}

void loop() {
  if (banderaEscaneo) {
    portENTER_CRITICAL(&timerMux);
    banderaEscaneo = false;
    unsigned long currentCount = contadorInterrupciones;
    portEXIT_CRITICAL(&timerMux);

    char tecla = teclado.getKey();
    if (tecla) {
      Serial.print("Interrupción #");
      Serial.print(currentCount);
      Serial.print(" | [I2C Target: 0x08] -> Tecla: ");
      Serial.println(tecla);

      Serial2.write(tecla);
    }
  }
}