#ifndef CSVPARSER
#define CSVPARSER

#define FILE_OPEN_ERROR "Cannot open file!"
#define MAX_LINE_LENGTH 30
#define MAX_FILE_SIZE 1300

char** FileParser(char filePath[]);
char* LineSplitter(char fileLine[]);
#endif