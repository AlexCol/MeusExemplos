:: Soltar esse bat na pasta principal do programa e executar, vai ser criada a pasta 'published' com um unico exe que pode ser usado para ativar o programa

@echo off
REM Define a versão do .NET a ser usada
set VERSAODOTNET=net8.0

REM Compila e publica o projeto .NET como um arquivo único
dotnet publish -f %VERSAODOTNET% -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

REM Verifica se a pasta de destino existe, se não, cria a pasta
IF NOT EXIST src\published (
    mkdir src\published
)

REM Move os arquivos publicados para a pasta de destino
move bin\Release\%VERSAODOTNET%\win-x64\publish\*.* src\published

REM Limpa a pasta publish da bin após mover os arquivos
rd /s /q bin\Release\%VERSAODOTNET