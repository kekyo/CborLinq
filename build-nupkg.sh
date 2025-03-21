#!/bin/sh

# CborLinq - Simple CBOR (Concise Binary Object Representation) serializer/deserializer
# Copyright (c) Kouji Matsui (@kekyo@mi.kekyo.net)
#
# Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0

echo
echo "==========================================================="
echo "Build CborLinq"
echo

# git clean -xfd

dotnet restore

dotnet build -p:Configuration=Release -p:Platform=AnyCPU CborLinq/CborLinq.csproj
dotnet pack -p:Configuration=Release -p:Platform=AnyCPU -o artifacts CborLinq/CborLinq.csproj
