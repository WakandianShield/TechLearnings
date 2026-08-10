#include <Wire.h>
#include <Adafruit_TCS34725.h>
#include <LiquidCrystal_I2C.h>

// ── LCD I2C: si no muestra nada cambia 0x27 por 0x3F
LiquidCrystal_I2C lcd(0x27, 16, 2);

// ── Sensor de color TCS34725
Adafruit_TCS34725 tcs = Adafruit_TCS34725(
  TCS34725_INTEGRATIONTIME_50MS,
  TCS34725_GAIN_4X
);

// ── Pines
const int PIN_LUZ    = 7;   // Sensor de luz digital DO
const int PIN_SONIDO = A0;  // Sensor de sonido analogico
const int PIN_RELE1  = 4;   // Foco 1 -> controlado por serial (luz del cuarto)
const int PIN_RELE2  = 5;   // Foco 2 -> enciende cuando detecta verde correcto
const int PIN_RELE3  = 6;   // Foco 3 -> encendido = banda en funcionamiento

// ── Umbral de sonido MUY bajo para maxima sensibilidad
// Detecta sonidos aun mas debiles
int UMBRAL_SONIDO = 20;

// ── Umbrales de verde para TCS34725 (ajustar con STATUS apuntando al objeto verde)
int VERDE_G_MIN = 100;
int VERDE_R_MAX = 120;
int VERDE_B_MAX = 120;

// ── Contador de perdidas
int perdidas = 0;

// ── Temporizador de 5 segundos para deteccion de color
unsigned long tiempoUltimVerde = 0;
const unsigned long INTERVALO_COLOR = 5000;

// ── Estado del cuarto (foco 1 controlado por serial)
bool cuartoEncendido = false;

// ── Prototipos
void apagar_focos_proceso();
void mostrar_lcd(String l1, String l2);
void leer_serial();
void mostrar_ayuda();
bool detectar_verde();
int leer_sonido_pico();

// ─────────────────────────────────────────────────────────────
void setup() {
  Serial.begin(9600);

  pinMode(PIN_LUZ,   INPUT);
  pinMode(PIN_RELE1, OUTPUT);
  pinMode(PIN_RELE2, OUTPUT);
  pinMode(PIN_RELE3, OUTPUT);

  // Relevadores invertidos:
  // HIGH = encendido
  // LOW  = apagado
  digitalWrite(PIN_RELE1, LOW);
  digitalWrite(PIN_RELE2, LOW);
  digitalWrite(PIN_RELE3, LOW);

  lcd.init();
  lcd.backlight();
  mostrar_lcd("Iniciando...", "");

  if (!tcs.begin()) {
    mostrar_lcd("ERROR TCS34725", "Revisa SDA/SCL");
    Serial.println("ERROR: TCS34725 no encontrado.");
    Serial.println("Verifica: SDA -> pin 20, SCL -> pin 21");
    while (1);
  }

  mostrar_lcd("Sistema listo", "Escribe AYUDA");
  tiempoUltimVerde = millis();
  mostrar_ayuda();
}

// ─────────────────────────────────────────────────────────────
void loop() {
  leer_serial();

  bool hayLuz   = (digitalRead(PIN_LUZ) == LOW);
  int  sonido   = leer_sonido_pico();   // pico maximo en 50ms
  bool hayVibra = (sonido > UMBRAL_SONIDO);

  // CASO 1: cuarto apagado por comando serial
  if (!cuartoEncendido) {
    apagar_focos_proceso();
    mostrar_lcd("OPERACION", "APAGADA");
    delay(500);
    return;
  }

  // CASO 2: cuarto encendido pero sensor no detecta luz
  if (!hayLuz) {
    apagar_focos_proceso();
    mostrar_lcd("OPERACION", "APAGADA");
    delay(500);
    return;
  }

  // CASO 3: hay luz pero no hay sonido -> FALLA EN BANDA
  if (!hayVibra) {
    apagar_focos_proceso();
    mostrar_lcd("FALLA EN BANDA", "Sin sonido");
    Serial.println("ERROR,falla_en_banda,sin_sonido_de_motor");
    delay(500);
    return;
  }

  // OPERACION NORMAL: hay luz y hay sonido
  digitalWrite(PIN_RELE3, HIGH);

  // Revisar color cada 5 segundos
  unsigned long ahora = millis();
  if (ahora - tiempoUltimVerde >= INTERVALO_COLOR) {
    tiempoUltimVerde = ahora;

    bool verde = detectar_verde();

    if (verde) {
      digitalWrite(PIN_RELE2, HIGH);
      mostrar_lcd("Banda OK", "Perdidas: " + String(perdidas));
      Serial.print("OK,verde_detectado,perdidas=");
      Serial.println(perdidas);
      delay(400);
      digitalWrite(PIN_RELE2, LOW);
    } else {
      perdidas++;
      digitalWrite(PIN_RELE2, LOW);
      mostrar_lcd("Perdidas: " + String(perdidas), "Obj no detectado");
      Serial.print("ERROR,objeto_perdido,perdidas=");
      Serial.println(perdidas);
    }

  } else {
    int seg = (INTERVALO_COLOR - (ahora - tiempoUltimVerde)) / 1000;
    mostrar_lcd("Banda OK", "Sig scan: " + String(seg) + "s");
  }

  delay(200);
}

