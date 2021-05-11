# Publishing packages
CodeBits packages are published independently, based on which source files are changed. Hence the versioning is independent and packages will have different versions.

The `Package.nuspec.t` file contains the nuspec template for all packages. It contains a gomplate template. For each package, there is a JSON file of the same name that contains its unique information to be injected into the template to generate the final nuspec file.

To publish a given package, run the `publish.ps1` file in the root directory in the following format:

```powershell
.\publish.ps1 <package name> <version>
```

* `<package name>`: The name of the corresponding JSON file in the `nuget` folder.
* `<version>`: The semantic version of the package to publish.

