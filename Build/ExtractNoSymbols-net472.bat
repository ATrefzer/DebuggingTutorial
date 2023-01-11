rmdir /q /s ..\Demo

mkdir ..\Demo
mkdir ..\Demo\Symbols
mkdir ..\Demo\App

xcopy "..\Code\DemoApp\bin\Release\net472\*.exe" ..\Demo\App 
xcopy "..\Code\DemoApp\bin\Release\net472\*.dll" ..\Demo\App 
rem xcopy "..\Code\DemoApp\bin\Release\net472\*.json" ..\Demo\App 
xcopy "..\Code\DemoApp\bin\Release\net472\*.pdb" ..\Demo\Symbols