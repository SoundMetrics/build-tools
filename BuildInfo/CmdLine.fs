// Copyright (c) 2012-2015 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

open CommandLine
open Model

type Options () =
    [<Option('v', "version-file", Required = true, HelpText = "The major/minor/patch version file")>]
    member val VersionFile = "" with get, set

    [<Option('t', "output-type", Required = true, HelpText = "The type of output generated")>]
    member val OutputType = OutputType.CSharp with get, set

    [<Option('o', "output-path", Required = true, HelpText = "The path to which the output is written")>]
    member val OutputPath = "" with get, set

    [<Option('n', "namespace", HelpText = "The namespace to be used for the generated BuildInfo class")>]
    member val Namespace = "" with get, set

    [<Option('b', "build-number", Required = true, HelpText = "The build number to be used in version numbers")>]
    member val BuildNumber = "" with get, set

    [<HelpOption>]
    member __.GetUsage() =
        sprintf
            @"
    USAGE: BuildInfo -v <MajorMinorVersionFile> -t (CSharp|FSharp|ResourceCompiler) -o <output-path> -n <e.g., Aris.Core>

        -n, --namespace         The namespace to be used for the generated BuildInfo class (CSharp only)
        -t, --output-type       The type of output generated
        -o, --output-path       The path to which the output is written
        -v, --version-file      The major/minor version file
        -b, --build-number      The build number to be used (default is %d)

"
            DefaultBuildNumber
