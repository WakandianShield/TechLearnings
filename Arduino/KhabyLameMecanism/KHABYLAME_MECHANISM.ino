#include <Stepper.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>

// Configuración del stepper
const int stepsPerRevolution = 2038;
Stepper myStepper(stepsPerRevolution, 8, 10, 9, 11);

// Configuración de la LCD
LiquidCrystal_I2C lcd(0x27, 16, 2);

// Pin del optoacoplador (que controla el TRIAC)
const int lampPin = 12; // Pin que controla el optoacoplador MOC3021

// Variables de tiempo
unsigned long previousTime = 0;
const long interval = 5000; // 5 segundos
int hours = 0;
int minutes = 0;
int seconds = 0;

// Variables de control
bool clockRunning = true;
unsigned long pauseTime = 0;

// Sistema de alarmas
struct Alarm {
  int hour;
  int minute;
  int second;
  bool active;
  bool triggered;
};

const int MAX_ALARMS = 5;
Alarm alarms[MAX_ALARMS];
int alarmCount = 0;
bool lampState = false;

void setup() {
  // Inicializar comunicación serial
  Serial.begin(9600);
  
  // Configurar pin del optoacoplador
  pinMode(lampPin, OUTPUT);
  digitalWrite(lampPin, HIGH); // Lámpara apagada al inicio
  
  // Inicializar LCD
  lcd.init();
  lcd.backlight();
  lcd.print("Iniciando...");
  delay(2000);
  lcd.clear();
  
  // Configurar stepper
  myStepper.setSpeed(12);
  delay(1000);
  
  // Inicializar alarmas
  for (int i = 0; i < MAX_ALARMS; i++) {
    alarms[i].active = false;
    alarms[i].triggered = false;
  }
  
  previousTime = millis();
  
  Serial.println("========================================");
  Serial.println("  RELOJ KHABY LAME CON ALARMAS");
  Serial.println("========================================");
  Serial.println();
  Serial.println("COMANDOS DISPONIBLES:");
  Serial.println("  PAUSE / STOP    - Pausar reloj");
  Serial.println("  START / RESUME  - Reanudar reloj");
  Serial.println("  SET HH:MM:SS    - Establecer hora");
  Serial.println("  ALARM HH:MM:SS  - Crear alarma");
  Serial.println("  LIST            - Ver alarmas");
  Serial.println("  CLEAR           - Borrar alarmas");
  Serial.println("  LAMP ON         - Encender lámpara");
  Serial.println("  LAMP OFF        - Apagar lámpara");
  Serial.println("  STATUS          - Ver estado");
  Serial.println("========================================");
  Serial.println();
}

void loop() {
  unsigned long currentTime = millis();
  
  // Control del stepper - SOLO gira si el reloj está running
  if (clockRunning) {
    myStepper.step(1);
  }
  
  // Actualizar tiempo cada 5 segundos solo si está running
  if (clockRunning && currentTime - previousTime >= interval) {
    previousTime = currentTime;
    updateTime();
    checkAlarms(); // Verificar alarmas cada vez que se actualiza el tiempo
    displayTime();
  }
  
  // Verificar comandos seriales
  checkSerialCommands();
}

void updateTime() {
  seconds += 5;
  
  if (seconds >= 60) {
    seconds = 0;
    minutes++;
    
    if (minutes >= 60) {
      minutes = 0;
      hours++;
      
      if (hours >= 24) {
        hours = 0;
      }
    }
  }
}

void displayTime() {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("KHABY LAME CLOCK");
  
  // Mostrar indicador de alarmas activas
  if (alarmCount > 0) {
    lcd.setCursor(15, 0);
    lcd.print("A");
  }
  
  lcd.setCursor(4, 1);
  
  // Formatear horas
  if (hours < 10) lcd.print("0");
  lcd.print(hours);
  lcd.print(":");
  
  // Formatear minutos
  if (minutes < 10) lcd.print("0");
  lcd.print(minutes);
  lcd.print(":");
  
  // Formatear segundos
  if (seconds < 10) lcd.print("0");
  lcd.print(seconds);
  
  // Mostrar estado de la lámpara
  if (lampState) {
    lcd.setCursor(13, 1);
    lcd.print("ON");
  }
}

