echo (!) Run in Visual Studio Console (!)
pause

call Clean.bat


dotnet restore ..\Code\ThirdPartyLibrary\ThirdPartyLibrary.sln
dotnet build ..\Code\ThirdPartyLibrary\ThirdPartyLibrary.sln -c Release

call ExtractThirdPartyLibrary-net472.bat


dotnet restore ..\Code\DemoApp\DemoApp.sln
dotnet build ..\Code\DemoApp\DemoApp.sln -c Release

call ExtractNoSymbols-net472.bat

move ..\Code ..\_Code