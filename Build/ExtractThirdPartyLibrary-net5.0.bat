rmdir /q /s ..\Code\ExternalLibraries
mkdir ..\Code\ExternalLibraries

rem 3rd party library
xcopy "..\Code\ThirdPartyLibrary\bin\Release\net5.0\*.dll" "..\Code\ExternalLibraries\" 
