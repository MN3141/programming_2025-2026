include defs.mak

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
	@gcc $< $(cc_inc) -c -o $@

$(build_dir)/$(proj).elf: $(objs)
	@gcc $^ -o $(build_dir)/$(proj).elf

test:$(tst_srcs)
	@gcc $^ $(tst_inc) -o $(build_dir)/tst_$(proj).elf

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