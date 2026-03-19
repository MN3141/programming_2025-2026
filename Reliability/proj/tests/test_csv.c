#include <stdbool.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include "csv_parser.h"
#include "unity.h"

char csvBuffer[MAX_FILE_SIZE][MAX_LINE_LENGTH];
char actualLine[MAX_LINE_LENGTH];
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
    FileParser(wrongPath,csvBuffer);
    strcpy(actualLine, csvBuffer[0]);

    TEST_ASSERT_EQUAL_STRING(FILE_OPEN_ERROR, actualLine);
}

/* Test basic tokenizing scenario*/
void test_CSV_LineSplitter_Tokenizer(void)
{

    char csvLine[] = "Americas,,1800,0,0";
    int num = 5;

    char **expectedLines = malloc(num * sizeof(char *));

    expectedLines[0] = malloc(strlen("Americas") + 1);
    strcpy(expectedLines[0], "Americas");

    expectedLines[1] = malloc(strlen("") + 1);
    strcpy(expectedLines[1], "");

    expectedLines[2] = malloc(strlen("") + 1);
    strcpy(expectedLines[2], "");

    expectedLines[3] = malloc(strlen("1800") + 1);
    strcpy(expectedLines[3], "1800");

    expectedLines[4] = malloc(strlen("0") + 1);
    strcpy(expectedLines[4], "0");

    char **splitLines = LineSplitter(csvLine, num);
    TEST_ASSERT_EQUAL_STRING_ARRAY(expectedLines, splitLines, num);
}

int main(void)
{
    UNITY_BEGIN();
    RUN_TEST(test_CSV_FileParser_LastLine);
    RUN_TEST(test_CSV_FileParser_Input_Is_Folder);
    RUN_TEST(test_CSV_FileParser_Input_Path_Invalid);
    RUN_TEST(test_CSV_LineSplitter_Tokenizer);
    return UNITY_END();
}