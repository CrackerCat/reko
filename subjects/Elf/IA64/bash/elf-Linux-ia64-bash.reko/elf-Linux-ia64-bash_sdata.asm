;;; Segment .sdata (600000000000A160)
_IO_stdin_used		; 600000000000A160
	db	0x01, 0x00, 0x02, 0x00
600000000000A164             00 00 00 00 0C D6 FF FF FF FF FF FF     ............
600000000000A170 60 26 00 00 00 00 00 60 60 26 00 00 00 00 00 60 `&.....``&.....`
default_buffered_input		; 600000000000A180
	db	0xFF, 0xFF, 0xFF, 0xFF
have_devfd		; 600000000000A184
	db	0x01, 0x00, 0x00, 0x00
current_command_number		; 600000000000A188
	db	0x01, 0x00, 0x00, 0x00
600000000000A18C                                     00 00 00 00             ....
600000000000A190 90 81 13 00 00 00 00 40 A0 36 00 00 00 00 00 60 .......@.6.....`
600000000000A1A0 7C A3 13 00 00 00 00 40 A0 36 00 00 00 00 00 60 |......@.6.....`
eof_encountered_limit		; 600000000000A1B0
	db	0x0A, 0x00, 0x00, 0x00
extended_quote		; 600000000000A1B4
	db	0x01, 0x00, 0x00, 0x00
promptvars		; 600000000000A1B8
	db	0x01, 0x00, 0x00, 0x00
600000000000A1BC                                     00 00 00 00             ....
secondary_prompt		; 600000000000A1C0
	dq	0x4000000000138188
primary_prompt		; 600000000000A1C8
	dq	0x4000000000138178
600000000000A1D0 FF FF FF FF 00 00 00 00 49 B2 00 00 00 00 00 60 ........I......`
bash_getcwd_errstr		; 600000000000A1E0
	dq	0x400000000013A448
xtrace_fd		; 600000000000A1E8
	db	0xFF, 0xFF, 0xFF, 0xFF
600000000000A1EC                                     04 00 00 00             ....
600000000000A1F0 68 AF 13 00 00 00 00 40 38 38 00 00 00 00 00 60 h......@88.....`
600000000000A200 38 38 00 00 00 00 00 60 38 38 00 00 00 00 00 60 88.....`88.....`
array_needs_making		; 600000000000A210
	db	0x01, 0x00, 0x00, 0x00
600000000000A214             00 00 00 00 01 00 00 00 00 00 00 00     ............
600000000000A220 00 00 00 00 FF FF FF FF                         ........        
the_current_maintainer		; 600000000000A228
	dq	0x400000000013B6A0
