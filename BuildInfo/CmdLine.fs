// Copyright (c) 2012-2018 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

open CommandLine
open Model

type Options = {
    [<Option('v', "version-file", Required = true, HelpText = "The major/minor/patch version file")>]
    VersionFile : string

    [<Option('t', "output-type", Required = true, HelpText = "The type of output generated")>]
    OutputType : OutputType

    [<Option('o', "output-path", Required = true, HelpText = "The path to which the output is written")>]
    OutputPath : string

    [<Option('n', "namespace", HelpText = "The namespace to be used for the generated BuildInfo class")>]
    Namespace : string

    [<Option('b', "build-number", Required = true, HelpText = "The build number to be used in version numbers")>]
    BuildNumber : string

    [<Option('p', "thumbprint", Required = false, HelpText = "The value to be used as a thumbprint")>]
    Thumbprint : string
}
