@echo off


REM Check for available Sync and .net versions controller and view page
setlocal EnableDelayedExpansion
REM Set /p list = "Set application configuration as ".NETcore framework <space> Syncfusion assembly version <space> Controller <space> View <space> Options"
echo .NETcore framework @space@ Syncfusion assembly version @space@ Controller @space@ View @space@ Options
set /P list=Set application configuration in above format Eg., 1.1 16.1.0.37 Datepicker Forcontrol: 

set n=0

REM Define Syncfusion assembly version available in application
set sync_versions[0]=16.1.0.37
set sync_versions[1]=16.2.0.41
set ver_exist=false
for %%a in (%list%) do (
   set vector[!n!]=%%a
   set /A n+=1
)

( for /L %%i in (0,1,4) do (
	if %%i==0 set net_ver=!vector[%%i]!
	if %%i==1 set sync_ver=!vector[%%i]!
	if %%i==2 set controller=!vector[%%i]!
	if %%i==3 set view=!vector[%%i]!
	if %%i==4 set options=!vector[%%i]!
))

for %%v in (0,1,2) do (
		if !sync_versions[%%v]!==%sync_ver% ( set ver_exist=true)
	)
echo %ver_exist%
if (%ver_exist%==false) ( echo Syncfusion assembly version does not match the available versions
) else (
	echo .NET version = %net_ver% 
	echo Syncfusion version = %sync_ver%
	echo Controller = %controller%
	echo View = %view%
	echo Options = %options%
cd "src/Syncfusion_%sync_ver%"
REM To kill the process running in port 5050 (port assigned for proj 1)
if %sync_ver%==%sync_versions[0]% (
set "port=5000"
) else (
set "port=5001"
) 

for /F "tokens=5 delims= " %%P in ('netstat -a -n -o ^| findstr :!port!.*LISTENING') do TaskKill.exe /PID %%P /f

dotnet restore

dotnet watch run --framework netcoreapp%net_ver% [ %controller% %view% %options% ]

)
pause
