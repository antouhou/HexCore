#!/usr/bin/env bash

dotnet restore --source https://api.nuget.org/v3/index.json
dotnet build --configuration Release --no-restore