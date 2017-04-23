#define IRpin_PIN PIND
#define IRpin 4
#define MAXPULSE 65000
#define NUMPULSES 50
#define RESOLUTION 20 
#define FUZZINESS 20
#define NOT_AN_INTERRUPT -1

#include <EEPROM.h>
#include <avr/pgmspace.h>
#include "ACWHeader.h"

//These Contain all of the pulses from the ir sensor
word pulses[NUMPULSES][2];
byte currentPulse = 0;

//This is used to control the turning on and off of the buzzer every second when needed
int timer;
byte count;

//These contain all of the information needed to use the buttons on the breadboard
const byte armKeyPin = 2;
const byte alarmKeyPin = 3;
const byte mode = CHANGE;
volatile boolean setting, armed, locked, alarmed, toned;

//These contain all the information needed to illuminate the LEDs
const byte notArmedOutPin = 12;
const byte armedOutPin = 13;
byte notArmedOutPinState = HIGH;
byte armedOutPinState = LOW;

//These are used for the buzzer
const byte buzzerPin = 7;
const byte hz = 82;

//These variables are used to control the length and number of passcodes
const byte lengthOfCode = 4;
const byte numberOfCodes = 2;
byte code[lengthOfCode][numberOfCodes];

void setup(void)
{
  //This starts the serial monitor
  Serial.begin(9600);

  //This loops through the following for the number of codes needed
  for(byte i = 0; i < numberOfCodes; i++)
  {
    Serial.print(F("Code "));
    Serial.print(i + 1);
    Serial.print(F(": "));

    //This calls the code setup method
    CodeSetup(i);

    //These are used to ensure the responce from the user is a valid responce
    boolean validResponce;
    char responce;

    //This breaks if the number of codes needed is hit
    if((i + 1) == numberOfCodes)
    {
      break;
    }
    
    do
    {
      Serial.print(F("Would you like to use another code? Y/N: "));

      //This waits until something is input into the serial monitor
      while(!Serial.available());
      {
        
      }

      //This delay is needed to ensure everything is read from the serial port without issue
      delay(20);

      //This reads what is in the serial port into the responce variable
      responce = Serial.read();

      //This checks to see if the input is a valid input
      if(responce == 'y' || responce == 'Y' || responce == 'n' || responce == 'N')
      {
        //This hold the validity of the input
        validResponce = true;

        //This prints the input
        Serial.println(responce);
      }
      else
      {
        //This hold the validity of the input
        validResponce = false;

        //This informs the user that their input was invalid
        Serial.println(F("This is an invalid selection."));
      }
    }while(!validResponce);

    //This ensures everything is read from the serial monitor
    while(Serial.available())
    {
      Serial.read();
    }

    //If the responce is no break out of the set up
    if(responce == 'n' || responce == 'N')
    {
      break;
    }
  }
  
  // initialize timer1 
  noInterrupts(); // disable all interrupts
  TCCR1A = 0;
  TCCR1B = 0;
  
  // Set timer1_counter to the correct value for our interrupt interval
  timer = 34286; // preload timer 65536-16MHz/256/2Hz
  
  TCNT1 = timer; // preload timer
  TCCR1B |= (1 << CS12); // 256 prescaler 
  TIMSK1 |= (1 << TOIE1); // enable timer overflow interrupt
  interrupts(); // enable all interrupts
  
  //configure keyPin as an input and enable the internal pull-up resistor plus initialise interupt
  pinMode(armKeyPin, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(armKeyPin), ArmKeyEvent, mode);
  pinMode(alarmKeyPin, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(alarmKeyPin), AlarmKeyEvent, mode);
  
  pinMode(notArmedOutPin, OUTPUT);
  pinMode(armedOutPin, OUTPUT);
  
  //set toggle to false
  setting = false;
  armed = false;
  locked = false;
  alarmed = false;
  toned = false;
  
  count = 0;

  //This illuminates the not armed LED
  digitalWrite(notArmedOutPin, notArmedOutPinState);
  digitalWrite(armedOutPin, armedOutPinState);

  Serial.println(F("Alarm online."));
}

