#include <LiquidCrystal.h>

LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

// -------------------------------
// Pins
// -------------------------------
const int restartButtonPin = 7;   // local restart button to GND
const int buzzerPin = 8;          // buzzer +
const int startSignalPin = 9;     // Arduino input from ESP32-S2 GPIO 4
const int finishedSignalPin = 10; // Arduino output to ESP32 GPIO 14 (through divider)

// -------------------------------
// Timer settings
// -------------------------------
const int startTimeSeconds = 180; // 3 minutes
int totalSeconds = startTimeSeconds;

// -------------------------------
// State
// -------------------------------
bool timerRunning = false;
bool timerFinishedState = false;

int lastStartSignalState = LOW;
int lastRestartButtonState = HIGH;

unsigned long lastTick = 0;

// -------------------------------
// Display helpers
// -------------------------------
void showWaitingScreen()
{
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Waiting for");
  lcd.setCursor(0, 1);
  lcd.print("GAME_STARTED");
}

void showReadyScreen()
{
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Press button");
  lcd.setCursor(0, 1);
  lcd.print("to restart");
}

void showTime()
{
  int minutes = totalSeconds / 60;
  int seconds = totalSeconds % 60;

  lcd.setCursor(0, 0);
  lcd.print("Time Left:      ");

  lcd.setCursor(4, 1);
  if (minutes < 10) lcd.print("0");
  lcd.print(minutes);
  lcd.print(":");
  if (seconds < 10) lcd.print("0");
  lcd.print(seconds);
  lcd.print("   ");
}

// -------------------------------
// Signal helpers
// -------------------------------
void pulseFinishedSignal()
{
  digitalWrite(finishedSignalPin, HIGH);
  delay(500);
  digitalWrite(finishedSignalPin, LOW);
}

// -------------------------------
// Timer control
// -------------------------------
void startTimer()
{
  totalSeconds = startTimeSeconds;
  timerRunning = true;
  timerFinishedState = false;
  lastTick = millis();

  digitalWrite(finishedSignalPin, LOW);

  lcd.clear();
  showTime();
}

void stopTimer()
{
  timerRunning = false;
}

void handleTimerFinished()
{
  timerRunning = false;
  timerFinishedState = true;

  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("TIME UP!");
  lcd.setCursor(0, 1);
  lcd.print("Press to restart");

  for (int i = 0; i < 3; i++)
  {
    tone(buzzerPin, 1000);
    delay(300);
    noTone(buzzerPin);
    delay(200);
  }

  pulseFinishedSignal();
}

// -------------------------------
// Setup
// -------------------------------
void setup()
{
  lcd.begin(16, 2);

  pinMode(restartButtonPin, INPUT_PULLUP);
  pinMode(buzzerPin, OUTPUT);
  pinMode(startSignalPin, INPUT);     // input from ESP32 GPIO 4
  pinMode(finishedSignalPin, OUTPUT);

  digitalWrite(finishedSignalPin, LOW);

  showWaitingScreen();
}

// -------------------------------
// Main loop
// -------------------------------
void loop()
{
  int currentStartSignalState = digitalRead(startSignalPin);
  int currentRestartButtonState = digitalRead(restartButtonPin);

  // Start timer on rising edge from ESP32 GPIO 4
  if (lastStartSignalState == LOW && currentStartSignalState == HIGH)
  {
    startTimer();
  }

  // Local restart button on falling edge
  if (lastRestartButtonState == HIGH && currentRestartButtonState == LOW)
  {
    delay(30);
    if (digitalRead(restartButtonPin) == LOW)
    {
      startTimer();
    }
  }

  lastStartSignalState = currentStartSignalState;
  lastRestartButtonState = currentRestartButtonState;

  if (timerRunning)
  {
    unsigned long now = millis();

    if (now - lastTick >= 1000)
    {
      lastTick += 1000;
      totalSeconds--;
      showTime();

      if (totalSeconds < 0)
      {
        handleTimerFinished();
      }
    }
  }
}