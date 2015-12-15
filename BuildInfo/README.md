## BuildInfo.exe

This is a .NET application that generates version information.
It can generate C#, F# or Win32 Resource Compiler source. Run BuildInfo.exe for usage information.

Some build systems will update version information (e.g., **AssemblyInfo Patcher** in TeamCity)
but unfortunately make the version information part of the build configuration.

We record our version numbers in a file that is placed under source control.
This allows for a repeatable build, including version numbers.
