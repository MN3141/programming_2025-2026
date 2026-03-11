#include <stdio.h>
#include <string.h>
#include "defs.h"

char *reverse(char arr[])
{

    int arrSize = strlen(arr);

    for (int i = 0; i < arrSize / 2; i++)
    {

        char temp = arr[i];
        arr[i] = arr[arrSize - i - 1];
        arr[arrSize - i - 1] = temp;
    }
    return arr;
}