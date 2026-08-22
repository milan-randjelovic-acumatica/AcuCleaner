using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace AcuCleanerVSExtension
{
    /// <summary>
    /// Command handler for the Tools &gt; AcuCleaner menu item.
    /// </summary>
    internal sealed class AcuCleanerCommand
    {
        /// <summary>Command set GUID — must match the value in AcuCleanerPackage.vsct.</summary>
        public static readonly Guid CommandSet = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

        /// <summary>Command ID — must match AcuCleanerCommandId in AcuCleanerPackage.vsct.</summary>
        public const int CommandId = 0x0100;

        private readonly AsyncPackage _package;

        private AcuCleanerCommand(AsyncPackage package, IMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// Must be called from <see cref="AcuCleanerVSExtensionPackage.InitializeAsync"/>.
        /// </summary>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            AcuCleanerToolWindow.Package = package;

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
            if (commandService != null)
                _ = new AcuCleanerCommand(package, commandService);
        }

        /// <summary>Called when the user clicks Tools &gt; AcuCleaner.</summary>
        private void Execute(object sender, EventArgs e)
        {
            _ = _package.ShowToolWindowAsync(
                toolWindowType: typeof(AcuCleanerToolWindow),
                id: 0,
                create: true,
                cancellationToken: _package.DisposalToken);
        }
    }
}
