// Enumerate a folder tree for files and version information.

open System
open System.Diagnostics
open System.IO

let usage = @"fsi EnumFolders.fsx -- <folder-name> <output-csv>"

type Result<'T, 'E> =
| Okay of 'T
| Error of 'E

// Get only the args after the first "--"
let getProgramArgs args =

    let rec findDelim argList =
        match argList with
        | "--" :: rem ->    rem |> List.toArray
        | _ :: rem ->       findDelim rem
        | [] ->             Array.empty

    findDelim (args |> Array.toList)

type System.Diagnostics.FileVersionInfo
with
    static member HeaderRow =
        "File Name, Path, File Description, Product Name, Company Name, Comments, Copyright, "
        + "File Version, Product Version"

    member vi.ToCsvRow () =
        let quoteString s = sprintf "\"%s\"" s

        let fileVersion =   if vi.FileVersion = Unchecked.defaultof<string> then
                                ""
                            else
                                sprintf "%d.%d.%d.%d"
                                    vi.FileMajorPart
                                    vi.FileMinorPart
                                    vi.FileBuildPart
                                    vi.FilePrivatePart
        let productVersion =    if vi.ProductVersion = Unchecked.defaultof<string> then
                                    ""
                                else
                                    sprintf "%d.%d.%d.%d"
                                        vi.ProductMajorPart
                                        vi.ProductMinorPart
                                        vi.ProductBuildPart
                                        vi.ProductPrivatePart

        sprintf "%s, %s, %s, %s, %s, %s, %s, %s, %s"
            (Path.GetFileName(vi.FileName) |> quoteString)
            (vi.FileName        |> quoteString)
            (vi.FileDescription |> quoteString)
            (vi.ProductName     |> quoteString)
            (vi.CompanyName     |> quoteString)
            (vi.Comments        |> quoteString)
            (vi.LegalCopyright  |> quoteString)
            (fileVersion        |> quoteString)
            (productVersion     |> quoteString)

let writeCsvHeader (outputFile : TextWriter) =
    outputFile.WriteLine(sprintf "%s" FileVersionInfo.HeaderRow)

let writeCsvRow (outputFile : TextWriter) (fileInfo : FileVersionInfo) =
    outputFile.WriteLine(sprintf "%s" (fileInfo.ToCsvRow()))

let rec enumFiles outputFile folder =
    Directory.EnumerateFiles(folder)
        |> Seq.map FileVersionInfo.GetVersionInfo
        |> Seq.iter (writeCsvRow outputFile)

    Directory.EnumerateDirectories(folder) |> Seq.iter (enumFolder outputFile)

and enumFolder outputFile path =
    enumFiles outputFile path

// Get the root folder and output file from arguments.
let parseArgs () =
    // Get only the args that apply to the program.
    let args = getProgramArgs fsi.CommandLineArgs

    match args with
    | [| rootFolder; outputFile |] -> Okay (rootFolder, outputFile)
    | [||] ->       Error "No arguments"
    | [| _ |] ->    Error "Too few arguments"
    | _ ->          Error "Too many arguments"

match parseArgs() with
| Error msg ->  eprintfn "ERROR: %s" msg
                eprintfn "USAGE: %s" usage
                exit 1
| Okay (rootFolder, outputFile) ->
    let output = new StreamWriter(outputFile, append = false)
    writeCsvHeader output
    enumFolder output rootFolder
    output.Close()

    printfn ""
    printfn "Success. Wrote file '%s'" outputFile
