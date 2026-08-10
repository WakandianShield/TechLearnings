const int rele1 = 11; // Relevador 1
const int rele2 = 10; // Relevador 2
const int rele3 = 9;  // Relevador 3
const int rele4 = 8;  // Relevador 4

void setup() {
  Serial.begin(9600);

  pinMode(rele1, OUTPUT);
  pinMode(rele2, OUTPUT);
  pinMode(rele3, OUTPUT);
  pinMode(rele4, OUTPUT);

  // Relés en HIGH = apagados
  digitalWrite(rele1, HIGH);
  digitalWrite(rele2, HIGH);
  digitalWrite(rele3, HIGH);
  digitalWrite(rele4, HIGH);
}

void loop() {
  if (Serial.available()) {
    char c = Serial.read();

    // ENCENDER INDIVIDUAL
    if (c == '1') digitalWrite(rele1, LOW);
    if (c == '2') digitalWrite(rele2, LOW);
    if (c == '3') digitalWrite(rele3, LOW);
    if (c == '4') digitalWrite(rele4, LOW);

    // APAGAR INDIVIDUAL
    if (c == 'a') digitalWrite(rele1, HIGH);
    if (c == 'b') digitalWrite(rele2, HIGH);
    if (c == 'c') digitalWrite(rele3, HIGH);
    if (c == 'd') digitalWrite(rele4, HIGH);

    // ENCENDER TODOS
    if (c == '0') {
      digitalWrite(rele1, LOW);
      digitalWrite(rele2, LOW);
      digitalWrite(rele3, LOW);
      digitalWrite(rele4, LOW);
    }

    // APAGAR TODOS
    if (c == 'z') {
      digitalWrite(rele1, HIGH);
      digitalWrite(rele2, HIGH);
      digitalWrite(rele3, HIGH);
      digitalWrite(rele4, HIGH);
    }
  }
}