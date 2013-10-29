@echo off Trigger a clean/re-build of the entire solution

@echo Tharga.Toolkit
..\..\..\Utils\NuGet.exe pack ..\Tharga.Toolkit\Tharga.Toolkit.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.Console
..\..\..\Utils\NuGet.exe pack ..\Tharga.Toolkit.Console\Tharga.Toolkit.Console.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.ServerStorage
..\..\..\Utils\NuGet.exe pack ..\Tharga.Toolkit.ServerStorage\Tharga.Toolkit.ServerStorage.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.LocalStorage
..\..\..\Utils\NuGet.exe pack ..\Tharga.Toolkit.LocalStorage\Tharga.Toolkit.LocalStorage.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.StorageConsole
..\..\..\Utils\NuGet.exe pack ..\Tharga.Toolkit.StorageConsole\Tharga.Toolkit.StorageConsole.csproj -Prop Configuration=Release

xcopy *.nupkg ..\..\..\NuGet /Y