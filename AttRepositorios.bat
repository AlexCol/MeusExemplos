@echo off
cd C:\Alexandre\C#

for /D %%G in (*) do (
    echo Atualizando repositorio: %%G
    cd "%%G"
    git pull
    cd ..
    echo  __________________________________________________________
    echo  __________________________________________________________
    echo  __________________________________________________________
)

echo Atualizacao completa!
pause
