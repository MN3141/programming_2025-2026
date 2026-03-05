#include <stdbool.h>
#include "defs.h"
#include "unity.h"


void setUp(void) {
}

void tearDown(void) {
}

void test_die(void){
    TEST_ASSERT_EQUAL(true,false);
}

void test_reverse(void){
    char string[] = {'a','b','c'};
    char string2[] = {'a'};
    char reversed[] = {'c','b','a'};
    TEST_ASSERT_EQUAL_CHAR(reversed,reverse(string));
    TEST_ASSERT_EQUAL('a',reverse(string2));
}

// not needed when using generate_test_runner.rb
int main(void) {
    UNITY_BEGIN();
    RUN_TEST(test_die);
    return UNITY_END();
}