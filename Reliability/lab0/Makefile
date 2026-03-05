SRC_DIR = src/
INC_DIR = inc/

CC_SRCS = $(wildcard $(SRC_DIR)/*.c)
CC_INC = -I$(INC_DIR)
CC_FLAGS =
CC  = gcc

# YOU NEED TO CONFIGURE UNITY_ROOT
UNITY_ROOT = ../../../Unity
TST_DIR = tests
TST_SRCS = $(UNITY_ROOT)/src/unity.c $(TST_DIR)/test0.c
TST_INC = $(CC_INC) -I$(UNITY_ROOT)/src

proj = foo
out_dir = build

all:
	@mkdir -p $(out_dir)
#	@$(CC) $(CC_SRCS) $(CC_INC) $(CC_FLAGS) -S -o $(out_dir)/$(proj).s
#	@$(CC) $(CC_SRCS) $(CC_INC) $(CC_FLAGS) -S
	@$(CC) $(CC_SRCS) $(CC_INC) $(CC_FLAGS) -o $(out_dir)/$(proj).elf

test:
	@mkdir -p $(out_dir)
	@$(CC) $(TST_SRCS) $(SRC_DIR)/lab0.c $(TST_INC) $(CC_FLAGS) -o $(out_dir)/$(proj)_tst0.elf

clean:
	@rm -rf $(out_dir)