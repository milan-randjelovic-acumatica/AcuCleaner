using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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

        /// <summary>The owning package — set during InitializeAsync.</summary>
        internal static AsyncPackage? Package { get; set; }

        public AcuCleanerToolWindow() : base(null)
        {
            Caption = "AcuCleaner";
            Content = new AcuCleanerToolWindowControl();
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            // Center the window and prevent maximization
            if (Frame is IVsWindowFrame frame)
            {
                // Set window to be not maximized and centered
                // Get the main VS window to center relative to it
                IntPtr vsHwnd = GetVSMainWindowHandle();
                if (vsHwnd != IntPtr.Zero)
                {
                    GetWindowRect(vsHwnd, out RECT vsRect);

                    int toolWindowWidth = 900;
                    int toolWindowHeight = 600;
                    int vsWidth = vsRect.Right - vsRect.Left;
                    int vsHeight = vsRect.Bottom - vsRect.Top;

                    int newX = vsRect.Left + (vsWidth - toolWindowWidth) / 2;
                    int newY = vsRect.Top + (vsHeight - toolWindowHeight) / 2;

                    // Set window position and size (SFP_fFrameNormal = 2)
                    Guid rguidPersistenceSlot = Guid.Empty;
                    frame.SetFramePos((VSSETFRAMEPOS)2, ref rguidPersistenceSlot, newX, newY, toolWindowWidth, toolWindowHeight);
                }
            }
        }

        private IntPtr GetVSMainWindowHandle()
        {
            if (Package == null)
                return IntPtr.Zero;

            var serviceProvider = (IServiceProvider)(Package as IServiceProvider);
            if (serviceProvider == null)
                return IntPtr.Zero;

            var uiShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            if (uiShell == null)
                return IntPtr.Zero;

            uiShell.GetDialogOwnerHwnd(out IntPtr hwnd);
            return hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    }
}
