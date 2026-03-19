#ifndef CSVPARSER
#define CSVPARSER

#define FILE_OPEN_ERROR "Cannot open file!"
#define FILE_IS_FOLDER_ERROR "Given path is folder!"
#define TOKEN_SPLIT_ERROR "Could not split line!"
#define MAX_LINE_LENGTH 100
#define MAX_FILE_SIZE 1300

char **FileParser(char filePath[]);
char **LineSplitter(char fileLine[], int fieldsNum);
#endif