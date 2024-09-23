$compress = @{
  Path = "./BetterChef/bin/Debug/netstandard2.1/BetterChef.dll", "./CHANGELOG.md", "./icon.png", "./manifest.json", "./README.md"
  CompressionLevel = "Fastest"
  DestinationPath = "./BetterChefPkg.zip"
}
Compress-Archive @compress
