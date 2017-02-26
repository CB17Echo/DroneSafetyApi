#!/bin/bash
dotnet restore
cd src/DroneSafetyApi
dotnet build
cd ../..
cd test/DroneSafetyApi.UnitTests
dotnet test