$script_location = $PSScriptRoot
$show_script = "" + [System.Io.Path]::Combine($script_location, "show-current-commit.ps1")

$status_lines = (git status --short | Measure-Object -Line).Lines
$dirty = ""
if ($status_lines -gt 0) {
    $dirty = " dirty"
}

$commitDescription = "" + (powershell.exe $show_script) + $dirty

$self = $MyInvocation.MyCommand.Path
$cwd = Get-Location

"// Generated by: [$self]"
"// Script at:    [$script_location]"
"// Commit:       [$commitDescription]"
"// CWD:          [$cwd]"
""
"namespace SoundMetrics {"
"    public static class CommitDescription {"
"        public static readonly string Value = ""$commitDescription"";"
"    }"
"}"

Pop-Location