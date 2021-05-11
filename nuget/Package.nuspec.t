<?xml version="1.0"?>
<package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
    <metadata>
        <version>0.1.0</version>
        <authors>Jeevan James</authors>
        <owners>Jeevan James</owners>
        <license type="expression">Apache-2.0</license>
        <projectUrl>https://github.com/JeevanJames/CodeBits</projectUrl>
        <icon>images\logo.png</icon>
        <!-- <readme>docs\README.md</readme> -->
        <id>CodeBits.{{ (datasource "data").id }}</id>
        <title>CodeBits {{ (datasource "data").title }}</title>
        <requireLicenseAcceptance>false</requireLicenseAcceptance>
        <description>{{ (datasource "data").description }}

CodeBits are useful code blocks that can included in your C# projects through NuGet.

See the project site for documentation.</description>
        <summary />
        <copyright>Copyright (c) Jeevan James 2012-2021</copyright>
        <tags>CodeBits code bits blocks {{ (datasource "data").tags }}</tags>
    </metadata>

    <files>
        <file src="..\..\logo.png" target="images\" />
        <file src="..\..\README.md" target="docs\" />

        <!-- For newer NuGet versions -->
        <file src="..\..\src\{{ (datasource "data").project }}\{{ (datasource "data").id }}.cs" target="contentFiles\cs\any\CodeBits\" />

        <!-- For older NuGet versions -->
        <file src="..\..\src\{{ (datasource "data").project }}\{{ (datasource "data").id }}.cs" target="content\CodeBits\" />
    </files>
</package>
