#include <stdio.h>
#include "defs.h"

char* reverse(char arr[]){

    int arrSize = sizeof(arr);
    for(int i=0; i < arrSize; i++){
        arr[i] = arr[arrSize-i];
    }
    return arr;
}