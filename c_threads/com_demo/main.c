    #include <stdio.h>
    #include <pthread.h>
    #include <stdlib.h>

    static int sharedCounter = 0;
    pthread_mutex_t lock; // for thread safety

    void *counterTask(void *arg)
    {
        for (int i = 0; i < 10; i++)
        {
            pthread_mutex_lock(&lock);
            sharedCounter++;
            pthread_mutex_unlock(&lock);
        }
        return NULL;
    }

    void *printTask(void *arg)
    {
        for (int i = 0; i < 10; i++)
        {
            pthread_mutex_lock(&lock);
            printf("\nCurrent counter value:%d", sharedCounter); //printing is slower than incrementing
            pthread_mutex_unlock(&lock);
        }

        return NULL;
    }

    int main(int argc, char **argv)
    {

        int enteredValue;
        int defaultValue = 10;

        pthread_t mainThreadHandle, cntThreadHandle;
        pthread_t printThreadHandle;
        pthread_attr_t *threadAttribute = NULL;

        pthread_mutex_init(&lock,NULL);

        if (argc > 1)
            enteredValue = atoi(argv[1]);
        else
            enteredValue = defaultValue;

        mainThreadHandle = pthread_self();
        printf("Entered value:%d", enteredValue);

        sharedCounter = enteredValue;

        pthread_create(&cntThreadHandle, threadAttribute, counterTask, NULL);
        pthread_create(&printThreadHandle, threadAttribute, printTask, NULL);

        pthread_join(cntThreadHandle, NULL);
        pthread_join(printThreadHandle, NULL);
        printf("\n Main thread %lu finished!", mainThreadHandle);

        pthread_mutex_destroy(&lock);
        return 0;
    }