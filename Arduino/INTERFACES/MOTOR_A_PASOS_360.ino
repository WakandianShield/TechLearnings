#include <Stepper.h>

// 28BYJ-48

const int stepsPerRevolution = 2038; 
Stepper myStepper(stepsPerRevolution, 8, 10, 9, 11);

void setup() {
  myStepper.setSpeed(12);
  delay(1000);
}

void loop() {
  myStepper.step(stepsPerRevolution);
  delay(100);
}