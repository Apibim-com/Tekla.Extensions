@echo off
setlocal EnableExtensions EnableDelayedExpansion

echo ========================================
echo Tekla.Extension Build / Sign / Pack
echo ========================================
echo.

REM ===============================
REM CONFIG
REM ===============================
set CERT_NAME=Apibim SpA
set TIMESTAMP_URL=http://timestamp.sectigo.com
set SIGNTOOL=C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe
REM 2020
set VERSIONS= 2021 2022 2023 2024 2025
set SIGN=0
set PUBLISH=0

REM ===============================
REM ARGUMENTS
REM ===============================
:parse_args
if "%~1"=="" goto args_done

if /I "%~1"=="sign" set SIGN=1
if /I "%~1"=="publish" set PUBLISH=1

shift
goto parse_args

:args_done

echo Sign: %SIGN%
echo Publish: %PUBLISH%
echo.

REM ===============================
REM BUILD LOOP
REM ===============================
for %%v in (%VERSIONS%) do (

    echo ----------------------------------------
    echo Building Tekla %%v
    echo ----------------------------------------

    msbuild Tekla.Extension.sln ^
        /p:Configuration=%%v ^
        /p:Platform=x64 ^
        /t:Rebuild ^
        /v:minimal /nologo

    if errorlevel 1 (
        echo ERROR: Build failed for %%v
        exit /b 1
    )

    REM ===============================
    REM SIGN (ONLY IF REQUESTED)
    REM ===============================
    if "%SIGN%"=="1" (

        set "DLL_PATH=Tekla.Extension\bin\%%v\x64\Tekla.Extension.dll"

        if not exist "!DLL_PATH!" (
            echo ERROR: Tekla.Extension.dll not found for %%v
            exit /b 1
        )

        echo Signing Tekla.Extension.dll for Tekla %%v

        "%SIGNTOOL%" sign ^
            /n "%CERT_NAME%" ^
            /tr "%TIMESTAMP_URL%" ^
            /td SHA256 ^
            /fd SHA256 ^
            /v ^
            "!DLL_PATH!"

        if errorlevel 1 (
            echo ERROR: Signing failed for %%v
            exit /b 1
        )
    )

    REM ===============================
    REM PACK
    REM ===============================
    echo Packing NuGet for Tekla %%v

    Tekla.Extension\nuget.exe pack ^
        Tekla.Extension\Tekla.Extension.csproj ^
        -Properties Configuration=%%v;Platform=x64 ^
        -OutputDirectory Tekla.Extension\bin\%%v\x64 ^
        -Verbosity quiet

    if errorlevel 1 (
        echo ERROR: Pack failed for %%v
        exit /b 1
    )

    echo SUCCESS: Tekla %%v
    echo.
)

REM ===============================
REM DONE
REM ===============================
echo ========================================
echo BUILD COMPLETE
echo ========================================
echo.

REM ===============================
REM PUBLISH
REM ===============================
if "%PUBLISH%"=="1" (

    echo ========================================
    echo PUBLISHING TO NUGET
    echo ========================================
    echo.

    for %%v in (%VERSIONS%) do (
        for %%f in (Tekla.Extension\bin\%%v\x64\*.nupkg) do (
            Tekla.Extension\nuget.exe push "%%f" -Source https://api.nuget.org/v3/index.json
            if errorlevel 1 (
                echo ERROR: Publish failed
                exit /b 1
            )
        )
    )
)

echo Done.
pause
