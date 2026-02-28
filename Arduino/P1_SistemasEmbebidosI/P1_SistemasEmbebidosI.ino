// === PINES ===
const int dip[8]  = {22,23,24,25,26,27,28,29};
const int leds[8] = {30,31,32,33,34,35,36,37};
const int seg[7]  = {38,39,40,41,42,43,44};
const int boton   = 45;

// === PATRONES 7 SEGMENTOS (a,b,c,d,e,f,g) ===
const byte NUMS[10][7] = {
  {1,1,1,1,1,1,0},{0,1,1,0,0,0,0},{1,1,0,1,1,0,1},{1,1,1,1,0,0,1},{0,1,1,0,0,1,1},
  {1,0,1,1,0,1,1},{1,0,1,1,1,1,1},{1,1,1,0,0,0,0},{1,1,1,1,1,1,1},{1,1,1,1,0,1,1}
};
const byte LETRAS[8][7] = {
  {1,0,1,1,0,1,1}, // S [0]
  {1,1,1,0,1,1,1}, // A [1]
  {1,1,1,0,1,1,0}, // N [2]
  {1,0,0,0,1,1,0}, // T [3]
  {0,1,1,0,0,0,0}, // I [4]
  {0,0,0,0,0,0,1}, // - [5]
  {1,1,1,1,1,1,0}, // O/D [6]
  {1,0,0,1,1,1,1}  // E [7]
};
const byte SEG_C[7]  = {1,0,0,1,1,1,0};
const byte SEG_OFF[7]= {0,0,0,0,0,0,0};

// INICIO: I N I C I O
const byte* INICIO[6] = { LETRAS[4],LETRAS[2],LETRAS[4],SEG_C,LETRAS[4],LETRAS[6] };
// SANTI-ANDRE
const byte* NOMBRE[11]= { LETRAS[0],LETRAS[1],LETRAS[2],LETRAS[3],LETRAS[4],
                           LETRAS[5],LETRAS[1],LETRAS[2],LETRAS[6],LETRAS[1],LETRAS[7] };

// === FUNCIONES ===
void mostrarSeg(const byte p[7]) { for(int i=0;i<7;i++) digitalWrite(seg[i],p[i]); }

void mostrarMensaje(const byte** msg, int len, int t) {
  for(int i=0;i<len;i++){ mostrarSeg(msg[i]); delay(t); mostrarSeg(SEG_OFF); delay(100); }
}

void mostrarNumero(int n) {
  if(n>=100){ mostrarSeg(NUMS[n/100]); delay(400); mostrarSeg(SEG_OFF); delay(80); }
  if(n>=10) { mostrarSeg(NUMS[(n%100)/10]); delay(400); mostrarSeg(SEG_OFF); delay(80); }
  mostrarSeg(NUMS[n%10]); delay(400); mostrarSeg(SEG_OFF); delay(80);
}

void mostrarLEDs(byte v) { for(int i=0;i<8;i++) digitalWrite(leds[i],(v>>i)&1); }

byte leerDIP() {
  byte v=0;
  for(int i=0;i<8;i++) if(digitalRead(dip[i])==HIGH) v|=(1<<i);
  return v;
}

void esperarEnter() {
  while(digitalRead(boton)==HIGH);  delay(50);
  while(digitalRead(boton)==LOW);   delay(50);
}

// === SETUP ===
void setup() {
  for(int i=0;i<8;i++){ pinMode(dip[i],INPUT_PULLUP); pinMode(leds[i],OUTPUT); }
  for(int i=0;i<7;i++)  pinMode(seg[i],OUTPUT);
  pinMode(boton,INPUT_PULLUP);
}

// === LOOP ===
void loop() {

  // ETAPA 1: INICIO
  mostrarMensaje(INICIO, 6, 500);
  mostrarSeg(SEG_OFF); delay(500);

  // ETAPA 2: 3 numeros -> promedio
  unsigned int suma = 0;
  for(int n=0; n<3; n++){
    for(int p=0;p<3;p++){ mostrarSeg(NUMS[n+1]); delay(300); mostrarSeg(SEG_OFF); delay(200); }
    esperarEnter();
    byte val = leerDIP();
    suma += val;
    mostrarLEDs(val);
    mostrarNumero((int)val);
    delay(500);
  }

  // Promedio
  byte prom = suma / 3;
  mostrarLEDs(prom);
  for(int r=0;r<5;r++){ mostrarNumero((int)prom); delay(300); }
  mostrarLEDs(0); mostrarSeg(SEG_OFF); delay(800);

  // ETAPA 3: Nombre SANTI-ANDRE
  for(int r=0;r<1;r++){ mostrarMensaje(NOMBRE,11,450); delay(500); }
  mostrarSeg(SEG_OFF);
  delay(3000);
}
