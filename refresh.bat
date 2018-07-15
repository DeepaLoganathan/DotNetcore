@echo off


REM Check for available Sync and .net versions controller and view page
setlocal EnableDelayedExpansion
REM Set /p list = "Set application configuration as ".NETcore framework <space> Syncfusion assembly version <space> Controller <space> View <space> Options"
set n=0
REM Define Syncfusion assembly version available in application
set sync_versions[0]=16.1.0.37
set sync_versions[1]=16.2.0.41
set ver_exist=false
	set net_ver= %1
	set sync_ver=%2
	set controller=%3
	set view=%4
	REM if %%i==4 set options=!vector[%%i]!

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
cd "E:/_work/Dotnetcore/Dotnet_finalsample/src/Syncfusion_%sync_ver%"
REM To kill the process running in port 5050 (port assigned for proj 1)
if %sync_ver%==%sync_versions[0]% (
set "port=5000"
) else (
set "port=5001"
) 

for /F "tokens=5 delims= " %%P in ('netstat -a -n -o ^| findstr :!port!.*LISTENING') do TaskKill.exe /PID %%P /f

dotnet watch run --framework netcoreapp%net_ver% [ %controller% %view% %options% ]

)
pause
