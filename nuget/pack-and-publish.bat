@echo off

del *.nupkg

nuget pack ByteArrayHelper.nuspec
nuget pack ByteSizeFriendlyName.nuspec
nuget pack EnumIterator.nuspec
nuget pack IniFile.nuspec
nuget pack MruCollection.nuspec
nuget pack OrderedCollection.nuspec
nuget pack OrderedObservableCollection.nuspec
nuget pack PasswordGenerator.nuspec
nuget pack SaltedHash.nuspec
nuget pack WindowsServiceRunner.nuspec
nuget pack WP.AppBarHelper.nuspec

rem Insert package versions for each file
nuget push ByteArrayHelper.nupkg -Source https://api.nuget.org/v3/index.json
nuget push ByteSizeFriendlyName.nupkg -Source https://api.nuget.org/v3/index.json
nuget push EnumIterator.nupkg -Source https://api.nuget.org/v3/index.json
nuget push IniFile.nupkg -Source https://api.nuget.org/v3/index.json
nuget push MruCollection.nupkg -Source https://api.nuget.org/v3/index.json
nuget push OrderedCollection.nupkg -Source https://api.nuget.org/v3/index.json
nuget push OrderedObservableCollection.nupkg -Source https://api.nuget.org/v3/index.json
nuget push PasswordGenerator.nupkg -Source https://api.nuget.org/v3/index.json
nuget push SaltedHash.nupkg -Source https://api.nuget.org/v3/index.json
nuget push WindowsServiceRunner.nupkg -Source https://api.nuget.org/v3/index.json
nuget push WP.AppBarHelper.nupkg -Source https://api.nuget.org/v3/index.json
