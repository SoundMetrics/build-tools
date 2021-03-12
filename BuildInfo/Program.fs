// Copyright (c) 2012-2018 Sound Metrics. All Rights Reserved.

namespace BuildInfo

module Main =

    open Model
    open System
    open System.IO

    [<AutoOpen>]
    module MainImpl =

        let writeStreamToPath (stm : Stream) path =

            if File.Exists(path) then
                use file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)
                stm.Position <- 0L
                if not (Streams.areIdentical stm file) then
                    stm.Position <- 0L
                    file.Position <- 0L
                    file.SetLength(0L)
                    stm.CopyTo(file)
            else
                use file = new FileStream(path, FileMode.CreateNew, FileAccess.Write)
                stm.Position <- 0L
                stm.CopyTo(file)

        let buildTheOutput (options : Options) =
            // Build key value pairs of the input from 'svn info'

            let versionString = Inputs.getVersion options.VersionFile
            let major, minor, patch = Inputs.parseRevString versionString
            let buildNumber = Inputs.parseBuildNumber options.BuildNumber

            // Define how new fields are generated from input

            let commaVersion =    sprintf "%d,%d,%d,%d" major minor patch buildNumber
            let dottedVersion =   sprintf "%d.%d.%d.%d" major minor patch buildNumber

            let allPairs = [
                "SemanticVersion",          sprintf "%d.%d.%d" major minor patch
                "MajorMinorRev",            versionString
                "BuildNumber",              buildNumber.ToString(System.Globalization.CultureInfo.InvariantCulture)
                "Thumbprint",               options.Thumbprint
                "CommitDescription",        options.CommitDescription
                "Namespace",                options.Namespace.Trim()
                "BuildDate",                DateTime.Now.ToString("d MMM yyyy")
                "RcFileVersion",            commaVersion
                "RcFileVersionString",      dottedVersion
                "RcProductVersion",         commaVersion
                "RcProductVersionString",   dottedVersion
                "AssemblyVersion",          dottedVersion
                "AssemblyFileVersion",      dottedVersion
            ]

            // Make subsitutions in templates

            let generated = new MemoryStream()
            use writer = new StreamWriter(generated)

            fprintfn writer "// %A" options.OutputType
            fprintfn writer "%s" (Inputs.replaceFields allPairs "// {AssemblyFileVersion}")
            fprintfn writer "//"

            allPairs |> Seq.iter (fun pair -> let key, value = pair
                                              fprintfn writer "// [%s] = [%s] " key value)

            let selectedTemplate = match options.OutputType with
                                   | OutputType.CSharp ->           Templates.CSharpTemplate
                                   | OutputType.FSharp ->           Templates.FSharpTemplate
                                   | OutputType.ResourceCompiler -> Templates.RCTemplate
                                   | _ -> failwith "unimplemented template type"

            fprintfn writer "%s" (Inputs.replaceFields allPairs selectedTemplate)
            writer.Flush()

            writeStreamToPath generated options.OutputPath

            printfn "%s" (Inputs.replaceFields allPairs "BuildInfo: {AssemblyFileVersion}")


    open CommandLine

    [<EntryPoint>]
    let main argv =

        let result = Parser.Default.ParseArguments<Options>(argv)
        match result with
        | :? Parsed<Options> as options ->
            let options = options.Value
            let outputType = options.OutputType

            let hasNamespaceOption = not (String.IsNullOrWhiteSpace(options.Namespace))

            match outputType with
            | OutputType.CSharp | OutputType.FSharp ->
                if not hasNamespaceOption then
                    printfn "-n or --namespace is required for %A" outputType
                    Environment.Exit -2

            | OutputType.ResourceCompiler -> if hasNamespaceOption then
                                                 printfn "-n or --namespace is not allowed for %A" outputType
                                                 Environment.Exit -3
            | _ -> failwith "unhandled output type"


            let buildNumber = Inputs.parseBuildNumber options.BuildNumber

            // Output BuildInfo parms
            printfn "BuildInfo.exe"
            printfn "--output-type:  %A" outputType
            printfn "--output-path:  %s" options.OutputPath
            printfn "--version-file: %s" options.VersionFile
            printfn "--build-number: %d" buildNumber

            match outputType with
            | OutputType.CSharp | OutputType.FSharp -> printfn "--namespace:    %s" (options.Namespace.Trim())
            | _ -> ()

            buildTheOutput options

        | :? NotParsed<Options> as _errors -> Environment.Exit(-1)
        | o -> failwithf "Unexpected command line parse result: %s" (o.ToString())

        0 // return an integer exit code
