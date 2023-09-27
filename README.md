# GDResChecker
GDResChecker is a tool for Godot resource existence checking which path specified in Scene(.tscn) files and Script(C#) files.

## Use as command line tool
### Install
```bash
dotnet tool install --global GDResChecker
```

### Usage
```bash
check-godot-resource /path/to/your/godot/project
```

## Use as GitHub action
Add this step to your action workflows.
```yml
    steps:
      - use: yet-another-ai/GDResChecker/action@main
        with:
          project: /path/to/your/godot/project
```

## Use it in GitLab Pipelines
Example:
```yml
check:resources:
  image: mcr.microsoft.com/dotnet/sdk:6.0
  before_script:
    - dotnet tool install -g YetAnotherAI.Godot.ResourceChecker.Console
  script:
    - check-godot-resource /path/to/your/godot/project
```

## How It Works
1. Collect all files path in your project directories recursively.
2. Read scene file, find external resource definition like `[ext_resource path="res://a.glb"]`, then check if all files path contains this path.
3. Read C# file, use `Microsoft.CodeAnalysis.CSharp` package to travel syntax tree, find string literal expression which starts with `res://`, then check if all files path contains this path.
4. If resource not exists, this tool will print all non-exists resource path then exit with code 1. If all resources exist, the tool will exit with code 0.
