SafeNuGet
=========

MsBuild task to warn about insecure NuGet libraries

Installation
============
1. Install the NuGet package or copy the dll into your project
2. Alter the .csproj files of your solution to run the MsBuild task. Make sure AssemblyFile points to the location of your SafeNuGet.dll:
    &lt;UsingTask AssemblyFile="SafeNuGet.dll" TaskName="SafeNuGet.AreNuGetPackagesSafe" /&gt;
    &lt;Target Name="AfterBuild"&gt;
      &lt;AreNuGetPackagesSafe /&gt;
    &lt;/Target&gt;
3. Build

Want to contribute?
===================
Great! If you want to contribute to the list of unsafe libraries, please create a pull request or email me at erlend@oftedal.no.

Code contributions are also very welcome. Fork and create a pull request.

Experience an issue?
====================
Register it here at github: [issues](https://github.com/eoftedal/SafeNuGet/issues)