600000000000A230 70 28 00 00 00 00 00 60 70 28 00 00 00 00 00 60 p(.....`p(.....`
600000000000A240 68 28 00 00 00 00 00 60 99 3A 00 00 00 00 00 60 h(.....`.:.....`
600000000000A250 70 28 00 00 00 00 00 60                         p(.....`        
brace_expansion		; 600000000000A258
	db	0x01, 0x00, 0x00, 0x00
interactive_comments		; 600000000000A25C
	db	0x01, 0x00, 0x00, 0x00
history_expansion		; 600000000000A260
	db	0x01, 0x00, 0x00, 0x00
hashing_enabled		; 600000000000A264
	db	0x01, 0x00, 0x00, 0x00
job_control		; 600000000000A268
	db	0x01, 0x00, 0x00, 0x00
last_asynchronous_pid		; 600000000000A26C
	db	0xFF, 0xFF, 0xFF, 0xFF
last_made_pid		; 600000000000A270
	db	0xFF, 0xFF, 0xFF, 0xFF
pgrp_pipe		; 600000000000A274
	dq	0xFFFFFFFFFFFFFFFF
original_pgrp		; 600000000000A27C
	db	0xFF, 0xFF, 0xFF, 0xFF
terminal_pgrp		; 600000000000A280
	db	0xFF, 0xFF, 0xFF, 0xFF
shell_pgrp		; 600000000000A284
	db	0xFF, 0xFF, 0xFF, 0xFF
shell_tty		; 600000000000A288
	db	0xFF, 0xFF, 0xFF, 0xFF
600000000000A28C                                     FF FF FF FF             ....
600000000000A290 FF FF FF FF 08 00 00 00                         ........        
wait_for_background_pids_GOT		; 600000000000A298
	dq	0x0000000000000000
600000000000A2A0 18 E7 00 00 00 00 00 60 18 E7 00 00 00 00 00 60 .......`.......`
600000000000A2B0 18 E7 00 00 00 00 00 60 18 E7 00 00 00 00 00 60 .......`.......`
current_command_subst_pid		; 600000000000A2C0
	db	0xFF, 0xFF, 0xFF, 0xFF
last_command_subst_pid		; 600000000000A2C4
	db	0xFF, 0xFF, 0xFF, 0xFF
600000000000A2C8                         9C EC 00 00 00 00 00 60         .......`
600000000000A2D0 80 EA 00 00 00 00 00 60 C4 D6 00 00 00 00 00 60 .......`.......`
600000000000A2E0 78 E8 00 00 00 00 00 60 C4 D6 00 00 00 00 00 60 x......`.......`
600000000000A2F0 80 EA 00 00 00 00 00 60 78 E8 00 00 00 00 00 60 .......`x......`
600000000000A300 C4 D6 00 00 00 00 00 60 9C EC 00 00 00 00 00 60 .......`.......`
600000000000A310 80 EA 00 00 00 00 00 60                         .......`        
shell_compatibility_level		; 600000000000A318
	db	0x2A, 0x00, 0x00, 0x00
600000000000A31C                                     00 00 00 00             ....
bash_license		; 600000000000A320
	dq	0x400000000013D030
bash_copyright		; 600000000000A328
	dq	0x400000000013CFF8
sccs_version		; 600000000000A330
	dq	0x400000000013D100
release_status		; 600000000000A338
	dq	0x400000000013CFD0
build_version		; 600000000000A340
	db	0x01, 0x00, 0x00, 0x00
patch_level		; 600000000000A344
	db	0x2D, 0x00, 0x00, 0x00
dist_version		; 600000000000A348
	dq	0x400000000013CFC8
bash_badsub_errmsg		; 600000000000A350
	dq	0x400000000013D190
command_oriented_history		; 600000000000A358
	db	0x01, 0x00, 0x00, 0x00
enable_history_list		; 600000000000A35C
	db	0x01, 0x00, 0x00, 0x00
remember_on_history		; 600000000000A360
	db	0x01, 0x00, 0x00, 0x00
600000000000A364             00 00 00 00 E1 D2 13 00 00 00 00 40     ...........@
force_fignore		; 600000000000A370
	db	0x01, 0x00, 0x00, 0x00
perform_hostname_completion		; 600000000000A374
	db	0x01, 0x00, 0x00, 0x00
600000000000A378                         FF FF FF FF                     ....    
prog_completion_enabled		; 600000000000A37C
	db	0x01, 0x00, 0x00, 0x00
sh_syntabsiz		; 600000000000A380
	db	0x00, 0x01, 0x00, 0x00
600000000000A384             00 00 00 00                             ....        
num_shell_builtins		; 600000000000A388
	db	0x4C, 0x00, 0x00, 0x00
600000000000A38C                                     00 00 00 00             ....
shell_builtins		; 600000000000A390
	dq	0x6000000000004700
600000000000A398                         18 E7 00 00 00 00 00 60         .......`
600000000000A3A0 90 E3 00 00 00 00 00 60 90 E3 00 00 00 00 00 60 .......`.......`
600000000000A3B0 F8 29 00 00 00 00 00 60 D8 29 00 00 00 00 00 60 .).....`.).....`
600000000000A3C0 F8 29 00 00 00 00 00 60 F8 29 00 00 00 00 00 60 .).....`.).....`
600000000000A3D0 D8 29 00 00 00 00 00 60 F8 29 00 00 00 00 00 60 .).....`.).....`
600000000000A3E0 20 E7 00 00 00 00 00 60 18 E7 00 00 00 00 00 60  ......`.......`
source_searches_cwd		; 600000000000A3F0
	db	0x01, 0x00, 0x00, 0x00
source_uses_path		; 600000000000A3F4
	db	0x01, 0x00, 0x00, 0x00
600000000000A3F8                         73 DE 00 00 00 00 00 60         s......`
600000000000A400 68 E8 14 00 00 00 00 40 48 2E 00 00 00 00 00 60 h......@H......`
600000000000A410 38 2E 00 00 00 00 00 60 48 2E 00 00 00 00 00 60 8......`H......`
600000000000A420 38 2E 00 00 00 00 00 60 48 2E 00 00 00 00 00 60 8......`H......`
600000000000A430 38 2E 00 00 00 00 00 60 FF FF FF FF             8......`....    
sh_optopt		; 600000000000A43C
	db	0x3F, 0x00, 0x00, 0x00
sh_opterr		; 600000000000A440
	db	0x01, 0x00, 0x00, 0x00
600000000000A444             00 00 00 00 2D 00 00 00 00 00 00 00     ....-.......
600000000000A450 28 F9 14 00 00 00 00 40 98 F9 14 00 00 00 00 40 (......@.......@
noglob_dot_filenames		; 600000000000A460
	db	0x01, 0x00, 0x00, 0x00
600000000000A464             00 00 00 00 20 06 15 00 00 00 00 40     .... ......@
600000000000A470 20 00 00 00 00 00 00 00 20 00 00 00 20 00 00 00  ....... ... ...
600000000000A480 00 00 00 00 20 00 00 00 00 00 00 00 00 00 00 00 .... ...........
600000000000A490 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF ................
600000000000A4A0 01 00 00 00 00 00 00 00 FF FF FF FF             ............    
