// Copyright (c) 2012-2018 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

module Inputs =

    open System
    open System.IO

    exception TooManyPartsInVersionString of string

    let parseRevString (revString: string) =

        let parts = revString.Split([| '.' |], StringSplitOptions.RemoveEmptyEntries)
        match parts with
        | [||] ->                       0, 0, 0
        | [| major |] ->                Int32.Parse(major), 0, 0
        | [| major; minor|] ->          Int32.Parse(major), Int32.Parse(minor), 0
        | [| major; minor; patch|] ->
            // Allow an "-alpha" style suffix on the third term.
            // At this time, we don't use it in this utility program.
            let patchNumericLength = patch |> Seq.filter (fun ch -> ch |> Char.IsDigit)
                                           |> Seq.length
            let trimmedPatch = patch.Substring(0, patchNumericLength)
            Int32.Parse(major), Int32.Parse(minor), Int32.Parse(trimmedPatch)
        | _ -> raise (TooManyPartsInVersionString revString)


    let getVersion (filePath: string) =

        use file = new StreamReader(filePath)
        file.ReadLine().Trim()

    let replaceFields (pairs: (string * string) seq) (outputTemplate: string) =

        let mutable output = outputTemplate

        for pair in pairs do
            let key, value = pair
            output <- output.Replace(sprintf "{%s}" key, value)

        output

    let parseBuildNumber (s: string) =

        if String.IsNullOrWhiteSpace s then
            Model.DefaultBuildNumber
        else
            let success, n = Int32.TryParse s
            if success then
                n
            else
                raise (Exception("Bad build number: " + s))