void CodeSetup(byte codeNumber)
{  
  //These are used to ensure the responce from the user is a valid responce
  boolean validCode, newCode;

  //This checks to see if there is a passcode stored in memory
  if(EEPROM.read(codeNumber * lengthOfCode) == 0 || EEPROM.read(codeNumber * lengthOfCode) == 1 || EEPROM.read(codeNumber * lengthOfCode) == 2 || EEPROM.read(codeNumber * lengthOfCode) == 3 || EEPROM.read(codeNumber * lengthOfCode) == 4 || EEPROM.read(codeNumber * lengthOfCode) == 5 || EEPROM.read(codeNumber * lengthOfCode) == 6 || EEPROM.read(codeNumber * lengthOfCode) == 7 || EEPROM.read(codeNumber * lengthOfCode) == 8 || EEPROM.read(codeNumber * lengthOfCode) == 9)
  {
    //These are used to ensure the responce from the user is a valid responce
    boolean validResponce;
    char responce;
    
    do
    {
      Serial.print(F("Would you like to use the storred code? Y/N: "));

      //This waits until something is input into the serial monitor
      while(!Serial.available());
      {
        
      }

      //This delay is needed to ensure everything is read from the serial port without issue
      delay(20);

      //This reads what is in the serial port into the responce variable
      responce = Serial.read();

      //This checks to see if the input is a valid input
      if(responce == 'y' || responce == 'Y' || responce == 'n' || responce == 'N')
      {
        //This hold the validity of the input
        validResponce = true;

        //This prints the input
        Serial.println(responce);

        //If the responce is yes
        if(responce == 'y' || responce == 'Y')
        {
          //These variables stop the program from overwriting the code that is about to be read both in ram and flash memory
          validCode = true;
          newCode = false;

          //This reads the code from EEPROM into ram
          for(byte i = 0; i < lengthOfCode; i++)
          {
            code[i][codeNumber] = EEPROM.read((codeNumber * lengthOfCode) + i);
          }
        }
        else
        {
          //This ensures that a new code is input in the next section
          validCode = false;
        }
      }
      else
      {
        //This hold the validity of the input
        validResponce = false;

        //This informs the user that their input was invalid
        Serial.println(F("This is an invalid selection."));
      }
    }while(!validResponce);

    //This ensures everything is read from the serial monitor
    while(Serial.available())
    {
      Serial.read();
    }
  }
  else
  {
    //This ensures that a new code is input in the next section
    validCode = false;
  }

  //This ensures that a new code is input
  while(!validCode)
  {
    Serial.print(F("Please enter a "));
    Serial.print(lengthOfCode);
    Serial.print(F(" digit code: "));
    
    validCode = true;

    //This waits until something is input into the serial monitor
    while(!Serial.available());
    {
      
    }

    //This delay is needed to ensure everything is read from the serial port without issue
    delay(20);

    //This reads the code from the serial monitor into ram
    for(byte i = 0; i < lengthOfCode; i++)
    {
      //This reads what is in the serial port into the code variable in the correct place
      code[i][codeNumber] = Serial.read();

      //This checks to see if the input is a valid input
      if(code[i][codeNumber] == '0' || code[i][codeNumber] == '1' || code[i][codeNumber] == '2' || code[i][codeNumber] == '3' || code[i][codeNumber] == '4' || code[i][codeNumber] == '5' || code[i][codeNumber] == '6' || code[i][codeNumber] == '7' || code[i][codeNumber] == '8' || code[i][codeNumber] == '9')
      {
        code[i][codeNumber] = code[i][codeNumber] - '0';
      }
      else
      {
        //This hold the validity of the input
        validCode = false;

        //This informs the user that their input was invalid
        Serial.println(F("Code rejected"));
        
        break;
      }
    }

    //This ensures everything is read from the serial monitor
    while(Serial.available())
    {
      Serial.read();
    }

    //If the code was accepted
    if(validCode)
    {
      //This informs the program later that this code needs to be written to EEPROM
      newCode = true;

      //This prints the new code onto the serial monitor
      for(byte i = 0; i < lengthOfCode; i++)
      {
        Serial.print(code[i][codeNumber]);
      }

      //This informs the user that the inout was valid
      Serial.println(F(" Code accepted."));
    }
  }

  //If the code need to be sotred in EEPROM
  if(newCode)
  {
    //This stored the new code in EEPROM
    for(byte i = 0; i < lengthOfCode; i++)
    {
      EEPROM.write((codeNumber * lengthOfCode) + i, code[i][codeNumber]);
    }
  }
}

ISR(TIMER1_OVF_vect) // interrupt service routine 
{
  TCNT1 = timer; // preload timer

  //If the buzzer is on or the alarm is alarmed or the exit delay has not yet ellapsed
  if(toned || alarmed || setting)
  {
    //If 20 seconds have passed
    if(count >= 40)
    {
      //This reinitialises the variables and arms the alarm
      count = 0;
      setting = false;
      
      Arm();
    }
    else
    {
      if(setting)
      {
        //Increments cout every half a second
        count++;
      }
    }

    //If the buzzer is off
    if(!toned)
    {
      toned = true;

      //Turn the buzzer on
      tone(buzzerPin, hz);
    }
    else
    {
      toned = false;

      //Turn the buzzer off
      noTone(buzzerPin);
    }
  }
  else
  {
    
  }
}

