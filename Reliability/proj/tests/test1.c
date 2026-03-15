#include <stdbool.h>
#include <stdio.h>
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
    char filePath[] ="tests/test.csv";

    char expectedLine0[] = "North Africa and the Middle East,,2011,1,0";
    char expectedLine1[] = "Sub-Saharan Africa,,1800,0,0";
    char expectedLine2[] = "Sub-Saharan Africa,,1801,0,0";

    char **readLines = FileParser(filePath);
    char *actualLine0 = readLines[0];

    TEST_ASSERT_EQUAL_STRING(expectedLine0,*actualLine0);
}

// not needed when using generate_test_runner.rb
int main(void) {
    UNITY_BEGIN();
    RUN_TEST(test_file_read);
    return UNITY_END();
}