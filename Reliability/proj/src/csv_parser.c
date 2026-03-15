#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "csv_parser.h"

char **FileParser(char filePath[])
{

    FILE *fileHandle = fopen(filePath, "r");
    char **fileLines = malloc(MAX_FILE_SIZE * sizeof(char *));

    if (!fileHandle)
        perror(FILE_OPEN_ERROR);
    else
    {
        char readBuffer[MAX_LINE_LENGTH];
        int lineCounter = 0;
        while (fgets(readBuffer, MAX_LINE_LENGTH, fileHandle) != NULL)
        {
            int len = strlen(readBuffer);
            if (readBuffer[len - 1] == '\n')
                readBuffer[len - 1] = '\0';

            fileLines[lineCounter] = malloc(MAX_LINE_LENGTH * sizeof(char));
            strcpy(fileLines[lineCounter], readBuffer);

            lineCounter++;
        }
        fclose(fileHandle);
    }
    return fileLines;
}