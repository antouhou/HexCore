VERSION=$1
API_KEY=$2
[[ -z "$1" ]] && { echo "Parameter 1 should be a version to publish" ; exit 1; }
[[ -z "$2" ]] && { echo "Parameter 2 should be the API key" ; exit 1; }

dotnet pack ./HexCore.csproj --configuration Release
dotnet nuget push ./bin/Release/HexCore.$VERSION.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json