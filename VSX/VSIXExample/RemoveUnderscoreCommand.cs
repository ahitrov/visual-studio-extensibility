using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace VSIXExample
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RemoveUnderscoreCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("83d837b5-b72f-48e4-8be5-81bbba0124f1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnderscoreCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private RemoveUnderscoreCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RemoveUnderscoreCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Verify the current thread is the UI thread - the call to AddCommand in RemoveUnderscoreCommand's constructor requires
            // the UI thread.
            ThreadHelper.ThrowIfNotOnUIThread();

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new RemoveUnderscoreCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            Task.Run(async () => {
                var dte = await this.package.GetServiceAsync(typeof(SDTE)) as DTE2;

                dte.Find.Action = vsFindAction.vsFindActionReplaceAll;
                dte.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxRegExpr;
                dte.Find.MatchCase = false;
                dte.Find.MatchInHiddenText = false;
                dte.Find.MatchWholeWord = false;
                dte.Find.Target = vsFindTarget.vsFindTargetCurrentProject;

                // Remove the leading underscore in the names of all private fields
                dte.Find.FindWhat = RegexConstants.MatchLeadingUnderscoreInPrivateFieldDeclarations;
                dte.Find.ReplaceWith = RegexConstants.RemoveLeadingUnderscoreReplacePattern;

                dte.Find.Execute();
            });
        }
    }
}
