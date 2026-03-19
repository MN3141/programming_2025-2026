#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <sys/stat.h>
#include "csv_parser.h"

void FileParser(char filePath[], char parserBuffer[][MAX_LINE_LENGTH])
{

    /* NOTE: this approach for folder
            checking may not be compatible
            on Windows.*/

    struct stat buffer;
    int status = stat(filePath, &buffer);

    if (status == -1)
    {
        strcpy(parserBuffer[0], FILE_OPEN_ERROR);
        return;
    }

    if (S_ISDIR(buffer.st_mode))
    {
        strcpy(parserBuffer[0], FILE_IS_FOLDER_ERROR);
        return;
    }

    FILE *fileHandle = fopen(filePath, "r");

    char readBuffer[MAX_LINE_LENGTH];
    int lineCounter = 0;
    while (fgets(readBuffer, MAX_LINE_LENGTH, fileHandle) != NULL)
    {
        int len = strlen(readBuffer);
        if (readBuffer[len - 1] == '\n')
            readBuffer[len - 1] = '\0';
        strcpy(parserBuffer[lineCounter], readBuffer);

        lineCounter++;
    }
    fclose(fileHandle);

    return;
}

char **LineSplitter(char fileLine[], int fieldsNum)
{

    char *copyStr = malloc((strlen(fileLine) + 1) * sizeof(char));
    strcpy(copyStr, fileLine);
    char delim[] = ",";
    char *token = strtok(copyStr, delim);

    if (token == NULL)
    {
        char **errorMsg = malloc(sizeof(char *));
        errorMsg[0] = malloc(strlen(TOKEN_SPLIT_ERROR) + 1);
        strcpy(errorMsg[0], TOKEN_SPLIT_ERROR);

        return errorMsg;
    }

    char **splitLines = malloc(fieldsNum * sizeof(char *));
    splitLines[0] = malloc((strlen(token) + 1) * sizeof(char));
    strcpy(splitLines[0], token);
    int counter = 1;

    while (token != NULL && counter < fieldsNum)
    {
        token = strtok(NULL, delim);
        if (token == NULL)
            break;
        splitLines[counter] = malloc((strlen(token) + 1) * sizeof(char));
        strcpy(splitLines[counter], token);

        counter++;
    }

    free(copyStr);
    return splitLines;
}