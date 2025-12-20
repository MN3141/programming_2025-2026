#include <stdio.h>
#include <pthread.h>

void *cntTask(void *arg)
{

    int *counter = (int *)arg;

    for (int i = 0; i < 100; i++)
        (*counter)++;

    void *returnValue = counter;
    printf("\nFoo ID:%lu",pthread_self());
    return returnValue;
}

int main()
{

    pthread_t threadHandle, mainThreadID;
    pthread_attr_t *threadAttribute = NULL;
    int x = 100;
    int *result;
    mainThreadID = pthread_self();
    pthread_create(&threadHandle, threadAttribute, cntTask, &x);
    pthread_join(threadHandle,(void*)&result); // wait for our thread to finish first
                                    //else, main thread will finish before the countin

    printf("\nMain thread done!");
    printf("\nCounting result:%d",*result);
    printf("\nThread IDS:");
    printf("\nMain thread:%lu",mainThreadID);
    printf("\nCnt thread ID:%lu",threadHandle);
    return 0;
}
