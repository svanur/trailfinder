@echo on
setlocal

echo Starting TrailFinder development environment...

:: 1. Start Supabase
echo Starting Supabase...
cd ..\TrailFinder.DB
call supabase start

:: 2. Start Frontend
echo Starting Frontend...
start "TrailFinder Frontend" cmd /k "cd ..\TrailFinder.Web && npm install && npm run dev"

:: 3. Start Backend API
echo Starting Backend API...
start "TrailFinder API" cmd /k "cd ..\TrailFinder.Api && dotnet run --urls=http://localhost:5263"

echo All services have been started!
echo Frontend should be available at: http://localhost:3000
echo Backend API should be available at: http://localhost:5263/swagger/index.html
echo Supabase Studio should be available at: http://localhost:54323

:: Keep the main window open
pause