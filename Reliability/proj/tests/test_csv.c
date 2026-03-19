#include <stdbool.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include "csv_parser.h"
#include "unity.h"

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

    char expectedLine2[] = "Sub-Saharan Africa,,1801,0,0";

    char **readLines = FileParser(filePath);
    char *actualLine2 = readLines[2];

    TEST_ASSERT_EQUAL_STRING(expectedLine2, actualLine2);
}

/* Test for path that is a directory*/
void test_CSV_FileParser_Input_Is_Folder(void)
{
    char dirPath[] = "/home";
    char **parserOutput = FileParser(dirPath);

    TEST_ASSERT_EQUAL_STRING(FILE_IS_FOLDER_ERROR, parserOutput[0]);
}

/* Test for path that is an invalid file*/
void test_CSV_FileParser_Input_Path_Invalid(void)
{
    char wrongPath[] = "ULBS";
    char **parserOutput2 = FileParser(wrongPath);

    TEST_ASSERT_EQUAL_STRING(FILE_OPEN_ERROR, parserOutput2[0]);
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