void checkSerialCommands() {
  if (Serial.available() > 0) {
    String command = Serial.readStringUntil('\n');
    command.trim();
    command.toUpperCase();
    
    if (command == "PAUSE" || command == "STOP") {
      pauseClock();
    }
    else if (command == "START" || command == "RESUME") {
      resumeClock();
    }
    else if (command.startsWith("SET ")) {
      setTime(command);
    }
    else if (command.startsWith("ALARM ")) {
      addAlarm(command);
    }
    else if (command == "LIST") {
      listAlarms();
    }
    else if (command == "CLEAR") {
      clearAlarms();
    }
    else if (command == "LAMP ON") {
      turnLampOn();
    }
    else if (command == "LAMP OFF") {
      turnLampOff();
    }
    else if (command == "STATUS") {
      showStatus();
    }
    else {
      Serial.println("✗ Comando no reconocido");
      Serial.println("Use: PAUSE, START, SET, ALARM, LIST, CLEAR, LAMP ON/OFF, STATUS");
    }
  }
}

void pauseClock() {
  if (clockRunning) {
    clockRunning = false;
    pauseTime = millis();
    Serial.println("✓ Reloj PAUSADO");
    lcd.clear();
    lcd.print("RELOJ PAUSADO");
    delay(1000);
    displayTime();
  } else {
    Serial.println("! El reloj ya está pausado");
  }
}

void resumeClock() {
  if (!clockRunning) {
    unsigned long pauseDuration = millis() - pauseTime;
    previousTime += pauseDuration;
    clockRunning = true;
    Serial.println("✓ Reloj REANUDADO");
    displayTime();
  } else {
    Serial.println("! El reloj ya está corriendo");
  }
}

void setTime(String command) {
  String timeStr = command.substring(4);
  
  int h = timeStr.substring(0, 2).toInt();
  int m = timeStr.substring(3, 5).toInt();
  int s = timeStr.substring(6, 8).toInt();
  
  if (h >= 0 && h < 24 && m >= 0 && m < 60 && s >= 0 && s < 60) {
    hours = h;
    minutes = m;
    seconds = s;
    previousTime = millis();
    
    Serial.print("✓ Hora establecida: ");
    if (hours < 10) Serial.print("0");
    Serial.print(hours);
    Serial.print(":");
    if (minutes < 10) Serial.print("0");
    Serial.print(minutes);
    Serial.print(":");
    if (seconds < 10) Serial.print("0");
    Serial.println(seconds);
    
    displayTime();
  } else {
    Serial.println("✗ Hora inválida. Use formato: SET HH:MM:SS");
  }
}

void addAlarm(String command) {
  if (alarmCount >= MAX_ALARMS) {
    Serial.println("✗ Máximo de alarmas alcanzado (5)");
    Serial.println("  Use CLEAR para borrar alarmas");
    return;
  }
  
  String timeStr = command.substring(6);
  
  int h = timeStr.substring(0, 2).toInt();
  int m = timeStr.substring(3, 5).toInt();
  int s = timeStr.substring(6, 8).toInt();
  
  if (h >= 0 && h < 24 && m >= 0 && m < 60 && s >= 0 && s < 60) {
    alarms[alarmCount].hour = h;
    alarms[alarmCount].minute = m;
    alarms[alarmCount].second = s;
    alarms[alarmCount].active = true;
    alarms[alarmCount].triggered = false;
    
    Serial.print("✓ Alarma #");
    Serial.print(alarmCount + 1);
    Serial.print(" creada: ");
    if (h < 10) Serial.print("0");
    Serial.print(h);
    Serial.print(":");
    if (m < 10) Serial.print("0");
    Serial.print(m);
    Serial.print(":");
    if (s < 10) Serial.print("0");
    Serial.println(s);
    
    alarmCount++;
  } else {
    Serial.println("✗ Hora inválida. Use formato: ALARM HH:MM:SS");
  }
}

