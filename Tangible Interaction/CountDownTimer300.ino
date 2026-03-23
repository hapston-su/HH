#include <LiquidCrystal.h>

LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

const int buttonPin = 7;
const int buzzerPin = 8;

const int startTime = 180;   // 3 minutes = 180 seconds
int totalSeconds = startTime;

bool timerRunning = false;
bool finished = false;

void setup() {
  lcd.begin(16, 2);

  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(buzzerPin, OUTPUT);

  lcd.setCursor(0, 0);
  lcd.print("Press button");
  lcd.setCursor(0, 1);
  lcd.print("to start");
}

void loop() {
  int buttonState = digitalRead(buttonPin);

  // Button pressed
  if (buttonState == LOW) {
    delay(200); // simple debounce

    totalSeconds = startTime;
    timerRunning = true;
    finished = false;
    lcd.clear();
  }

  if (timerRunning) {
    int minutes = totalSeconds / 60;
    int seconds = totalSeconds % 60;

    lcd.setCursor(0, 0);
    lcd.print("Time Left:");

    lcd.setCursor(4, 1);
    if (minutes < 10) lcd.print("0");
    lcd.print(minutes);
    lcd.print(":");
    if (seconds < 10) lcd.print("0");
    lcd.print(seconds);
    lcd.print("   ");

    delay(1000);
    totalSeconds--;

    if (totalSeconds < 0) {
      timerRunning = false;
      finished = true;

      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print("TIME UP!");

      // Beep 3 times
      for (int i = 0; i < 3; i++) {
        tone(buzzerPin, 1000); // 1000 Hz
        delay(300);
        noTone(buzzerPin);
        delay(200);
      }

      lcd.setCursor(0, 1);
      lcd.print("Press to restart");
    }
  }
}