ADB_CLI := $(ANDROID_HOME)/platform-tools/adb

all:
	@gradlew assembleDebug
	@$(ADB_CLI) install -r app/build/outputs/apk/debug/app-debug.apk
	@$(ADB_CLI) shell am start -n com.example.myapplication/.MainActivity

clean:
	@gradlew clean

info:
	@$(ADB_CLI) devices