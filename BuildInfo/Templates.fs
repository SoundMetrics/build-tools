// Copyright (c) 2012-2015 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

module Templates =

    // This file is actually the correct place to modify the content.
    let commonFileHeader = 
          @"
//-------------------------------------------------------------------------------------------------
// WARNING: !!! This file contains only automatically generated content. Do not modify it. !!!
//-------------------------------------------------------------------------------------------------
"

    //-----------------------------------------------------------------------------

    let CSharpTemplate =
        commonFileHeader +
          @"
using System;
using System.Reflection;

[assembly: AssemblyVersion(""{AssemblyVersion}"")]
[assembly: AssemblyFileVersion(""{AssemblyFileVersion}"")]

namespace {Namespace}
{
    public static class BuildInfo
    {
        public const string SemanticVersion = ""{SemanticVersion}"";
        public const string FullVersion = ""{AssemblyFileVersion}"":
        public const string BuildNumber = ""{BuildNumber}"";
        public const string BuildDate = ""{BuildDate}"";
    }
}
"

    //-----------------------------------------------------------------------------

    let FSharpTemplate =
        commonFileHeader +
          @"
namespace {Namespace}

open System.Reflection

module BuildInfo =

    [<Literal>]
    let SemanticVersion = ""{SemanticVersion}""

    [<Literal>]
    let FullVersion = ""{AssemblyFileVersion}""

    [<Literal>]
    let BuildNumber = {BuildNumber}

    [<Literal>]
    let BuildDate = ""{BuildDate}""

[<assembly: AssemblyVersion(""{AssemblyVersion}"")>]
[<assembly: AssemblyFileVersion(""{AssemblyFileVersion}"")>]

do ()
"

    //-----------------------------------------------------------------------------
    // Win32 resource compiler

    let RCTemplate = 
        commonFileHeader +
          @"
#define FILE_VERSION {RcFileVersion}
#define FILE_VERSION_STRING ""{RcFileVersionString}""
#define PRODUCT_VERSION {RcProductVersion}
#define PRODUCT_VERSION_STRING ""{RcProductVersionString}""
#define BUILD_NUMBER {BuildNumber}
"
