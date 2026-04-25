@echo off
echo Starting Vaamonos...

start cmd /k "cd /d "%~dp0Final Project\ExcursionManager.Web" && npm run dev"

timeout /t 3 /nobreak >nul
start http://localhost:5173

echo Frontend running! Now press F5 in Visual Studio for the API.
pause