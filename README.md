## build-tools

These are tools we use when building source; these are used in our other public repositories.

- **BuildInfo.exe** This is a .NET application that generates version information.
It can generate C#, F# or Win32 Resource Compiler source. Run BuildInfo.exe for usage information.

The `binaries` folder is a convenient place to access these tools.
The [Packet](https://fsprojects.github.io/Paket/) package manager can be used to fetch these binaries at build time.
