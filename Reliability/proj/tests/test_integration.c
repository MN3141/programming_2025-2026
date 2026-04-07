#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include "csv_parser.h"
#include "unity.h"

char csvBuffer[MAX_FILE_SIZE][MAX_LINE_LENGTH];
char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH];
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
void test_CSVCreate_CSVDestroy(){

    char entity[] = "SPQR";
    char code[] = "X";
    int year = 33;
    int civilWars = 20;
    int interStateWars = 100;

    CSVLine *myObj = CSVLine_Create(entity, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine_Destroy(myObj);

    TEST_ASSERT_EQUAL_HEX32((uintptr_t)myObj, (uintptr_t)NULL);
}
int main(void)
{

    UNITY_BEGIN();
    RUN_TEST(test_CSV_LineRead_LineSplit);
    RUN_TEST(test_CSV_LineSplit_LineCreate);
    RUN_TEST(test_CSVCreate_CSVDestroy);

    return UNITY_END();
}