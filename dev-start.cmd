@echo off
echo Running app server...
start /b "app server" /d scripts run-app.cmd
echo Running web server...
start /b "web server" /d scripts run-web.cmd