using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace AcuCleanerVSExtension
{
    /// <summary>
    /// Dockable tool window that hosts the AcuCleaner UI.
    /// Registered via [ProvideToolWindow] on <see cref="AcuCleanerVSExtensionPackage"/>.
    /// </summary>
    [Guid(AcuCleanerToolWindow.ToolWindowGuidString)]
    public sealed class AcuCleanerToolWindow : ToolWindowPane
    {
        public const string ToolWindowGuidString = "b1c2d3e4-f5a6-7890-bcde-f01234567890";

        public AcuCleanerToolWindow() : base(null)
        {
            Caption = "AcuCleaner";
            Content = new AcuCleanerToolWindowControl();
        }
    }
}
