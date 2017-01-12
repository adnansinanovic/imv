@echo off

:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
    IF "%PROCESSOR_ARCHITECTURE%" EQU "amd64" (
>nul 2>&1 "%SYSTEMROOT%\SysWOW64\cacls.exe" "%SYSTEMROOT%\SysWOW64\config\system"
) ELSE (
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
)

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params = %*:"=""
    echo UAC.ShellExecute "cmd.exe", "/c ""%~s0"" %params%", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    CD /D "%~dp0"


cls
@echo off
SET destination=%ProgramFiles%\IMV
SET serviceExe=Sinantrop.IMV.Uploader.Service
SET serviceName=IMV

echo Uninstalling... please wait

rem stop service if exists
sc query | find /I "%serviceName%" > nul	
    if errorlevel 0 sc stop %serviceName% > nul
	if not errorlevel 0  goto TERMINATE

rem wait few seconds
ping 127.0.0.1 -n 6 > nul 
echo ...wait for it...
	
rem delete service if exists
sc query | find /I "%serviceName%" > nul
	if errorlevel 0 sc delete %serviceName% > nul 
	if not errorlevel 0  goto TERMINATE

rem wait few seconds
ping 127.0.0.1 -n 6 > nul
echo ...wait a little bit longer...
	
rem delete destination dir if exists
if exist "%destination%" rmdir /s /q  "%destination%" > nul
	if not errorlevel 0 goto TERMINATE

if errorlevel 0 echo Uninstalling completed
goto the_end

:TERMINATE
echo Uninstalling not completed!!!

:THE_END

pause
@echo on