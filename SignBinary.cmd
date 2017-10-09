ECHO SignBinary.cmd

SETLOCAL

REM %1
REM %2 $(Configuration) - the build configuration
REM %3 $(TargetPath) - the path of the output file

SET CONFIGURATION=%2
SET TARGET_PATH=%3

IF "%2" == "Release" GOTO Sign
GOTO NoSign

:Sign
echo SignBinary.cmd: %CONFIGURATION% - Signing binary %TARGET_PATH%
if "%SIGNTOOL_DIR%" == "" GOTO SigntoolPath
"%SIGNTOOL_DIR%\signtool" sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a %TARGET_PATH%
GOTO DoneSigning

:NoSign
echo SignBinary.cmd: %CONFIGURATION% - Not signing  %TARGET_PATH%
GOTO DoneSigning

:SigntoolPath
echo The environment variable SIGNTOOL_DIR must point to the folder in which signtool.exe resides.
GOTO DoneSigning

:DoneSigning

:EOF
