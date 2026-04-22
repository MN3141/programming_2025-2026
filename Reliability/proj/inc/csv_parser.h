#ifndef CSVPARSER
#define CSVPARSER

#define OK_CODE 0
#define FILE_OPEN_ERR_CODE -1   /* Cannot open given file*/
#define FILE_IS_DIR_ERR_CODE -2 /* Given file is actually a path (applicable to Linux systems)*/

#define LINE_SPLITTER_OK OK_CODE

#define CSV_OBJ_CREATED_OK "OK"
#define CSV_OBJ_CREATED_ERROR "Could allocate memory for object!"
#define CSV_ENTITY_ERROR "Could not assign entity!"
#define CSV_CODE_ERROR "Could not assign code!"

#define MAX_LINE_LENGTH 100
#define MAX_FILE_SIZE 1300
#define MAX_TOKENS 6
#define CSV_CREATE_BUFF_SIZE 34

#define ANALYSIS_RESULT_OK OK_CODE
#define ANALYSIS_RESULT_EMPTY_WARNING -3 /* The given number of elements is null*/

typedef struct
{
    char *Entity;
    char *Code;
    int Year;
    int CivilWars;
    int InterstateWars;
} CSVLine;

typedef struct
{
    int Max;
    int Min;
    float Mean;
    float Median;
} AnalysisResult;

/* File processing*/
int FileParser(char filePath[], char parserBuffer[][MAX_LINE_LENGTH]);
int LineSplitter(char fileLine[], char splitterBuffer[MAX_TOKENS][MAX_LINE_LENGTH]);

/* Object constructor and destructor*/
CSVLine *CSVLine_Create(char entity[], char code[], unsigned int year, unsigned int civilWars, unsigned int interStateWars, char constructorStatus[CSV_CREATE_BUFF_SIZE]);
void CSVLine_Destroy(CSVLine *csvLineObj);

/* Utils*/
int CSV_Analyzer(CSVLine *csvLines[], int numElems, AnalysisResult *analysis);
#endif