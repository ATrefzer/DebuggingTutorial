move ..\_Code ..\Code

rmdir /q /s ..\Demo

rmdir /q /s ..\Code\DemoApp\bin
rmdir /q /s ..\Code\DemoApp\obj
rmdir /q /s ..\Code\DemoApp\.vs
del ..\Code\ExternalLibraries\ThirdPartyLibrary.dll



rmdir /q /s ..\Code\ThirdPartyLibrary\bin
rmdir /q /s ..\Code\ThirdPartyLibrary\obj
rmdir /q /s ..\Code\ThirdPartyLibrary\.vs