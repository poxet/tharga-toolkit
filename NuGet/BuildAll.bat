@echo off
'Trigger a clean/re-build of the entire solution

@echo Tharga.Toolkit
..\_External\NuGet.exe pack ..\Tharga.Toolkit\Tharga.Toolkit.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.Console
..\_External\NuGet.exe pack ..\Tharga.Toolkit.Console\Tharga.Toolkit.Console.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.ServerStorage
..\_External\NuGet.exe pack ..\Tharga.Toolkit.ServerStorage\Tharga.Toolkit.ServerStorage.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.LocalStorage
..\_External\NuGet.exe pack ..\Tharga.Toolkit.LocalStorage\Tharga.Toolkit.LocalStorage.csproj -Prop Configuration=Release

@echo Tharga.Toolkit.StorageConsole
..\_External\NuGet.exe pack ..\Tharga.Toolkit.StorageConsole\Tharga.Toolkit.StorageConsole.csproj -Prop Configuration=Release
