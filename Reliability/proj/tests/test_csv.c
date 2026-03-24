#include <stdbool.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <stdint.h>
#include "csv_parser.h"
#include "unity.h"

char csvBuffer[MAX_FILE_SIZE][MAX_LINE_LENGTH];
char actualLine[MAX_LINE_LENGTH];
char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH];
char constructorStatus[CSV_CREATE_BUFF_SIZE];
char entityStr[] = "Rhomaioi";

int code = 10;
int civilWars = 100;
int interStateWars = 99;

void setUp(void)
{
}

void tearDown(void)
{
}

void test_die(void)
{
    TEST_ASSERT_EQUAL(true, false);
}

/* Test if the file if the last CSV line is parsed*/
void test_CSV_FileParser_LastLine(void)
{
    char csvFile[] = "/tests/test.csv";
    char cwd[100];
    int pathSize = 0;

    getcwd(cwd, 100);
    pathSize = strlen(csvFile) + strlen(cwd) + 1; /*for the null character*/

    char filePath[pathSize];
    strcpy(filePath, cwd);
    strcat(filePath, csvFile);

    char expectedLine[] = "Sub-Saharan Africa,,1801,0,0";

    FileParser(filePath, csvBuffer);
    strcpy(actualLine, csvBuffer[2]);

    TEST_ASSERT_EQUAL_STRING(expectedLine, actualLine);
}

/* Test for path that is a directory*/
void test_CSV_FileParser_Input_Is_Folder(void)
{
    char dirPath[] = "/home";
    FileParser(dirPath, csvBuffer);
    strcpy(actualLine, csvBuffer[0]);

    TEST_ASSERT_EQUAL_STRING(FILE_IS_FOLDER_ERROR, actualLine);
}

/* Test for path that is an invalid file*/
void test_CSV_FileParser_Input_Path_Invalid(void)
{
    char wrongPath[] = "ULBS";
    FileParser(wrongPath, csvBuffer);
    strcpy(actualLine, csvBuffer[0]);

    TEST_ASSERT_EQUAL_STRING(FILE_OPEN_ERROR, actualLine);
}

/* Test basic tokenizing scenario*/
void test_CSV_LineSplitter_Tokenizer(void)
{

    char parsedLine[] = "Americas,,1800,0,0";
    char *expectedTokens[] = {
        "Americas", "1800", "0", "0", ""};

    LineSplitter(parsedLine, tokenBuffer);

    for (int i = 0; i < 5; i++)
        TEST_ASSERT_EQUAL_STRING(expectedTokens[i], tokenBuffer[i]);
}

/* Test tokenizing for empty values*/
void test_CSV_LineSplitter_EmptyValues(void)
{
    char parsedLine[] = ",,,,";
    char *expectedTokens[] = {
        "", "", "", "", ""};

    LineSplitter(parsedLine, tokenBuffer);

    for (int i = 0; i < 5; i++)
        TEST_ASSERT_EQUAL_STRING(expectedTokens[i], tokenBuffer[i]);
}

/* Test constructor for a basic scenario*/
void test_CSVLine_Create_Normal(void)
{

    CSVLine *myObj = CSVLine_Create(entityStr, code, civilWars, interStateWars, constructorStatus);
    char expectedStatus[] = CSV_OBJ_CREATED_OK;

    TEST_ASSERT_EQUAL_STRING(expectedStatus, constructorStatus);
}

/* Test if the constructor copies the address or the content of entity string*/
void test_CSVLine_Create_Entity_Copy(void)
{
    CSVLine *myObj = CSVLine_Create(entityStr, code, civilWars, interStateWars, constructorStatus);
    char *entityAddr = entityStr;
    char *constructorAddr = myObj->Entity;

    TEST_ASSERT_NOT_EQUAL((uintptr_t)entityStr, (uintptr_t)myObj->Entity);
}

/* Test if the memory allocated for entity if freed*/
void test_CSVLine_Destroy_Entity(void)
{

    CSVLine *myObj = CSVLine_Create(entityStr, code, civilWars, interStateWars, constructorStatus);
    CSVLine_Destroy(myObj);
    char *entityAddr = myObj->Entity;

    TEST_ASSERT_EQUAL_PTR(entityAddr, NULL);
}
int main(void)
{
    UNITY_BEGIN();
    RUN_TEST(test_CSV_FileParser_LastLine);
    RUN_TEST(test_CSV_FileParser_Input_Is_Folder);
    RUN_TEST(test_CSV_FileParser_Input_Path_Invalid);
    RUN_TEST(test_CSV_LineSplitter_Tokenizer);
    RUN_TEST(test_CSV_LineSplitter_EmptyValues);
    RUN_TEST(test_CSVLine_Create_Normal);
    RUN_TEST(test_CSVLine_Create_Entity_Copy);
    RUN_TEST(test_CSVLine_Destroy_Entity);

    return UNITY_END();
}