// ─────────────────────────────────────────────────────────────
// Lee el sensor de sonido 50 veces en 50ms y devuelve el pico maximo.
// Asi no se pierde ningun sonido breve o debil.
int leer_sonido_pico() {
  int pico = 0;
  for (int i = 0; i < 50; i++) {
    int val = analogRead(PIN_SONIDO);
    if (val > pico) pico = val;
    delay(1);
  }
  return pico;
}

// ─────────────────────────────────────────────────────────────
bool detectar_verde() {
  uint16_t r, g, b, c;
  tcs.getRawData(&r, &g, &b, &c);
  if (c == 0) return false;
  float nr = (float)r / c * 255;
  float ng = (float)g / c * 255;
  float nb = (float)b / c * 255;
  return (ng >= VERDE_G_MIN && nr <= VERDE_R_MAX && nb <= VERDE_B_MAX && ng > nr && ng > nb);
}

void apagar_focos_proceso() {
  digitalWrite(PIN_RELE2, LOW);
  digitalWrite(PIN_RELE3, LOW);
}

void mostrar_lcd(String l1, String l2) {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(l1.substring(0, 16));
  lcd.setCursor(0, 1);
  lcd.print(l2.substring(0, 16));
}

void mostrar_ayuda() {
  Serial.println("=== Control de Calidad - Banda Transportadora ===");
  Serial.println("Comandos:");
  Serial.println("  LUZ_ON        -> Enciende foco 1 (cuarto encendido)");
  Serial.println("  LUZ_OFF       -> Apaga foco 1 (cuarto apagado)");
  Serial.println("  STATUS        -> Ver lectura actual de todos los sensores");
  Serial.println("  RESET         -> Reiniciar contador de perdidas a cero");
  Serial.println("  UMBRAL_S xxx  -> Cambiar umbral de sonido (ej: UMBRAL_S 80)");
  Serial.println("  AYUDA         -> Mostrar este menu");
  Serial.println("");
  Serial.println("Trama serial salida:");
  Serial.println("  ESTADO,descripcion,perdidas=N");
  Serial.println("=================================================");
}

void leer_serial() {
  if (!Serial.available()) return;
  String cmd = Serial.readStringUntil('\n');
  cmd.trim();

  if (cmd == "LUZ_ON") {
    cuartoEncendido = true;
    digitalWrite(PIN_RELE1, HIGH);
    Serial.println("Foco 1 encendido -> cuarto ON");
    mostrar_lcd("Cuarto ON", "Banda lista");
    tiempoUltimVerde = millis();

  } else if (cmd == "LUZ_OFF") {
    cuartoEncendido = false;
    digitalWrite(PIN_RELE1, LOW);
    Serial.println("Foco 1 apagado -> cuarto OFF");
    mostrar_lcd("OPERACION", "APAGADA");

  } else if (cmd == "STATUS") {
    uint16_t r, g, b, c;
    tcs.getRawData(&r, &g, &b, &c);
    float nr = c > 0 ? (float)r / c * 255 : 0;
    float ng = c > 0 ? (float)g / c * 255 : 0;
    float nb = c > 0 ? (float)b / c * 255 : 0;
    int pico = leer_sonido_pico();
    Serial.println("── STATUS ──────────────────────");
    Serial.print("Sonido pico : "); Serial.print(pico);
    Serial.print("  (umbral="); Serial.print(UMBRAL_SONIDO); Serial.println(")");
    Serial.print("Hay sonido  : "); Serial.println(pico > UMBRAL_SONIDO ? "SI" : "NO");
    Serial.print("Luz         : "); Serial.println(digitalRead(PIN_LUZ) == LOW ? "DETECTADA" : "NO detectada");
    Serial.print("Cuarto      : "); Serial.println(cuartoEncendido ? "ON" : "OFF");
    Serial.print("Color raw   : R="); Serial.print(r);
    Serial.print(" G="); Serial.print(g);
    Serial.print(" B="); Serial.println(b);
    Serial.print("Color norm  : R="); Serial.print(nr, 1);
    Serial.print(" G="); Serial.print(ng, 1);
    Serial.print(" B="); Serial.println(nb, 1);
    Serial.print("Verde?      : "); Serial.println(detectar_verde() ? "SI" : "NO");
    Serial.print("Perdidas    : "); Serial.println(perdidas);
    Serial.println("────────────────────────────────");
    mostrar_lcd("STATUS", "Ver serial...");

  } else if (cmd == "RESET") {
    perdidas = 0;
    tiempoUltimVerde = millis();
    Serial.println("Perdidas reiniciadas a 0.");
    mostrar_lcd("Perdidas", "reiniciadas: 0");

  } else if (cmd.startsWith("UMBRAL_S ")) {
    int val = cmd.substring(9).toInt();
    if (val > 0 && val < 1024) {
      UMBRAL_SONIDO = val;
      Serial.print("Umbral sonido -> "); Serial.println(val);
      mostrar_lcd("Umbral sonido", String(val));
    } else {
      Serial.println("Valor invalido. Rango: 1-1023");
    }

  } else if (cmd == "AYUDA") {
    mostrar_ayuda();

  } else if (cmd.length() > 0) {
    Serial.print("Comando no reconocido: '");
    Serial.print(cmd);
    Serial.println("'  Escribe AYUDA.");
  }
}