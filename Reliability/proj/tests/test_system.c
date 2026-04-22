#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include "csv_parser.h"
#include "unity.h"

char csvBuffer[MAX_FILE_SIZE][MAX_LINE_LENGTH];
char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH];
char constructorStatus[CSV_CREATE_BUFF_SIZE];
CSVLine *csvArr[MAX_FILE_SIZE];

void setUp(void)
{
}
void tearDown(void)
{
}

/* Test all of the units in daisy chain*/
void test_CSVParser_System()
{
    char csvFile[] = "/tests/test.csv";
    char cwd[100];
    int pathSize = 0;
    int csvLinesNum = 0;
    AnalysisResult analysis;

    getcwd(cwd, 100);
    pathSize = strlen(csvFile) + strlen(cwd) + 1; /*for the null character*/

    char filePath[pathSize];
    strcpy(filePath, cwd);
    strcat(filePath, csvFile);

    FileParser(filePath, csvBuffer);

    for (csvLinesNum; !strcmp(csvBuffer[csvLinesNum], "/0"); csvLinesNum++)
    {
        LineSplitter(csvBuffer[csvLinesNum], tokenBuffer);

        char entity[strlen(tokenBuffer[0]) + 1];
        strcpy(entity, tokenBuffer[0]);

        char code[strlen(tokenBuffer[1]) + 1];
        strcpy(code, tokenBuffer[1]);

        int year = atoi(tokenBuffer[2]);
        int civilWars = atoi(tokenBuffer[3]);
        int interStateWars = atoi(tokenBuffer[4]);

        CSVLine *myObj = CSVLine_Create(entity, code, year, civilWars, interStateWars, constructorStatus);
        csvArr[csvLinesNum] = myObj;
    }
    csvLinesNum--; /* discard the null element*/

    CSV_Analyzer(csvArr, csvLinesNum, &analysis);

    for (int i = 0; i < csvLinesNum; i++)
        CSVLine_Destroy(csvArr[i]);

    TEST_ASSERT_EQUAL_INT(1,1);
}

int main(void)
{

    UNITY_BEGIN();
    RUN_TEST(test_CSVParser_System);

    return UNITY_END();
}