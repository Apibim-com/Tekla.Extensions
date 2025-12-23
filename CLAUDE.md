# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Tekla.Extension is an unofficial extension library for Tekla Structures Open API that provides LINQ-compatible extension methods and helper utilities to simplify working with Tekla Structures models. The library enables cleaner, more readable code by wrapping Tekla's ModelObjectEnumerator patterns with modern C# LINQ queries.

## Build Commands

This project uses MSBuild (not dotnet CLI) as it targets .NET Framework 4.8.

### Multi-Version Support

The project supports multiple Tekla Structures API versions (2020-2025) through separate build configurations. Each configuration:
- Uses version-specific NuGet packages
- Defines a preprocessor constant (TEKLA2020, TEKLA2021, etc.)
- Outputs to a version-specific directory (`bin\{version}\x64\`)
- Generates a version-specific NuGet package (Tekla.Extension.{version})

### Restore packages
```bash
nuget restore Tekla.Extension.sln
```

### Build for specific Tekla version
```bash
msbuild Tekla.Extension.sln /p:Configuration=2020 /p:Platform=x64
msbuild Tekla.Extension.sln /p:Configuration=2021 /p:Platform=x64
msbuild Tekla.Extension.sln /p:Configuration=2022 /p:Platform=x64
msbuild Tekla.Extension.sln /p:Configuration=2023 /p:Platform=x64
msbuild Tekla.Extension.sln /p:Configuration=2024 /p:Platform=x64
msbuild Tekla.Extension.sln /p:Configuration=2025 /p:Platform=x64
```

### Build all versions
```bash
for %%v in (2020 2021 2022 2023 2024 2025) do msbuild Tekla.Extension.sln /p:Configuration=%%v /p:Platform=x64
```

### Default Configuration
If no configuration is specified, the project defaults to `2023|x64`.

### NuGet Package Output
Each build generates a version-specific NuGet package:
- `bin\2020\x64\Tekla.Extension.2020.{version}.nupkg`
- `bin\2021\x64\Tekla.Extension.2021.{version}.nupkg`
- `bin\2022\x64\Tekla.Extension.2022.{version}.nupkg`
- `bin\2023\x64\Tekla.Extension.2023.{version}.nupkg`
- `bin\2024\x64\Tekla.Extension.2024.{version}.nupkg`
- `bin\2025\x64\Tekla.Extension.2025.{version}.nupkg`

The project automatically generates NuGet packages on build (see `GeneratePackageOnBuild` in .csproj).

## Architecture

### Extension Method Pattern

The library is organized around static extension classes that extend Tekla Structures API types. Each extension class focuses on a specific Tekla type or concept:

- **LinqExtension.cs** - Core LINQ integration; `ToIEnumerable<T>()` converts `ModelObjectEnumerator` to `IEnumerable<T>` for LINQ queries
- **ModelObjectExtension.cs** - Base extensions for all ModelObjects including case-insensitive report property retrieval
- **AssemblyExtension.cs** - Assembly-specific operations (parts retrieval, bounding boxes, finding neighbors)
- **PartExtension.cs** - Part-specific operations (OBB/AABB calculation, weight, dimensions, profile type)
- **Geometry Extensions** - Point, Vector, Line, CoordinateSystem extensions for 3D geometry operations
- **BoltExtension.cs** - Bolt-related utilities
- **BeamExtension.cs** - Beam-specific helpers
- **ProfileExtension.cs** - Profile type utilities with custom ProfileType enum

### Key Helper Classes

- **ComponentHelper.cs** - Working with Tekla components
- **CuttingHelper.cs** - Cutting and fitting operations
- **Drawer.cs** - Visualization and drawing utilities (14KB - substantial helper)
- **Intersections.cs** - Geometric intersection calculations
- **FileExtension.cs** - File operations

### Services Layer

- **Services/ProfileTypeEnumConverter.cs** - Converts Tekla profile type strings to custom ProfileType enum

### Core Design Principle

The library wraps Tekla's enumerator-based patterns to enable LINQ queries:

**Before (Tekla API):**
```csharp
ModelObjectEnumerator enumerator = selector.GetSelectedObjects();
while (enumerator.MoveNext()) {
    if (enumerator.Current is Assembly assembly) {
        // work with assembly
    }
}
```

**After (Tekla.Extension):**
```csharp
selector.GetSelectedObjects()
    .ToIEnumerable<Assembly>()
    .FirstOrDefault()
```

### Report Property Handling

`ModelObjectExtension.GetReportProperty<T>()` provides:
- Case-insensitive property name matching
- Automatic whitespace normalization (converts spaces to underscores, removes extra spaces)
- Generic type conversion for string, int, double
- Overloads with/without `out bool isSuccess` parameter

### Bounding Box Calculations

Two bounding box types are provided:
- **AABB** (Axis-Aligned Bounding Box) - Aligned to global coordinate system
- **OBB** (Oriented Bounding Box) - Aligned to part's local coordinate system

Parts and Assemblies can retrieve both types via extension methods.

## Dependencies

- **Tekla.Structures SDK** (version 2023.0.0) - Multiple packages including Model, Drawing, Catalogs, Datatype, Dialog, Plugins
- **DotNetZip** (1.13.7) - File compression utilities
- **Mono.Cecil** (0.9.6.1) - Assembly manipulation
- **Newtonsoft.Json** (13.0.1) - JSON serialization
- **Tekla.Technology.*** packages - Scripting and plugin support

## NuGet Package

The project builds multiple NuGet package versions targeting different Tekla Structures releases (2020-2024). Package specification is in `Tekla.Extension.nuspec`.

## Code Conventions

- Target framework: .NET Framework 4.8
- C# language version: latest (specified in .csproj)
- XML documentation is generated for Debug and Release builds
- Static extension classes use the pattern `{Type}Extension` (e.g., PartExtension, AssemblyExtension)
- Namespace: `Tekla.Extension`
