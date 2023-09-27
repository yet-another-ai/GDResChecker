#!/bin/sh -l

dotnet tool install -g YetAnotherAI.Godot.ResourceChecker.Console
check-godot-resource $1
