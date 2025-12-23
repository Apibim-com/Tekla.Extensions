@echo off
REM BuildAndPublish.bat - Build, sign, pack, and publish all Tekla.Extension versions
REM Usage: BuildAndPublish.bat [publish]
REM   BuildAndPublish.bat          - Build and pack only (no publish)
REM   BuildAndPublish.bat publish  - Build, pack, AND publish to NuGet

setlocal enabledelayedexpansion

echo ========================================
echo Tekla.Extension Build and Publish Script
echo ========================================
echo.

REM Check if publishing is requested
set PUBLISH=0
if /i "%1"=="publish" (
    set PUBLISH=1
    echo MODE: Build + Pack + Publish to NuGet
) else (
    echo MODE: Build + Pack only (no publish)
    echo TIP: Run "BuildAndPublish.bat publish" to publish to NuGet
)
echo.

REM Versions to build
set VERSIONS=2020 2021 2022 2023 2024 2025

REM Clean old packages
echo Cleaning old packages...
for %%v in (%VERSIONS%) do (
    if exist "Tekla.Extension\bin\%%v\x64\*.nupkg" (
        del /Q "Tekla.Extension\bin\%%v\x64\*.nupkg"
    )
)
echo.

REM Build and pack all versions
echo ========================================
echo STEP 1: Building and Packing All Versions
echo ========================================
echo.

for %%v in (%VERSIONS%) do (
    echo ----------------------------------------
    echo Building Tekla %%v...
    echo ----------------------------------------

    REM Build
    msbuild Tekla.Extension.sln /p:Configuration=%%v /p:Platform=x64 /t:Rebuild /v:minimal /nologo
    if errorlevel 1 (
        echo ERROR: Build failed for Tekla %%v
        pause
        exit /b 1
    )

    REM Pack
    echo Packing NuGet package for Tekla %%v...
    Tekla.Extension\nuget.exe pack Tekla.Extension\Tekla.Extension.csproj -Properties Configuration=%%v -OutputDirectory Tekla.Extension\bin\%%v\x64 -Verbosity quiet
    if errorlevel 1 (
        echo ERROR: Pack failed for Tekla %%v
        pause
        exit /b 1
    )

    echo SUCCESS: Built and packed Tekla %%v
    echo.
)

echo ========================================
echo BUILD AND PACK COMPLETE!
echo ========================================
echo.
echo Packages created:
for %%v in (%VERSIONS%) do (
    for %%f in (Tekla.Extension\bin\%%v\x64\*.nupkg) do (
        echo   - %%~nxf
    )
)
echo.

REM If not publishing, exit here
if %PUBLISH%==0 (
    echo Build and pack complete. Packages are ready in bin\{version}\x64\
    echo.
    echo To publish to NuGet, run: BuildAndPublish.bat publish
    pause
    exit /b 0
)

REM Publishing section
echo ========================================
echo STEP 2: Publishing to NuGet.org
echo ========================================
echo.
echo WARNING: You are about to publish to NuGet.org!
echo This action cannot be undone.
echo.
echo Packages to be published:
for %%v in (%VERSIONS%) do (
    for %%f in (Tekla.Extension\bin\%%v\x64\*.nupkg) do (
        echo   - %%~nxf
    )
)
echo.
set /p CONFIRM="Type 'YES' to confirm publication: "
if /i not "%CONFIRM%"=="YES" (
    echo Publication cancelled.
    pause
    exit /b 0
)

echo.
echo Publishing packages...
echo.

for %%v in (%VERSIONS%) do (
    echo ----------------------------------------
    echo Publishing Tekla %%v to NuGet.org...
    echo ----------------------------------------

    for %%f in (Tekla.Extension\bin\%%v\x64\*.nupkg) do (
        Tekla.Extension\nuget.exe push "%%f" -Source https://api.nuget.org/v3/index.json
        if errorlevel 1 (
            echo ERROR: Failed to publish %%~nxf
            echo.
            echo Common issues:
            echo   1. API key not set. Run: nuget.exe setApiKey YOUR_KEY -Source https://api.nuget.org/v3/index.json
            echo   2. Package version already exists on NuGet
            echo   3. Network connection issue
            echo.
            pause
            exit /b 1
        )
        echo SUCCESS: Published %%~nxf
        echo.
    )
)

echo ========================================
echo PUBLISHING COMPLETE!
echo ========================================
echo.
echo All packages have been published to NuGet.org
echo View your packages at:
for %%v in (%VERSIONS%) do (
    echo   https://www.nuget.org/packages/Tekla.Extension.%%v
)
echo.
echo Note: It may take a few minutes for packages to appear in search results.
echo.
pause
