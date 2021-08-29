# Code Bits
Code Bits is a set of useful code blocks that can included in your .NET projects through [NuGet](http://nuget.org/). Each code bit is a single source file that gets added to a _CodeBits_ folder in your project when you import it.

Code Bits does not add assemblies to your project.

You can do a search for '[CodeBits](https://www.nuget.org/packages?q=CodeBits)' on NuGet.org to view the list of available Code Bits packages.

## General code bits
| Name | Compatibility | Description |
|------|---------------|-------------|
| *[ByteArrayHelper](https://github.com/JeevanJames/CodeBits/wiki/ByteArrayHelper)* | .NET Framework 3.5 and higher | Set of utility extensions for byte arrays. |
| *[ByteSizeFriendlyName](https://github.com/JeevanJames/CodeBits/wiki/ByteSizeFriendlyName)* | .NET Framework 3.5 and higher | Builds a friendly string representation of a specified byte size value, after converting it to the best matching unit (bytes, KB, MB, GB, etc.). |
| *[EnumIterator](https://github.com/JeevanJames/CodeBits/wiki/EnumIterator)* | .NET Framework 3.5 and higher | Provides an iterator for traversing through the values of an enum type. |
| *[IniFile](https://github.com/JeevanJames/CodeBits/wiki/IniFile)* | .NET Framework 3.5 and higher | Class to read and modify .INI files. |
| *[PasswordGenerator](https://github.com/JeevanJames/CodeBits/wiki/PasswordGenerator)* | .NET Framework 3.5 and higher | Generates a random password. |
| *[SaltedHash](https://github.com/JeevanJames/CodeBits/wiki/SaltedHash)* | .NET Framework 3.5 and higher | A helper class to generate and validate salted hashes. |
| *[ShortGuid](https://github.com/JeevanJames/CodeBits/wiki/ShortGuid)* | .NetStandard 2.1 and higher | Shorter, URL-friendly and readable GUID. |

## Custom collections
| Name | Description |
|------|-------------|
| *[OrderedCollection<T>](https://github.com/JeevanJames/CodeBits/wiki/OrderedCollection)* | An automatically sorted collection that has options to allow/disallow duplicate items and optionally sort in reverse order. |
| *OrderedObservableCollection<T>* | An automatically sorted observable collection that has options to allow/disallow duplicate items and optionally sort in reverse order. |

## Windows-specific code bits
| Name | Description |
|------|-------------|
| *[WindowsServiceRunner](https://github.com/JeevanJames/CodeBits/wiki/WindowsServiceRunner)* | Helper class to execute a Windows Service project either as a Console Application or a Windows Service, depending on the project type. |
