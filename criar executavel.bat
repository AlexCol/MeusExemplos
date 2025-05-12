:: Soltar esse bat na pasta principal do programa e executar, vai ser criada a pasta 'published' com um unico exe que pode ser usado para ativar o programa

@echo off
REM Define a versão do .NET a ser usada
set VERSAODOTNET=net8.0

REM Compila e publica o projeto .NET como um arquivo único (---------------------- old)
REM dotnet publish -f %VERSAODOTNET% -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

REM Publica o projeto .NET como um executável único, pronto para distribuição
REM 
REM -f %VERSAODOTNET%               : Define o framework alvo (ex: net6.0, net7.0). Usado aqui como variável.
REM -c Release                      : Usa a configuração de build "Release", otimizada para produção.
REM -r win-x64                      : Define o runtime de destino (aqui, Windows 64 bits).
REM --self-contained true           : Gera um executável que inclui o runtime do .NET (não precisa estar instalado na máquina).
REM /p:PublishSingleFile=true       : Junta todos os arquivos (DLLs, etc.) em um único executável.
REM /p:IncludeNativeLibrariesForSelfExtract=true : Inclui bibliotecas nativas que serão extraídas temporariamente em tempo de execução.
REM /p:EnableCompressionInSingleFile=true        : Comprime os recursos internos para reduzir o tamanho final do executável.
dotnet publish -f %VERSAODOTNET% -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:EnableCompressionInSingleFile=true

REM Verifica se a pasta de destino existe, se não, cria a pasta
IF NOT EXIST src\published (
    mkdir src\published
)

REM Move os arquivos publicados para a pasta de destino
move bin\Release\%VERSAODOTNET%\win-x64\publish\*.* src\published

REM Limpa a pasta publish da bin após mover os arquivos
rd /s /q bin\Release\%VERSAODOTNET