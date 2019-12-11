#include <eGizmo_Kto12BOT.h>

//Codes & Modified by e-Gizmo Mechatronix Central 
//Ardublocks at http://www.e-gizmo.net 
eGizmo_Kto12BOT KBOT;
char _ABVAR_1_INPUT_DATA = ' ' ;

void setup()
{
  // Set the KBOT begin 
  KBOT.BEGIN();
  Serial.begin(9600);
}

void loop()
{
  //READ/SET ALL THE SENSORS 
  KBOT.LDR_SENSE(); 
  KBOT.MEASURE_IN_CM(); 
  KBOT.MIC_DIGITAL(); 
  KBOT.MIC_ANALOG(); 
  KBOT.VIB_SENSE(); 

  if (( ( Serial.available() ) > ( 0 ) ))
  {
    _ABVAR_1_INPUT_DATA = Serial.read();
  }
  KBOT.PRINT("LDR =");
  KBOT.GET_DATA(KBOT.LDR_READ);
  KBOT.PRINTLN("");
  if (( ( KBOT.LDR_READ ) < ( 800 ) ))
  {
    if (( ( _ABVAR_1_INPUT_DATA ) == ( 119 ) ))
    {
      KBOT.FORWARD(255);
      KBOT.HEADLIGHT_LEFT(1);
      KBOT.HEADLIGHT_RIGHT(1);
    }
    if (( ( _ABVAR_1_INPUT_DATA ) == ( 97 ) ))
    {
      KBOT.TURNLEFT(255);
      KBOT.HEADLIGHT_LEFT(1);
      KBOT.HEADLIGHT_RIGHT(0);
    }
    if (( ( _ABVAR_1_INPUT_DATA ) == ( 100 ) ))
    {
      KBOT.TURNRIGHT(255);
      KBOT.HEADLIGHT_LEFT(0);
      KBOT.HEADLIGHT_RIGHT(1);
    }
    if (( ( _ABVAR_1_INPUT_DATA ) == ( 115 ) ))
    {
      KBOT.REVERSE(255);
      KBOT.HEADLIGHT_LEFT(1);
      KBOT.HEADLIGHT_RIGHT(1);
    }
    if (( ( _ABVAR_1_INPUT_DATA ) == ( 120 ) ))
    {
      KBOT.STOP(0);
      KBOT.HEADLIGHT_LEFT(0);
      KBOT.HEADLIGHT_RIGHT(0);
    }
  }
  else
  {
    KBOT.STOP(0);
    KBOT.HORN();
  }
}
