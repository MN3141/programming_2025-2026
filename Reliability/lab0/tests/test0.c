#include <stdbool.h>
#include "defs.h"
#include <stdio.h>
#include "unity.h"


void setUp(void) {
}

void tearDown(void) {
}

void test_die(void){
    TEST_ASSERT_EQUAL(true,false);
}

void test_reverse(void){

    char string[] = "abc";
    char string2[] = "a";
    char expectedString[] = "cba";

    TEST_ASSERT_EQUAL_STRING(expectedString,reverse(string));
    TEST_ASSERT_EQUAL_STRING("a",reverse(string2));
}

// not needed when using generate_test_runner.rb
int main(void) {
    UNITY_BEGIN();
    RUN_TEST(test_reverse);
    return UNITY_END();
}