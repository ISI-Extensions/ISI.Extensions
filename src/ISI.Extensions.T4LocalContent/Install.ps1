param($installPath, $toolsPath, $package, $project)

$projectFolder = $project.Properties.Item("FullPath").Value

try {
	$t4LocalContentFolderProjectItem = $project.ProjectItems.Item("T4LocalContent")
}
catch {
		Write-Host "No T4LocalContent folder found"
}

$t4LocalContentFolderProjectItem.ProjectItems.Item("T4LocalContent.tt").Object.RunCustomTool()
