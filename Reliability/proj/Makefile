include defs.mak

dbg :=
DBG_FLAG :=

ifneq ($(dbg),)
DBG_FLAG := -g
endif

all: $(app)| setup

setup:
	@mkdir -p $(build_dir) $(obj_dir)

info: $(intermediaries) $(asms) $(objs) $(app)| setup
	@gcc $(objs) -o $(build_dir)/$(proj).elf -Wl,-Map=$(build_dir)/$(proj).map
	@readelf -S $(app) > $(build_dir)/$(proj)_sections.txt
	@objdump -d $(app) > $(build_dir)/$(proj)_assembled.s

$(i_dir)%.i: $(src_dir)%.c
	@mkdir -p $(i_dir)
	@gcc $< $(cc_inc) -E -P -o $@

$(as_dir)%.s: $(src_dir)%.c
	@mkdir -p $(as_dir)
	@gcc $< $(cc_inc) -S -o $@

$(obj_dir)%.o: $(src_dir)%.c
	@mkdir -p $(obj_dir)
	@gcc $(DBG_FLAG) $< $(cc_inc) -c -o $@

$(build_dir)/$(proj).elf: $(objs)
	@gcc $^ -o $(build_dir)/$(proj).elf

test-unit:$(unit_tst_srcs) | setup
	@gcc $^ $(tst_inc) $(src_dir)/csv_parser.c $(DBG_FLAG) -g -o $(build_dir)/unit_tst_$(proj).elf

test-int:$(integration_tst_srcs) | setup
	@gcc $^ $(tst_inc) $(src_dir)/csv_parser.c $(DBG_FLAG) -g -o $(build_dir)/integration_tst_$(proj).elf

test-sys: $(system_tst_srcs) | setup
	@gcc $^ $(tst_inc) $(src_dir)/csv_parser.c $(DBG_FLAG) -g -o $(build_dir)/system_tst_$(proj).elf

help:
	@echo "Basic makefile script"
	@echo "Available build options:"
	@echo "all (or empty): builds the app"
	@echo "clean: cleans the built directory"
	@echo "info: builds the app with additional debugging info"
	@echo "test: builds unit tests"
	@echo "help: shows these messages"

clean:
	@rm -rf $(build_dir)

.PHONY: clean