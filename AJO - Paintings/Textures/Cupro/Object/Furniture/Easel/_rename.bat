@echo off
setlocal enabledelayedexpansion
for %%f in (*_back.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-9!_north.png
	set newname=!var:~0,-9!_north.png
	ren %%f !newname!
)
for %%f in (*_front.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-10!_south.png
	set newname=!var:~0,-10!_south.png
	ren %%f !newname!
)
for %%f in (*_side.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-9!_east.png
	set newname=!var:~0,-9!_east.png
	ren %%f !newname!
)
for %%f in (*_backm.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-10!_northm.png
	set newname=!var:~0,-10!_northm.png
	ren %%f !newname!
)
for %%f in (*_frontm.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-10!_southm.png
	set newname=!var:~0,-11!_southm.png
	ren %%f !newname!
)
for %%f in (*_sidem.png) do (
	::echo %%f
	set var=%%f
	::echo !var:~0,-9!_eastm.png
	set newname=!var:~0,-10!_eastm.png
	ren %%f !newname!
)
::pause