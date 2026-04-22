#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include "csv_parser.h"
#include "unity.h"

char csvBuffer[MAX_FILE_SIZE][MAX_LINE_LENGTH];
char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH];
char tokenBuffer2[MAX_TOKENS][MAX_LINE_LENGTH];
char constructorStatus[CSV_CREATE_BUFF_SIZE];

void setUp(void)
{
}
void tearDown(void)
{
}

/* Test basic interaction between LineReader and LineSplitter*/
void test_CSV_LineRead_LineSplit()
{

    char csvFile[] = "/tests/test.csv";
    char cwd[100];
    int pathSize = 0;
    char *expectedTokens[] = {
        "North Africa and the Middle East", "", "2011", "1", "0"};

    getcwd(cwd, 100);
    pathSize = strlen(csvFile) + strlen(cwd) + 1; /*for the null character*/

    char filePath[pathSize];
    strcpy(filePath, cwd);
    strcat(filePath, csvFile);

    FileParser(filePath, csvBuffer);

    char firstReadLine[strlen(csvBuffer[0]) + 1];
    strcpy(firstReadLine, csvBuffer[0]);

    LineSplitter(firstReadLine, tokenBuffer);

    for (int i = 0; i < 5; i++)
        TEST_ASSERT_EQUAL_STRING(expectedTokens[i], tokenBuffer[i]);
}

/* Test basic interaction between LineSplitter and CSVLine_Create*/
void test_CSV_LineSplit_LineCreate()
{

    char parsedLine[] = "Americas,,1800,0,0";

    LineSplitter(parsedLine, tokenBuffer);

    char entity[strlen(tokenBuffer[0]) + 1];
    strcpy(entity, tokenBuffer[0]);

    char code[strlen(tokenBuffer[1]) + 1];
    strcpy(code, tokenBuffer[1]);

    int year = atoi(tokenBuffer[2]);
    int civilWars = atoi(tokenBuffer[3]);
    int interStateWars = atoi(tokenBuffer[4]);

    CSVLine *myObj = CSVLine_Create(entity, code, year, civilWars, interStateWars, constructorStatus);

    TEST_ASSERT_EQUAL_STRING(entity, myObj->Entity);
    TEST_ASSERT_EQUAL_STRING(code, myObj->Code);
    TEST_ASSERT_EQUAL_INT(year, myObj->Year);
    TEST_ASSERT_EQUAL_INT(civilWars, myObj->CivilWars);
    TEST_ASSERT_EQUAL_INT(interStateWars, myObj->InterstateWars);
}

/* Test basic scenario where the object is instantiated and then destroyed*/
void test_CSVCreate_CSVDestroy()
{

    char entity[] = "SPQR";
    char code[] = "X";
    int year = 33;
    int civilWars = 20;
    int interStateWars = 100;

    CSVLine *myObj = CSVLine_Create(entity, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine_Destroy(myObj);

    TEST_ASSERT_EQUAL_HEX32((uintptr_t)myObj, (uintptr_t)NULL);
}

/* Test basic scenario where for computing max value amongst created CSV lines*/
void test_CSVCreate_CSVAnalyzer()
{

    char parsedLine[] = "Americas,,1800,231,0";
    char parsedLine2[] = "SPQR,FOO,0,2000,0";
    AnalysisResult analysis;

    LineSplitter(parsedLine, tokenBuffer);
    LineSplitter(parsedLine2, tokenBuffer2);

    char entity[strlen(tokenBuffer[0]) + 1];
    strcpy(entity, tokenBuffer[0]);
    char entity2[strlen(tokenBuffer2[0]) + 1];
    strcpy(entity2, tokenBuffer2[0]);

    char code[strlen(tokenBuffer[1]) + 1];
    strcpy(code, tokenBuffer[1]);
    char code2[strlen(tokenBuffer2[1]) + 1];
    strcpy(code2, tokenBuffer2[1]);

    int year = atoi(tokenBuffer[2]);
    int year2 = atoi(tokenBuffer2[2]);

    int civilWars = atoi(tokenBuffer[3]);
    int civilWars2 = atoi(tokenBuffer2[3]);

    int interStateWars = atoi(tokenBuffer[4]);
    int interStateWars2 = atoi(tokenBuffer2[4]);

    int expectedMaxNum = 2000;

    CSVLine *myObj0 = CSVLine_Create(entity, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine *myObj1 = CSVLine_Create(entity2, code2, year2, civilWars2, interStateWars2, constructorStatus);
    CSVLine *csvArr[] = {myObj0, myObj1};

    CSV_Analyzer(csvArr, 2, &analysis);

    int actualMaxNum = analysis.Max;

    TEST_ASSERT_EQUAL_INT(expectedMaxNum, actualMaxNum);
}

int main(void)
{

    UNITY_BEGIN();
    RUN_TEST(test_CSV_LineRead_LineSplit);
    RUN_TEST(test_CSV_LineSplit_LineCreate);
    RUN_TEST(test_CSVCreate_CSVDestroy);
    RUN_TEST(test_CSVCreate_CSVAnalyzer);

    return UNITY_END();
}