void loop(void)
{
  //This checks to see if the interact button on the remote has been pressed
  if(RemoteInput() == 10)
  {
    //This stops the program from doing things when it shouldnt
    if(!setting)
    {
      //If the alarm isnt armed or on
      if(!armed && !alarmed)
      {
        //This checks to see if the passcode has been entered
        if(CheckCode())
        {
          //This arms the alarm
           InitialiseArm();
        }
        else
        {
          //This shows an invalid input has been entered
          Serial.println(F("Code rejected."));
        }
      }
      else
      {
        //This checks to see if the passcode has been entered
        if(CheckCode())
        {
          //This disarms the alarm
          Disarm();
        }
        else
        {
          //This shows an invalid input has been entered
          Serial.println(F("Code rejected."));
        }
      }
    }
  }
}

void ArmKeyEvent(void)
{
  //This is the interupt for the arm button and simply calls another method
  InitialiseArm();
}

void InitialiseArm()
{
  //If the alarm is not being set has not been armed and is not currently alarmed
  if(!setting && !armed && !alarmed)
  {
    //This causes the exit delay
    setting = true;
    
    Serial.println(F("Setting"));
  }
}

void Arm(void)
{
  //This locks whatever is to be locked
  Lock();

  //This boolean keeps trakc of the alarms state
  armed = true;

  //This changes the LEDs from red on to green on
  notArmedOutPinState = LOW;
  armedOutPinState = HIGH;
  
  digitalWrite(notArmedOutPin, notArmedOutPinState);
  digitalWrite(armedOutPin, armedOutPinState);
  
  Serial.println(F("Armed"));
}

void Lock(void)
{
  //If not locked
  if(!locked)
  {
    //Locks something
    locked = true;
    Serial.println(F("Locked"));
  }
}

void Disarm(void)
{
  //This unlocks whatever is to be locked
  Unlock();

  //This ensures the alarm is disarmed and stops the siren
  armed = false;
  alarmed = false;

  //This changes the LEDs from green on to red on
  notArmedOutPinState = HIGH;
  armedOutPinState = LOW;
  
  digitalWrite(notArmedOutPin, notArmedOutPinState);
  digitalWrite(armedOutPin, armedOutPinState);
  
  Serial.println(F("Disarmed"));
}

void Unlock(void)
{
  //If locked
  if(locked)
  {
    //Unlocks something
    locked = false;
    Serial.println(F("Unlocked"));
  }
}

void AlarmKeyEvent(void)
{
  //This is the interupt for the alarm button and simply calls another method
  Alarmed();
}

void Alarmed(void)
{
  //If the alarm is armed
  if(armed)
  {
    //Sounds the alarm
    armed = false;
    alarmed = true;

    //Turns both LEDs on
    notArmedOutPinState = HIGH;
    armedOutPinState = HIGH;
    
    digitalWrite(notArmedOutPin, notArmedOutPinState);
    digitalWrite(armedOutPin, armedOutPinState);
    
    Serial.println(F("Alarmed"));
  }
}

boolean CheckCode(void)
{
  //These are temporary variables to hold the state of the check function
  byte checkCode[lengthOfCode];
  bool codeSelector[numberOfCodes];

  //For each boolean in codeSelector makes it true
  for(byte i = 0; i < numberOfCodes; i++)
  {
    codeSelector[i] = true;
  }

  //For the length of the code
  for(byte i = 0; i < lengthOfCode; i++)
  {
    //This takes an input from the remote
    Serial.print(F("Please enter the next digit of your code via the remote: "));
    checkCode[i] = RemoteInput();

    //If the number recieved is one of the fail states imediatly exits the method
    if(checkCode[i] == -1 || checkCode[i] == 10 || checkCode[i] == 255)
    {
      return false;
    }

    //This checks each code at the point in the code where we are recieveing the number for if it is not the same number it makes the boolean for that code false
    for(byte j = 0; j < numberOfCodes; j++)
    {
      if(checkCode[i] != code[i][j])
      {
        codeSelector[j] = false;
      }
    }

    //This prints the number input
    Serial.println(checkCode[i]);
  }

  //This checks all of the codes to see if any of the boolean variable are still true
  for(byte j = 0; j < numberOfCodes; j++)
  {
    //If so returns true
    if(codeSelector[j] == true)
    {
      return true;
    }
  }

  return false;
}

int RemoteInput(void)
{
  //This takes in the input from the remote
  int numberPulses = ListenForIR();

  //This checks the first 18 pulses from the remote as they are all the same
  if(PreambleCheck(18))
  {
    //This checks the next 18 against all known pulses
    return ButtonCheck((numberPulses - 18));
  }
  else
  {
    return -1;
  }
}

