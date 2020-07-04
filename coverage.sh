#!/usr/bin/env bash
dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput='../coverage/lcov.info'
