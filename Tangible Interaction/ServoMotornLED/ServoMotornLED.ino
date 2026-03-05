#include <ESP32Servo.h>

static const int SERVO_PIN = 9;   
static const int BUTTON_PIN = 0;  

// LED pins
static const int LED1_PIN = 5;
static const int LED2_PIN = 6;
static const int LED3_PIN = 7;

Servo s;

bool paused = false;
bool lastButtonState = HIGH;

void pauseAwareDelay(int ms)
{
  unsigned long start = millis();

  while (millis() - start < ms)
  {
    bool buttonState = digitalRead(BUTTON_PIN);

    if (lastButtonState == HIGH && buttonState == LOW)
    {
      paused = !paused;

      if (paused)
        Serial.println("PAUSED");
      else
        Serial.println("RESUMED");

      delay(200);
    }

    lastButtonState = buttonState;

    while (paused)
    {
      buttonState = digitalRead(BUTTON_PIN);

      if (lastButtonState == HIGH && buttonState == LOW)
      {
        paused = !paused;
        Serial.println("RESUMED");
        delay(200);
      }

      lastButtonState = buttonState;
    }

    delay(10);
  }
}

void setup() {
  Serial.begin(115200);
  delay(200);

  pinMode(BUTTON_PIN, INPUT_PULLUP);

  // LED outputs
  pinMode(LED1_PIN, OUTPUT);
  pinMode(LED2_PIN, OUTPUT);
  pinMode(LED3_PIN, OUTPUT);

  s.setPeriodHertz(50);
  s.attach(SERVO_PIN, 500, 2500);

  Serial.println("Servo identifier test starting...");
}

void loop() {

  // Phase 1
  Serial.println("Command: write(0) for 2s");
  digitalWrite(LED1_PIN, HIGH);
  digitalWrite(LED2_PIN, LOW);
  digitalWrite(LED3_PIN, LOW);

  s.write(0);
  pauseAwareDelay(2000);

  // Phase 2
  Serial.println("Command: write(90), wait 1s");
  digitalWrite(LED1_PIN, LOW);
  digitalWrite(LED2_PIN, HIGH);
  digitalWrite(LED3_PIN, LOW);

  s.write(90);
  pauseAwareDelay(1000);

  // Phase 3
  Serial.println("Command: write(180), wait 1s");
  digitalWrite(LED1_PIN, LOW);
  digitalWrite(LED2_PIN, LOW);
  digitalWrite(LED3_PIN, HIGH);

  s.write(180);
  pauseAwareDelay(1000);

  Serial.println("Command: write(0), wait 1s");

  digitalWrite(LED1_PIN, HIGH);
  digitalWrite(LED2_PIN, LOW);
  digitalWrite(LED3_PIN, LOW);

  s.write(0);
  pauseAwareDelay(1000);

  Serial.println("---- cycle repeat ----");
  pauseAwareDelay(1000);
}