int ListenForIR(void)
{
  currentPulse = 0;
  
  while(true)
  {
    //These variables contain the length of the high and low pulses
    uint16_t highPulse, lowPulse;
    highPulse = lowPulse = 0;
  
    while(IRpin_PIN & (1 << IRpin))
    {
      //This adds one to the high pulse
      highPulse++;
      
      //This delays the program by the resolution defined at the top of the page
      delayMicroseconds(RESOLUTION);

      //If the maximum number of pulses has been met and the current number of pulses isnt zero return the pulses
      if(((highPulse >= MAXPULSE) && (currentPulse != 0)) || (currentPulse == NUMPULSES))
      {
        return currentPulse;
      }
    }

    //This adds the high pulses to the pulses array
    pulses[currentPulse][0] = highPulse;
    
    while(!(IRpin_PIN & _BV(IRpin)))
    {
      //This adds one to the high pulse
      lowPulse++;
      
      //This delays the program by the resolution defined at the top of the page
      delayMicroseconds(RESOLUTION);

      //If the maximum number of pulses has been met and the current number of pulses isnt zero return the pulses
      if(((lowPulse >= MAXPULSE)  && (currentPulse != 0)) || (currentPulse == NUMPULSES))
      {
        return currentPulse;
      }
    }

    //This adds the high pulses to the pulses array
    pulses[currentPulse][1] = lowPulse;
    
    currentPulse++;
  }
}

boolean PreambleCheck(int numberPulses)
{
  //This is a temporary variable to hold the array read from progmem
  int buffer[36];

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Preamble + i);
  }

  //If the input from the ir sensor matches the known preamble return true
  if(IRComparePreamble(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return true;
  }
  
  return false;
}

int ButtonCheck(int numberPulses)
{
  //This is a temporary variable to hold the array read from progmem
  int buffer[40];

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Zero + i);
  }

  //If the input from the ir sensor matches zero return 0
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 0;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(One + i);
  }

  //If the input from the ir sensor matches one return 1
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 1;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Two + i);
  }

  //If the input from the ir sensor matches two return 2
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 2;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Three + i);
  }

  //If the input from the ir sensor matches three return 3
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 3;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Four + i);
  }

  //If the input from the ir sensor matches four return 4
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 4;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Five + i);
  }

  //If the input from the ir sensor matches five return 5
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 5;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Six + i);
  }

  //If the input from the ir sensor matches six return 6
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 6;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Seven + i);
  }

  //If the input from the ir sensor matches seven return 7
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 7;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Eight + i);
  }

  //If the input from the ir sensor matches eight return 8
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 8;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Nine + i);
  }

  //If the input from the ir sensor matches nine return 9
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 9;
  }

  //This for loop reads the array from progmem into the buffer array int by int
  for(byte i = 0; i < sizeof(buffer) / 2; i++)
  {
    buffer[i] = pgm_read_word_near(Power + i);
  }

  //If the input from the ir sensor matches power return 10
  if(IRCompare(numberPulses, buffer, sizeof(buffer) / 4))
  {
    return 10;
  }

  //Else return -1
  return -1;
}

boolean IRComparePreamble(int numberPulses, const int signal[], int refSize)
{
  //This is the number of pulses in the array
  int count = min(numberPulses, refSize);
  int onCode;
  int offCode;

  //For every pulse in the array
  for(byte i = 0; i < count - 1; i++)
  {
    //These are the high and low values
    onCode = pulses[i][1] * RESOLUTION / 10;
    offCode = pulses[i + 1][0] * RESOLUTION / 10;

    //If the high value does not match the stored high value with a margine for error defined at the top of the page return false
    if(!(abs(onCode - signal[i * 2 + 0]) <= (signal[i * 2 + 0] * FUZZINESS / 100)))
    {
      return false;
    }

    //If the low value does not match the stored high value with a margine for error defined at the top of the page return false
    if(!(abs(offCode - signal[i * 2 + 1]) <= (signal[i * 2 + 1] * FUZZINESS / 100)))
    {
      return false;
    }
  }
  
  return true;
}

boolean IRCompare(int numberPulses, const int signal[], int refSize)
{
  //This is the number of pulses in the array
  int count = min(numberPulses, refSize);
  int onCode;
  int offCode;
  byte j = 18;
  
  for(byte i = 0; i < count - 1; i++)
  {
    //These are the high and low values
    onCode = pulses[j][1] * RESOLUTION / 10;
    offCode = pulses[j + 1][0] * RESOLUTION / 10;
    j++;

    //If the high value does not match the stored high value with a margine for error defined at the top of the page return false
    if(!(abs(onCode - signal[i * 2 + 0]) <= (signal[i * 2 + 0] * FUZZINESS / 100)))
    {
      return false;
    }

    //If the low value does not match the stored high value with a margine for error defined at the top of the page return false
    if(!(abs(offCode - signal[i * 2 + 1]) <= (signal[i * 2 + 1] * FUZZINESS / 100)))
    {
      return false;
    }
  }
  
  return true;
}
