#!/usr/bin/env bash

VERSION=$1
API_KEY=$2
PACKAGES_API_KEY=$3
MISSING_OPTIONS_MESSAGE="
Invalid arguments!\n
Usage: ./publish.sh %version_to_publish% %nuget_key% %GitHub_packages_key%
"
[[ -z "$1" ]] && { echo $MISSING_OPTIONS_MESSAGE; exit 1; }
[[ -z "$2" ]] && { echo $MISSING_OPTIONS_MESSAGE ; exit 1; }
[[ -z "$3" ]] && { echo $MISSING_OPTIONS_MESSAGE ; exit 1; }

dotnet pack ./HexCore/HexCore.csproj --configuration Release
PACKAGES_API_KEY=${PACKAGES_API_KEY} dotnet nuget push ./HexCore/bin/Release/HexCore.$VERSION.nupkg -s "github" --skip-duplicate
dotnet nuget push ./HexCore/bin/Release/HexCore.$VERSION.nupkg -k ${API_KEY} -s "nuget.org" --skip-duplicate