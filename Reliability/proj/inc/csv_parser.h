#ifndef CSVPARSER
#define CSVPARSER

#define FILE_OPEN_ERROR "Cannot open file!"
#define FILE_IS_FOLDER_ERROR "Given path is folder!"
#define TOKEN_SPLIT_ERROR "Could not split line!"
#define CSV_OBJ_CREATED_OK "OK"
#define CSV_OBJ_CREATED_ERROR "Could allocate memory for object!"
#define CSV_ENTITY_ERROR "Could not assign entity!"

#define MAX_LINE_LENGTH 100
#define MAX_FILE_SIZE 1300
#define MAX_TOKENS 6
#define CSV_CREATE_BUFF_SIZE 34

typedef struct
{
    char *Entity;
    int Code;
    int Year;
    int CivilWars;
    int InterstateWars;
} CSVLine;

void FileParser(char filePath[], char parserBuffer[][MAX_LINE_LENGTH]);
void LineSplitter(char fileLine[], char splitterBuffer[MAX_TOKENS][MAX_LINE_LENGTH]);
void CSVLine_Destroy(CSVLine *csvLineObj);
CSVLine *CSVLine_Create(char entity[], int code, unsigned int civilWars, unsigned int interStateWars, char constructorStatus[CSV_CREATE_BUFF_SIZE]);

#endif