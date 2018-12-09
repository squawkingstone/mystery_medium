/*
 * --------------------------------------------------------------------------------------------------------------------
 * Example sketch/program showing how to read new NUID from a PICC to serial.
 * --------------------------------------------------------------------------------------------------------------------
 * This is a MFRC522 library example; for further details and other examples see: https://github.com/miguelbalboa/rfid
 * 
 * Example sketch/program showing how to the read data from a PICC (that is: a RFID Tag or Card) using a MFRC522 based RFID
 * Reader on the Arduino SPI interface.
 * 
 * When the Arduino and the MFRC522 module are connected (see the pin layout below), load this sketch into Arduino IDE
 * then verify/compile and upload it. To see the output: use Tools, Serial Monitor of the IDE (hit Ctrl+Shft+M). When
 * you present a PICC (that is: a RFID Tag or Card) at reading distance of the MFRC522 Reader/PCD, the serial output
 * will show the type, and the NUID if a new card has been detected. Note: you may see "Timeout in communication" messages
 * when removing the PICC from reading distance too early.
 * 
 * @license Released into the public domain.
 * 
 * Typical pin layout used:
 * -----------------------------------------------
 *             MFRC522      Arduino       Arduino 
 *             Reader/PCD   Uno/101       Mega    
 * Signal      Pin          Pin           Pin     
 * -----------------------------------------------
 * RST/Reset   RST          9             5       
 * SPI SS      SDA(SS)      10            53      
 * SPI MOSI    MOSI         11 / ICSP-4   51      
 * SPI MISO    MISO         12 / ICSP-1   50      
 * SPI SCK     SCK          13 / ICSP-3   52      
 * 
 * MOSI, MISO, SCK and RST can all reside on the same pins for multi use
 * 
 */

#include <SPI.h>
#include <MFRC522.h>

// PIN Numbers : RESET + SDAs
#define RST_PIN         5
#define SS_1_PIN        47
#define SS_2_PIN        45
#define SS_3_PIN        43
#define SS_4_PIN        41
#define SS_5_PIN        39
#define SS_6_PIN        38

#define NR_OF_READERS   6
byte ssPins[] = {SS_1_PIN, SS_2_PIN, SS_3_PIN, SS_4_PIN, SS_5_PIN, SS_6_PIN};

// Create an MFRC522 instance :
MFRC522 mfrc522[NR_OF_READERS];
bool readed[NR_OF_READERS];
String uid[NR_OF_READERS];
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
    change[reader] = false;
    uid[reader] = String();
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
          uid[reader] += arrayToString(mfrc522[reader].uid.uidByte, mfrc522[reader].uid.size);
          Serial.print(F("Reader "));
          Serial.print(reader);
          Serial.print(F(": Card UID:"));
          dump_byte_array(mfrc522[reader].uid.uidByte, mfrc522[reader].uid.size);
          Serial.print(F(" Being read"));
          Serial.println();
          readed[reader] = true;
          printCards();
          }
        }else{
          if(readed[reader]){
            uid[reader] = String();
            Serial.print(F("Reader "));
            Serial.print(reader);
            Serial.print(F(" not being read"));
            Serial.println();
            readed[reader] = false;
            printCards();
            }
          }
      }
    }
}

/*
   prints all cards that are actively being read atm
*/
void printCards(){
  Serial.println("Cards that are being read: ");
  for(uint8_t i = 0; i < NR_OF_READERS; i++){
    Serial.print(F("Reader "));
    Serial.print(i);
    Serial.print(F(" reading: "));
    Serial.println(String(uid[i]));
    }
  }

/*
   Helper routine to dump a byte array as hex values to Serial.
*/
void dump_byte_array(byte * buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? " 0" : " ");
    Serial.print(String(buffer[i], HEX));
  }
}

String arrayToString(byte * buffer, byte bufferSize){
   String card = String();
    for (byte i = 0; i < bufferSize; i++) {
    card = card + String(buffer[i] < 0x10 ? " 0" : " ");
    card = card + String(buffer[i], HEX);
  }
  return card;
  }
