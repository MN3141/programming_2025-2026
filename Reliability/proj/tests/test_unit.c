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

char code[] = "FOO";
int year = 2003;
int civilWars = 100;
int interStateWars = 99;

int status_code;

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
    status_code = FileParser(dirPath, csvBuffer);

    TEST_ASSERT_EQUAL_INT(FILE_IS_DIR_ERR_CODE, status_code);
}

/* Test for path that is an invalid file*/
void test_CSV_FileParser_Input_Path_Invalid(void)
{
    char wrongPath[] = "ULBS";
    status_code = FileParser(wrongPath, csvBuffer);

    TEST_ASSERT_EQUAL_INT(FILE_OPEN_ERR_CODE, status_code);
}

// /* Test basic tokenizing scenario*/
void test_CSV_LineSplitter_Tokenizer(void)
{

    char parsedLine[] = "Americas,,1800,0,0";
    char *expectedTokens[] = {
        "Americas", "", "1800", "0", "0"};

    LineSplitter(parsedLine, tokenBuffer);

    for (int i = 0; i < 5; i++)
        TEST_ASSERT_EQUAL_STRING(expectedTokens[i], tokenBuffer[i]);
}

// /* Test tokenizing for empty values*/
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

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    char expectedStatus[] = CSV_OBJ_CREATED_OK;

    TEST_ASSERT_EQUAL_STRING(expectedStatus, constructorStatus);
}

/* Test if the constructor copies the address or the content of entity string*/
void test_CSVLine_Create_Entity_Copy(void)
{
    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    char *entityAddr = entityStr;
    char *constructorAddr = myObj->Entity;

    TEST_ASSERT_NOT_EQUAL((uintptr_t)entityStr, (uintptr_t)myObj->Entity);
}

/* Test if the memory allocated for entity is freed*/
void test_CSVLine_Destroy_Entity(void)
{
    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine_Destroy(myObj);
    char *entityAddr = myObj->Entity;

    TEST_ASSERT_EQUAL_HEX32((uintptr_t)myObj->Entity, (uintptr_t)NULL);
}

/* Determine the maximum number of civil wars for two CSV line objects*/
void test_CSV_Analyzer_Max()
{
    int expectedMaxNum = 9999;
    AnalysisResult analysis;

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, expectedMaxNum, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2};

    CSV_Analyzer(csvArr, 2, &analysis);
    int actualMaxNum = analysis.Max;

    TEST_ASSERT_EQUAL_INT(expectedMaxNum, actualMaxNum);
}

/* Determine the minimum number of civil wars for two CSV line objects*/
void test_CSV_Analyzer_Min()
{

    int expectedMinNum = 1;
    AnalysisResult analysis;

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, expectedMinNum, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2};

    CSV_Analyzer(csvArr, 2, &analysis);
    int actualMinNum = analysis.Min;

    TEST_ASSERT_EQUAL_INT(expectedMinNum, actualMinNum);
}

/* Determine the maximum number of civil wars for two CSV line objects*/
void test_CSV_Analyzer_Mean()
{
    int civilWars2 = 140;
    float expectedMean = (civilWars + civilWars2) / 2;
    AnalysisResult analysis;

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, civilWars2, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2};

    CSV_Analyzer(csvArr, 2, &analysis);
    int actualMeanNum = analysis.Mean;

    TEST_ASSERT_EQUAL_INT(expectedMean, actualMeanNum);
}

/* Determine the median value in the scenario of odd number of elements*/
void test_CSV_Analyzer_Median_Odd()
{

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, 5, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, 10, interStateWars, constructorStatus);
    CSVLine *myObj3 = CSVLine_Create(entityStr, code, year, 17, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2, myObj3};
    AnalysisResult analysis;
    float expectedMedianNum = 10;

    CSV_Analyzer(csvArr, 3, &analysis);
    float actualMedianNum = analysis.Median;

    TEST_ASSERT_EQUAL_INT(expectedMedianNum, actualMedianNum);
}

/* Determine the median value in the scenario of even number of elements*/
void test_CSV_Analyzer_Median_Even()
{

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, 5, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, 10, interStateWars, constructorStatus);
    CSVLine *myObj3 = CSVLine_Create(entityStr, code, year, 17, interStateWars, constructorStatus);
    CSVLine *myObj4 = CSVLine_Create(entityStr, code, year, 20, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2, myObj3, myObj4};
    AnalysisResult analysis;
    float expectedMedianNum = 13.5;

    CSV_Analyzer(csvArr, 4, &analysis);
    float actualMedianNum = analysis.Median;

    TEST_ASSERT_EQUAL_INT(expectedMedianNum, actualMedianNum);
}

/* Check the behaviour when given a null counter of elemnts
NOTE: We are not concerned here with the scenario where the
actual list has elements but the counter is null;
Garbage In -> Garbage Out*/
void test_CSV_Analyzer_Empty()
{

    AnalysisResult analysis;
    int expectedAnalysisStatus = ANALYSIS_RESULT_EMPTY_WARNING;

    CSVLine *myObj = CSVLine_Create(entityStr, code, year, civilWars, interStateWars, constructorStatus);
    CSVLine *myObj2 = CSVLine_Create(entityStr, code, year, 999, interStateWars, constructorStatus);
    CSVLine *csvArr[] = {myObj, myObj2};

    int actualAnalysisStatus = CSV_Analyzer(csvArr, 0, &analysis);
    TEST_ASSERT_EQUAL_INT(expectedAnalysisStatus, actualAnalysisStatus);
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
    RUN_TEST(test_CSV_Analyzer_Max);
    RUN_TEST(test_CSV_Analyzer_Empty);
    RUN_TEST(test_CSV_Analyzer_Min);
    RUN_TEST(test_CSV_Analyzer_Mean);
    RUN_TEST(test_CSV_Analyzer_Median_Odd);
    RUN_TEST(test_CSV_Analyzer_Median_Even);

    return UNITY_END();
}