#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <sys/stat.h>
#include "csv_parser.h"

char **FileParser(char filePath[])
{

    /* NOTE: this approach for folder
            checking may not be compatible
            on Windows.*/

    struct stat buffer;
    int status = stat(filePath, &buffer);

    if (status == -1)
    {
        char **errorMsg = malloc(sizeof(char *));
        errorMsg[0] = malloc(strlen(FILE_OPEN_ERROR) + 1);
        strcpy(errorMsg[0], FILE_OPEN_ERROR);

        return errorMsg;
    }

    if (S_ISDIR(buffer.st_mode))
    {
        char **errorMsg = malloc(sizeof(char *));
        errorMsg[0] = malloc(strlen(FILE_IS_FOLDER_ERROR) + 1);
        strcpy(errorMsg[0], FILE_IS_FOLDER_ERROR);

        return errorMsg;
    }

    FILE *fileHandle = fopen(filePath, "r");
    char **fileLines = malloc(MAX_FILE_SIZE * sizeof(char *));

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

    return fileLines;
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

    char **splitLines = malloc(fieldsNum * sizeof(char*));
    splitLines[0] = malloc((strlen(token) + 1) * sizeof(char));
    strcpy(splitLines[0],token);
    int counter = 1;

    while (token != NULL && counter < fieldsNum)
    {
        token = strtok(NULL, delim);
        if (token == NULL) break;
        splitLines[counter] = malloc((strlen(token) + 1) * sizeof(char));
        strcpy(splitLines[counter],token);

        counter++;
    }

    free(copyStr);
    return splitLines;
}