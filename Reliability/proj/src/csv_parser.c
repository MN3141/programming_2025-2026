#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "csv_parser.h"

char **FileParser(char filePath[])
{

    FILE *fileHandle = fopen(filePath, "r");
    char **fileLines = malloc(MAX_FILE_SIZE * MAX_LINE_LENGTH);

    if (!fileHandle)
        perror(FILE_OPEN_ERROR);
    else
    {
        char readBuffer[MAX_LINE_LENGTH];
        int lineCounter = 0;
        while (!feof(fileHandle))
        {
            if (fgets(readBuffer, MAX_LINE_LENGTH, fileHandle) == NULL)
                break;
            fileLines[lineCounter] = malloc(MAX_LINE_LENGTH);
            strcpy(*(fileLines + lineCounter), readBuffer);
        }
        fclose(fileHandle);
    }
    return fileLines;
}