void listAlarms() {
  Serial.println("========================================");
  Serial.println("         ALARMAS CONFIGURADAS");
  Serial.println("========================================");
  
  if (alarmCount == 0) {
    Serial.println("  No hay alarmas configuradas");
  } else {
    for (int i = 0; i < alarmCount; i++) {
      Serial.print("  [");
      Serial.print(i + 1);
      Serial.print("] ");
      
      if (alarms[i].hour < 10) Serial.print("0");
      Serial.print(alarms[i].hour);
      Serial.print(":");
      if (alarms[i].minute < 10) Serial.print("0");
      Serial.print(alarms[i].minute);
      Serial.print(":");
      if (alarms[i].second < 10) Serial.print("0");
      Serial.print(alarms[i].second);
      
      if (alarms[i].triggered) {
        Serial.println(" [ACTIVADA]");
      } else {
        Serial.println();
      }
    }
  }
  Serial.println("========================================");
}

void clearAlarms() {
  alarmCount = 0;
  for (int i = 0; i < MAX_ALARMS; i++) {
    alarms[i].active = false;
    alarms[i].triggered = false;
  }
  Serial.println("✓ Todas las alarmas borradas");
  displayTime();
}

void checkAlarms() {
  for (int i = 0; i < alarmCount; i++) {
    if (alarms[i].active && !alarms[i].triggered) {
      if (alarms[i].hour == hours && 
          alarms[i].minute == minutes && 
          alarms[i].second == seconds) {
        
        // ¡ALARMA ACTIVADA!
        alarms[i].triggered = true;
        triggerAlarm(i);
      }
    }
  }
}

void triggerAlarm(int alarmIndex) {
  // Encender la lámpara
  digitalWrite(lampPin, LOW);
  lampState = true;
  
  Serial.println();
  Serial.println("========================================");
  Serial.println("       *** ALARMA ACTIVADA ***");
  Serial.println("========================================");
  Serial.print("  Alarma #");
  Serial.print(alarmIndex + 1);
  Serial.print(" - ");
  if (alarms[alarmIndex].hour < 10) Serial.print("0");
  Serial.print(alarms[alarmIndex].hour);
  Serial.print(":");
  if (alarms[alarmIndex].minute < 10) Serial.print("0");
  Serial.print(alarms[alarmIndex].minute);
  Serial.print(":");
  if (alarms[alarmIndex].second < 10) Serial.print("0");
  Serial.println(alarms[alarmIndex].second);
  Serial.println("  Lámpara ENCENDIDA");
  Serial.println("  Use 'LAMP OFF' para apagar");
  Serial.println("========================================");
  Serial.println();
  
  // Mostrar en LCD
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("*** ALARMA ***");
  lcd.setCursor(0, 1);
  if (alarms[alarmIndex].hour < 10) lcd.print("0");
  lcd.print(alarms[alarmIndex].hour);
  lcd.print(":");
  if (alarms[alarmIndex].minute < 10) lcd.print("0");
  lcd.print(alarms[alarmIndex].minute);
  lcd.print(":");
  if (alarms[alarmIndex].second < 10) lcd.print("0");
  lcd.print(alarms[alarmIndex].second);
  lcd.print("  AC:ON");
  
  delay(3000); // Mostrar mensaje por 3 segundos
  displayTime();
}

void turnLampOn() {
  digitalWrite(lampPin, LOW);
  lampState = true;
  Serial.println("✓ Lámpara ENCENDIDA manualmente");
  displayTime();
}

void turnLampOff() {
  digitalWrite(lampPin, HIGH);
  lampState = false;
  Serial.println("✓ Lámpara APAGADA");
  displayTime();
}

void showStatus() {
  Serial.println("========================================");
  Serial.println("       ESTADO DEL SISTEMA");
  Serial.println("========================================");
  
  Serial.print("  Hora actual: ");
  if (hours < 10) Serial.print("0");
  Serial.print(hours);
  Serial.print(":");
  if (minutes < 10) Serial.print("0");
  Serial.print(minutes);
  Serial.print(":");
  if (seconds < 10) Serial.print("0");
  Serial.println(seconds);
  
  Serial.print("  Estado reloj: ");
  Serial.println(clockRunning ? "CORRIENDO" : "PAUSADO");
  
  Serial.print("  Lámpara AC: ");
  Serial.println(lampState ? "ENCENDIDA" : "APAGADA");
  
  Serial.print("  Alarmas activas: ");
  Serial.print(alarmCount);
  Serial.println("/5");
  
  Serial.println("  Control: MOC3021 + BTA08");
  Serial.print("  Pin lámpara: ");
  Serial.println(lampPin);
  
  Serial.println("========================================");
}