# Code Bits
Code Bits is a set of useful code blocks that can included in your .NET projects through [NuGet](http://nuget.org/). Each code bit is a single source file that gets added to a _CodeBits_ folder in your project when you import it.

Code Bits does not add assemblies to your project.

You can do a search for '[CodeBits](https://www.nuget.org/packages?q=CodeBits)' on NuGet.org to view the list of available Code Bits packages.

## General code bits
| Name | Description |
|------|-------------|
| *[ByteArrayHelper](https://github.com/JeevanJames/CodeBits/wiki/ByteArrayHelper)* | Set of utility extensions for byte arrays. |
| *[ByteSizeFriendlyName](https://github.com/JeevanJames/CodeBits/wiki/ByteSizeFriendlyName)* | Builds a friendly string representation of a specified byte size value, after converting it to the best matching unit (bytes, KB, MB, GB, etc.). |
| *[EnumIterator](https://github.com/JeevanJames/CodeBits/wiki/EnumIterator)* | Provides an iterator for traversing through the values of an enum type. |
| *[IniFile](https://github.com/JeevanJames/CodeBits/wiki/IniFile)* | Class to read and modify .INI files. |
| *[PasswordGenerator](https://github.com/JeevanJames/CodeBits/wiki/PasswordGenerator)* | Generates a random password. |
| *[SaltedHash](https://github.com/JeevanJames/CodeBits/wiki/SaltedHash)* | A helper class to generate and validate salted hashes. |

## Custom collections
| Name | Description |
|------|-------------|
| *[OrderedCollection<T>](https://github.com/JeevanJames/CodeBits/wiki/OrderedCollection)* | An automatically sorted collection that has options to allow/disallow duplicate items and optionally sort in reverse order. |
| *OrderedObservableCollection<T>* | An automatically sorted observable collection that has options to allow/disallow duplicate items and optionally sort in reverse order. |

## Windows-specific code bits
| Name | Description |
|------|-------------|
| *[WindowsServiceRunner](https://github.com/JeevanJames/CodeBits/wiki/WindowsServiceRunner)* | Helper class to execute a Windows Service project either as a Console Application or a Windows Service, depending on the project type. |
