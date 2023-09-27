#!/bin/sh -l

dotnet tool install -g YetAnotherAI.Godot.ResourceChecker.Console
export PATH="$PATH:/github/home/.dotnet/tools"
check-godot-resource $1
