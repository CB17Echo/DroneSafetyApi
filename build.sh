#!/bin/bash
set -ev
dotnet restore
cd src/DroneSafetyApi
dotnet build
cd ../../test/DroneSafetyApi.UnitTests
dotnet test
