#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <sys/stat.h>
#include <limits.h>
#include "csv_parser.h"

int FileParser(char filePath[], char parserBuffer[][MAX_LINE_LENGTH])
{

    /* NOTE: this approach for folder
            checking may not be compatible
            on Windows.*/

    struct stat buffer;
    int status = stat(filePath, &buffer);

    if (status == -1)
        return FILE_OPEN_ERR_CODE;

    if (S_ISDIR(buffer.st_mode))
    {
        return FILE_IS_DIR_ERR_CODE;
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

    return OK_CODE;
}

int LineSplitter(char fileLine[], char tokenBuffer[MAX_TOKENS][MAX_LINE_LENGTH])
{
    char *start = fileLine;
    int i = 0;
    while (i < MAX_TOKENS)
    {
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

        if (*start == '\0')
            break;
    }
    for (; i < MAX_TOKENS; i++)
    {
        strcpy(tokenBuffer[i], "");
    }

    return LINE_SPLITTER_OK;
}

CSVLine *CSVLine_Create(char entity[], char code[], unsigned int year, unsigned int civilWars, unsigned int interStateWars, char constructorStatus[CSV_CREATE_BUFF_SIZE])
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

        csvLineObj->Code = malloc(strlen(code) + 1);
        if (csvLineObj->Code != NULL)
            strcpy(csvLineObj->Code, code);
        else
            strcpy(constructorStatus, CSV_CODE_ERROR);

        csvLineObj->Year = year;
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
        free(csvLineObj->Code);
        csvLineObj->Entity = NULL;
        csvLineObj->Code = NULL;
    }

    free(csvLineObj);
    csvLineObj = NULL;
}

int CSV_Analyzer(CSVLine *csvLines[], int numElems, AnalysisResult *analysis)
{
    if (numElems <= 0)
        return ANALYSIS_RESULT_EMPTY_WARNING;

    int maxCivilWars = INT_MIN;
    int minCivilWars = INT_MAX;
    float meanCivilWars = 0;
    float medianCivilWars = 0;

    for (int i = 0; i < numElems; i++)
    {
        meanCivilWars += csvLines[i]->CivilWars;

        if (csvLines[i]->CivilWars > maxCivilWars)
            maxCivilWars = csvLines[i]->CivilWars;

        if (csvLines[i]->CivilWars < minCivilWars)
            minCivilWars = csvLines[i]->CivilWars;
    }

    for (int i = 0; i < numElems - 1; i++)
    {
        for (int j = i + 1; j < numElems; j++)
        {
            int x = csvLines[i]->CivilWars;
            int y = csvLines[j]->CivilWars;

            if (x > y)
            {
                int temp = x;
                csvLines[i]->CivilWars = y;

                csvLines[j]->CivilWars = temp;
            }
        }
    }

    if (numElems % 2 == 1)
    {
        int index = numElems / 2;
        medianCivilWars = csvLines[index]->CivilWars;
    }
    else
    {
        int index0 = (numElems - 1) / 2;
        int index1 = numElems / 2;

        medianCivilWars = (csvLines[index0]->CivilWars + csvLines[index1]->CivilWars) / 2;
    }

    analysis->Max = maxCivilWars;
    analysis->Min = minCivilWars;
    analysis->Mean = meanCivilWars / numElems;
    analysis->Median = medianCivilWars;

    return ANALYSIS_RESULT_OK;
}