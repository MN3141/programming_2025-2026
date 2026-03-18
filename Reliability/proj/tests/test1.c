#include <stdbool.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include "csv_parser.h"
#include "unity.h"


void setUp(void) {
}

void tearDown(void) {
}

void test_die(void){
    TEST_ASSERT_EQUAL(true,false);
}

void test_file_read(void){

    /* Test if the file if the last CSV line is parsed*/
    char csvFile[] = "/tests/test.csv";
    char cwd[100];
    int pathSize = 0;

    getcwd(cwd,100);
    pathSize = strlen(csvFile) + strlen(cwd) + 1; /*for the null character*/

    char filePath[pathSize];
    strcpy(filePath,cwd);
    strcat(filePath,csvFile);

    char expectedLine2[] = "Sub-Saharan Africa,,1801,0,0";

    char **readLines = FileParser(filePath);
    char *actualLine2 = readLines[2];

    TEST_ASSERT_EQUAL_STRING(expectedLine2,actualLine2);

    /* Test what happens if the path is a folder*/

    char dirPath[] = "/home";
    char **parserOutput = FileParser(dirPath);

    TEST_ASSERT_EQUAL_STRING(FILE_IS_FOLDER_ERROR,parserOutput[0]);

    /* Test what happens if the given path does not exist*/

    char wrongPath[] = "ULBS";
    char **parserOutput2 = FileParser(wrongPath);

    TEST_ASSERT_EQUAL_STRING(FILE_OPEN_ERROR,parserOutput2[0]);
}

// not needed when using generate_test_runner.rb
int main(void) {
    UNITY_BEGIN();
    RUN_TEST(test_file_read);
    return UNITY_END();
}