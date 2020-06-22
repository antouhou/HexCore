#!/usr/bin/env bash

VERSION=$1
API_KEY=$2
MISSING_OPTIONS_MESSAGE="
Invalid arguments!\n
Usage: ./publish.sh %version_to_publish% %nuget_key%
"
[[ -z "$1" ]] && { echo $MISSING_OPTIONS_MESSAGE; exit 1; }
[[ -z "$2" ]] && { echo $MISSING_OPTIONS_MESSAGE ; exit 1; }

dotnet pack ./HexCore/HexCore.csproj --configuration Release
dotnet nuget push ./HexCore/bin/Release/HexCore.$VERSION.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json