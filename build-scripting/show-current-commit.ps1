$branchName = "" + (git branch --show-current)
$branchName = $branchName.Trim()

$commitHash = "" + (git show --oneline)
$commitHash = $commitHash.Split()[0]

if ([string]::IsNullOrWhiteSpace($branchName)) {
  $branchName = "HEAD detached at " + $commitHash
}
else {
  $branchName = $branchName + " (" + $commitHash + ")"

}

$branchName
