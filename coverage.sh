#!/usr/bin/env bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput='./coverage/lcov.info'
