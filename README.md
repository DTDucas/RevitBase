# DTDucas.RevitBase

[![NuGet](https://img.shields.io/nuget/v/DTDucas.RevitBase.svg)](https://www.nuget.org/packages/DTDucas.RevitBase)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Revit Versions](https://img.shields.io/badge/Revit-2021--2026-blue.svg)](https://www.autodesk.com/products/revit)

Base framework for Revit API development with comprehensive command handling, exception suppression, and external event management.

## 🚀 Features

- **Multi-Version Support**: Compatible with Revit 2021-2026
- **Base External Command**: Comprehensive abstract base class for external commands
- **Exception & Dialog Suppression**: Built-in support for handling Revit failures and dialogs
- **External Event Framework**: Thread-safe execution of Revit API operations
- **Command Model**: Data structure for command tracking and auditing
- **License Utilities**: Command execution validation and tracking functionality
- **Comprehensive Error Handling**: Standardized error management and reporting
- **RevitAppContext**: Centralized context for accessing Revit application objects

## 📦 Installation

Install the package for your target Revit version:

### Package Manager Console
```powershell
# For Revit 2025
Install-Package DTDucas.RevitBase -Version 2025.1.0.0

# For Revit 2024
Install-Package DTDucas.RevitBase -Version 2024.1.0.0
```

### .NET CLI
```bash
# For Revit 2025
dotnet add package DTDucas.RevitBase --version 2025.1.0.0

# For Revit 2024
dotnet add package DTDucas.RevitBase --version 2024.1.0.0
```

### PackageReference (csproj)
```xml
<!-- For Revit 2025 -->
<PackageReference Include="DTDucas.RevitBase" Version="2025.1.0.0" />

<!-- For Revit 2024 -->
<PackageReference Include="DTDucas.RevitBase" Version="2024.1.0.0" />

<!-- For Revit 2023 -->
<PackageReference Include="DTDucas.RevitBase" Version="2023.1.0.0" />

<!-- For Revit 2022 -->
<PackageReference Include="DTDucas.RevitBase" Version="2022.1.0.0" />

<!-- For Revit 2021 -->
<PackageReference Include="DTDucas.RevitBase" Version="2021.1.0.0" />
```

## 🏗️ Architecture

### Core Components

1. **BaseExternalCommand**: Abstract base class for external commands
2. **RevitAppContext**: Centralized application context management
3. **ExternalEventHandlerAction**: Singleton external event handler
4. **ExternalEventHandlers**: Multi-action external event handler
5. **CommandModel**: Data model for command tracking
6. **LicenseUtils**: License validation and command tracking

## 🔧 Usage Examples

### Creating a Custom External Command

```csharp
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitBase.Commands;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class MyCustomCommand : BaseExternalCommand
{
    public override Result Execute()
    {
        try
        {
            // Initialize RevitAppContext
            RevitAppContext.Initialize(ExternalCommandData);

            // Your command logic here
            var doc = RevitAppContext.Document;
            var selection = RevitAppContext.Selection;

            // Example: Get selected elements
            var selectedIds = selection.GetElementIds();

            using (var transaction = new Transaction(doc, "My Custom Command"))
            {
                transaction.Start();

                // Your modifications here

                transaction.Commit();
            }

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Result.Failed;
        }
    }
}
```

### Using Exception and Dialog Suppression

```csharp
public class SuppressedCommand : BaseExternalCommand
{
    public override Result Execute()
    {
        // Suppress all exceptions
        SuppressExceptions();

        // Suppress failures (warnings/errors)
        SuppressFailures();

        try
        {
            // Your risky operations here
            // Exceptions and failures will be automatically handled

            return Result.Succeeded;
        }
        finally
        {
            // Cleanup is automatic in the base class
        }
    }
}
```

### Using External Events

```csharp
public class ExternalEventExample
{
    public void TriggerExternalEvent()
    {
        // Set the action to execute
        RevitAppContext.HandlerAction.SetAction(() =>
        {
            var doc = RevitAppContext.Document;
            using (var transaction = new Transaction(doc, "External Event Action"))
            {
                transaction.Start();
                // Your modifications here
                transaction.Commit();
            }
        });

        // Raise the external event
        RevitAppContext.ExternalEvent.Raise();
    }
}
```

### Custom Exception Handling

```csharp
public class CustomHandlingCommand : BaseExternalCommand
{
    public override Result Execute()
    {
        // Custom exception handler
        SuppressExceptions(ex =>
        {
            // Log the exception
            Console.WriteLine($"Error occurred: {ex.Message}");

            // Show custom dialog
            TaskDialog.Show("Error", $"Command failed: {ex.Message}");
        });

        // Your command logic here

        return Result.Succeeded;
    }
}
```

### Command Tracking and Auditing

```csharp
public class TrackedCommand : BaseExternalCommand
{
    public override Result Execute()
    {
        try
        {
            // Command execution logic

            // The base class automatically tracks command execution
            // using LicenseUtils.CreateCommand() extension method

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Result.Failed;
        }
    }
}
```

## 🛡️ Error Handling

The framework provides comprehensive error handling:

### Automatic Exception Suppression
```csharp
// Suppress all exceptions
SuppressExceptions();

// Suppress with custom handler
SuppressExceptions(ex =>
{
    // Custom error handling logic
});
```

### Failure Management
```csharp
// Automatically resolve Revit failures/warnings
SuppressFailures();

// Failures are automatically restored in cleanup
```

### Status Tracking
```csharp
public override Result Execute()
{
    // Status is automatically set based on Result
    // Status.Success for Result.Succeeded
    // Status.Error for Result.Failed

    return Result.Succeeded;
}
```

## 🔍 Context Management

### RevitAppContext Usage
```csharp
// Initialize context (done automatically in BaseExternalCommand)
RevitAppContext.Initialize(commandData);

// Access Revit objects
var doc = RevitAppContext.Document;
var uiDoc = RevitAppContext.UiDoc;
var app = RevitAppContext.Application;
var uiApp = RevitAppContext.UiApplication;
var selection = RevitAppContext.Selection;
var activeView = RevitAppContext.ActiveView;

// Access user information
var username = RevitAppContext.Username;
var version = RevitAppContext.Version;

// Error logging
RevitAppContext.ErrorLog = "Custom error message";
```

## 📚 API Reference

### BaseExternalCommand Methods

| Method | Description |
|--------|-------------|
| `Execute()` | Abstract method to implement command logic |
| `SuppressExceptions()` | Suppress all exceptions |
| `SuppressExceptions(Action<Exception>)` | Suppress with custom handler |
| `SuppressFailures()` | Suppress Revit failures/warnings |
| `RestoreDialogs()` | Restore dialog handling |
| `RestoreFailures()` | Restore failure processing |

### RevitAppContext Properties

| Property | Type | Description |
|----------|------|-------------|
| `UiDoc` | `UIDocument?` | Current UI document |
| `Document` | `Document?` | Current document |
| `Application` | `Application?` | Revit application |
| `UiApplication` | `UIApplication?` | UI application |
| `Selection` | `Selection?` | Current selection |
| `ActiveView` | `View?` | Active view |
| `Username` | `string?` | Current user |
| `ErrorLog` | `string` | Error log information |

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [NuGet Package](https://www.nuget.org/packages/DTDucas.RevitBase)
- [GitHub Repository](https://github.com/DTDucas/RevitBase)
- [Issues](https://github.com/DTDucas/RevitBase/issues)

## 🏷️ Version Compatibility

| Revit Version | Package Version | .NET Framework |
|---------------|-----------------|----------------|
| 2021 | 2021.x.x.x | .NET Framework 4.8 |
| 2022 | 2022.x.x.x | .NET Framework 4.8 |
| 2023 | 2023.x.x.x | .NET Framework 4.8 |
| 2024 | 2024.x.x.x | .NET Framework 4.8 |
| 2025 | 2025.x.x.x | .NET 8.0 |
| 2026 | 2026.x.x.x | .NET 8.0 |

## ✨ Author

**Duong Tran Quang - DTDucas**
- GitHub: [@DTDucas](https://github.com/DTDucas)
- Email: baymax.contact@gmail.com