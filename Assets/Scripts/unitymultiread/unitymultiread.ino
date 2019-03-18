/*
 * this is the real one
 * Typical pin layout used:
 * -----------------------------------------------
 *             MFRC522      Arduino       Arduino 
 *             Reader/PCD   Uno/101       Mega    
 * Signal      Pin          Pin           Pin     
 * -----------------------------------------------
 * RST/Reset   RST          9             5        
 * SPI SCK     SCK          13 / ICSP-3   52    
 * SPI MOSI    MOSI         11 / ICSP-4   51      
 * SPI MISO    MISO         12 / ICSP-1   50      
 * 
 * MOSI, MISO, SCK and RST can all reside on the same pins for multi use
 */

#include <SPI.h>
#include <MFRC522.h>
// PIN Numbers : RESET + SDAs
#define RST_PIN         5
#define SS_1_PIN        47
#define SS_2_PIN        45
#define SS_3_PIN        43

#define NR_OF_READERS   3
byte ssPins[] = {SS_1_PIN, SS_2_PIN, SS_3_PIN};

// Create an MFRC522 instance :
MFRC522 mfrc522[NR_OF_READERS];
bool readed[NR_OF_READERS];
bool change[NR_OF_READERS];

/**
   Initialize.
*/
void setup() {

  Serial.begin(9600);           // Initialize serial communications with the PC
  while (!Serial);              // Do nothing if no serial port is opened (added for Arduinos based on ATMEGA32U4)

  SPI.begin();                  // Init SPI bus

  /* looking for MFRC522 readers */
  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {
    mfrc522[reader].PCD_Init(ssPins[reader], RST_PIN);
    
    Serial.print(F("Reader "));
    Serial.print(reader);
    Serial.print(F(": "));
    mfrc522[reader].PCD_DumpVersionToSerial();
    
    readed[reader] = false;
    //mfrc522[reader].PCD_SetAntennaGain(mfrc522[reader].RxGain_max);
  }
}

/*
   Main loop.
*/

void loop() {
 
  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {
    /*
      Look for new cards by checking if there isn't a card first
      then checking for the presence of a new card
    */
    String id = "";
    if(!mfrc522[reader].PICC_IsNewCardPresent()){
      if(mfrc522[reader].PICC_IsNewCardPresent()
          && mfrc522[reader].PICC_ReadCardSerial()){
        if(!readed[reader]){
          Serial.print(reader);
          Serial.print(F(","));
          dump_byte_array(mfrc522[reader].uid.uidByte, mfrc522[reader].uid.size);
          Serial.println();
          readed[reader] = true;
          }
        }else{
          if(readed[reader]){
            Serial.print(reader);
            Serial.print(F(",00000000"));
            Serial.println();
            readed[reader] = false;
            }
          }
      }
    }
}

/*
   Helper routine to dump a byte array as hex values to Serial.
*/
void dump_byte_array(byte * buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? "0" : "");
    Serial.print(String(buffer[i], HEX));
  }
}
