
# win
dotnet publish -c Release -o ./output/windows -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:UseAppHost=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:SelfContained=true --self-contained true


# linux
dotnet publish -c Release -o ./output/linux -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:UseAppHost=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:SelfContained=true --self-contained true


# macOS
dotnet publish -c Release -o ./output/macOS -r osx-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true /p:UseAppHost=true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:SelfContained=true --self-contained true