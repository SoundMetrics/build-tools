ECHO SignBinary.cmd

IF NOT "%SIGNBINARY_SKIP%" == "" GOTO SkipSign

SETLOCAL

REM %1
REM %2 $(Configuration) - the build configuration
REM %3 $(TargetPath) - the path of the output file

SET IGNORE1=%1

SHIFT
SET CONFIGURATION=%1
ECHO CONFIGURATION=[%CONFIGURATION%]

SHIFT
SET TARGET_PATH=%1
ECHO TARGET_PATH=[%TARGET_PATH%]

IF "%CONFIGURATION%" == "Release" GOTO Sign

:AlternateSignedConfig
SHIFT
ECHO AlternateSignedConfig=[%1]
IF "%1" == "" GOTO NoSign
IF "%1" == "%CONFIGURATION%" GOTO Sign
GOTO AlternateSignedConfig

:Sign
echo SignBinary.cmd: %CONFIGURATION% - Signing binary %TARGET_PATH%
if "%SIGNTOOL_DIR%" == "" GOTO SigntoolPath
"%SIGNTOOL_DIR%\signtool" sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a %TARGET_PATH%
GOTO DoneSigning

:SkipSign
echo SignBinary.cmd: SIGNBINARY_SKIP is defined; signing skipped.
GOTO DoneSigning

:NoSign
echo SignBinary.cmd: %CONFIGURATION% - Not signing  %TARGET_PATH%
GOTO DoneSigning

:SigntoolPath
echo The environment variable SIGNTOOL_DIR must point to the folder in which signtool.exe resides.
GOTO DoneSigning

:DoneSigning

:EOF
