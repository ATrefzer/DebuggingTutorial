rmdir /q /s ..\Demo

mkdir ..\Demo
mkdir ..\Demo\Symbols
mkdir ..\Demo\App

xcopy "..\Code\DemoApp\bin\Release\net5.0-windows\*.exe" ..\Demo\App 
xcopy "..\Code\DemoApp\bin\Release\net5.0-windows\*.dll" ..\Demo\App 
rem xcopy "..\Code\DemoApp\bin\Release\net5.0-windows\*.json" ..\Demo\App 
xcopy "..\Code\DemoApp\bin\Release\net5.0-windows\*.pdb" ..\Demo\Symbols