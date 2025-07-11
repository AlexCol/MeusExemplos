@echo off
set "root_path=%~dp0"
echo Diretório inicial "%root_path%"
set errorlevel=0

for /D %%G in ("%root_path%*") do (
    cd "%%G"
	if exist ".git" (
		echo Atualizando repositorio: %%G
		git pull || set errorlevel=1
	) else if exist ".svn" (
		echo Atualizando repositorio SVN: %%G
		svn update || set errorlevel=1
	)else if exist "update.bat" (
	    echo Atualizando "%%G\%~nx0" apartir de "%~f0"
	    copy "%~f0" "%%G\%~nx0"
		echo Executando arquivo update.bat
		start cmd /c call update.bat
	)
    cd "%root_path%"
)

if errorlevel 1 (
    echo Ocorreu um erro durante a atualizacao em "%root_path%"
	pause
) else (
    echo Atualizacao completa!
)