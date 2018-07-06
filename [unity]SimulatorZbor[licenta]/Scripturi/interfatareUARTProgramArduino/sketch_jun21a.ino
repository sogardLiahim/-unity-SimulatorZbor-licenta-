
#include <LiquidCrystal.h>

 int senzorValue1 = 0;
 int senzorValue2 = 0;
 int senzorValue3 = 0;
 byte isIn;
 char buffer[7];
 String textVelocity;
 LiquidCrystal lcd(12, 11, 5, 4, 3, 2);



void setup()
{

  pinMode(A0,INPUT);
  pinMode(A1,INPUT);
  pinMode(A2,INPUT);
  Serial.begin(250000);
  while (!Serial) {}
  lcd.begin(16, 2);
}


void loop()
{
  senzorValue2 = (map(analogRead(A0),0,1024,-45,45)); 
  senzorValue1 = (map(analogRead(A1),0,1024,30,-30));
  senzorValue3 = (map(analogRead(A2),550,800,10,1));
  sprintf(buffer,"%d|%d|%d", senzorValue1,senzorValue2,senzorValue3);
  Serial.println(buffer);
  lcdPrintAndRecive();
  
}

void lcdPrintAndRecive() 
  {
  while (Serial.available() > 0)
  {
  textVelocity = Serial.read();
  }
  lcd.setCursor(0,0);
  lcd.print("V:(m/s)"); 
  lcd.setCursor(0,1);
  lcd.print(textVelocity);
  lcd.print("  ");
}

 

 


