@ECHO OFF
powershell -ExecutionPolicy ByPass -NoProfile "%~dp0run.ps1" %*
echo %ERRORLEVEL%
