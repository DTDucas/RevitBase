using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitBase.Commands.Events;
using Application = Autodesk.Revit.ApplicationServices.Application;
using View = Autodesk.Revit.DB.View;

namespace RevitBase.Commands;

/// <summary>
///     Centralized context for accessing Revit application objects and external events.
/// </summary>
public static class RevitAppContext
{
    // Private fields for lazy initialization
    private static ExternalEvent? _externalEvent;
    private static ExternalEventHandlerAction? _handlerAction;

    private static ExternalEventHandlers? _externalEventHandlers;

    // Core Revit objects
    /// <summary>
    /// Gets the current UIDocument instance.
    /// </summary>
    public static UIDocument? UiDoc { get; private set; }

    /// <summary>
    /// Gets the current Document instance.
    /// </summary>
    public static Document? Document { get; private set; }

    /// <summary>
    /// Gets the current Application instance.
    /// </summary>
    public static Application? Application { get; private set; }

    /// <summary>
    /// Gets the current UIApplication instance.
    /// </summary>
    public static UIApplication? UiApplication { get; private set; }

    /// <summary>
    /// Gets the current Selection instance.
    /// </summary>
    public static Selection? Selection { get; private set; }

    /// <summary>
    /// Gets the currently active View.
    /// </summary>
    public static View? ActiveView { get; private set; }

    // App information
    /// <summary>
    /// Gets the current user's username.
    /// </summary>
    public static string? Username { get; private set; }

    /// <summary>
    /// Gets or sets the path to application settings.
    /// </summary>
    public static string? SettingPath { get; set; }

    /// <summary>
    /// Gets or sets the application version.
    /// </summary>
    public static string? Version { get; set; }

    /// <summary>
    /// Gets or sets the error log information.
    /// </summary>
    public static string ErrorLog { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the singleton instance of the external event handler.
    /// </summary>
    public static ExternalEventHandlerAction HandlerAction
    {
        get => _handlerAction ??= new ExternalEventHandlerAction();
        set => _handlerAction = value;
    }

    /// <summary>
    ///     Gets the singleton instance of the external event.
    /// </summary>
    public static ExternalEvent ExternalEvent
    {
        get => _externalEvent ??= ExternalEvent.Create(HandlerAction);
        set => _externalEvent = value;
    }

    /// <summary>
    ///     Gets the singleton instance of the external event handlers collection.
    /// </summary>
    public static ExternalEventHandlers ExternalEventHandlers
    {
        get => _externalEventHandlers ??= new ExternalEventHandlers();
        set => _externalEventHandlers = value;
    }

    /// <summary>
    ///     Initializes context information from a UIDocument.
    /// </summary>
    public static void Initialize(UIDocument uiDoc)
    {
        UiDoc = uiDoc;
        Document = uiDoc.Document;
        Application = uiDoc.Application.Application;
        UiApplication = uiDoc.Application;
        Selection = uiDoc.Selection;
        ActiveView = Document.ActiveView;
        ErrorLog = string.Empty;
    }

    /// <summary>
    ///     Initializes context information from an ExternalCommandData object.
    /// </summary>
    public static void Initialize(ExternalCommandData data)
    {
        var uiDoc = data.Application.ActiveUIDocument;
        UiDoc = uiDoc;
        Document = uiDoc.Document;
        Application = uiDoc.Application.Application;
        UiApplication = uiDoc.Application;
        Selection = uiDoc.Selection;
        Username = Application.Username;
        ActiveView = Document.ActiveView;
        ErrorLog = string.Empty;
    }
}