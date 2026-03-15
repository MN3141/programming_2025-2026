proj = csv_analysis
src_dir = src/
inc_dir = inc/
cc_inc = -I$(inc_dir)

build_dir = build
obj_dir = $(build_dir)/objs/
as_dir = $(build_dir)/assembly/
i_dir = $(build_dir)/preprocessed/
app = $(build_dir)/$(proj).elf

srcs = $(wildcard $(src_dir)*.c)
intermediaries = $(patsubst $(src_dir)%.c,$(i_dir)%.i,$(srcs))
asms = $(patsubst $(src_dir)%.c,$(as_dir)%.s,$(srcs))
objs = $(patsubst $(src_dir)%.c,$(obj_dir)%.o,$(srcs))

# YOU NEED TO CONFIGURE test_framework_root
test_framework_root = ../../../Unity
tst_dir = tests
tst_srcs = $(test_framework_root)/src/unity.c $(tst_dir)/test1.c
tst_inc = $(cc_inc) -I$(test_framework_root)/src