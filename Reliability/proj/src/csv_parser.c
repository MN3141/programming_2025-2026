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
}

void LineSplitter(char fileLine[], char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH])
{
    char *start = fileLine;
    int i = 0;
    while (i < MAX_TOKENS)
    {
        while (*start == ',')
            start++;
        if (*start == '\0')
            break;
        char *end = start;
        while (*end && *end != ',')
            end++;
        if (*end == ',')
        {
            *end = '\0';
            strcpy(tokenBuffer[i], start);
            start = end + 1;
        }
        else
        {
            strcpy(tokenBuffer[i], start);
            start = end;
        }
        i++;
    }
    for (; i < MAX_TOKENS; i++)
    {
        strcpy(tokenBuffer[i], "");
    }
}

CSVLine *CSVLine_Create(char entity[], int code, unsigned int civilWars, unsigned int interStateWars, char constructorStatus[CSV_CREATE_BUFF_SIZE])
{

    CSVLine *csvLineObj = malloc(sizeof(CSVLine));

    if (csvLineObj != NULL)
    {
        strcpy(constructorStatus, CSV_OBJ_CREATED_OK);

        csvLineObj->Entity = malloc(strlen(entity) + 1);

        if (csvLineObj->Entity != NULL)
            strcpy(csvLineObj->Entity, entity);
        else
            strcpy(constructorStatus, CSV_ENTITY_ERROR);

        csvLineObj->Code = code;
        csvLineObj->CivilWars = civilWars;
        csvLineObj->InterstateWars = interStateWars;
    }

    else
        strcpy(constructorStatus, CSV_OBJ_CREATED_ERROR);

    return csvLineObj;
}

void CSVLine_Destroy(CSVLine *csvLineObj)
{

    if (csvLineObj != NULL)
    {
        free(csvLineObj->Entity);
        csvLineObj->Entity = NULL;
    }

    free(csvLineObj);
    csvLineObj = NULL;
}