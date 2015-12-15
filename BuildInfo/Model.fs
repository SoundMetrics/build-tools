// Copyright (c) 2012-2015 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

module Model =

    type OutputType = // Needs to be an enum for command line processor.
        | CSharp = 1
        | FSharp = 2
        | ResourceCompiler = 3


    [<Literal>]
    let DefaultBuildNumber = 55555 // This is a 16-bit value on Windows.
