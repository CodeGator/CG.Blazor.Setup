
using CG.Blazor.Forms.Attributes;
using CG.Blazor.Setup.QuickStart.ViewModels.Setup;

namespace CG.Blazor.Setup.QuickStart.ViewModels
{
    /// <summary>
    /// This class is a view-model for the setup plugin.
    /// </summary>
    [RenderFluentValidationValidator]
    [RenderValidationSummary]
    [RenderMudTabs]
    public class SetupVM
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the database portion of the setup.
        /// </summary>
        [RenderMudTabPanel]
        public DatabaseVM Database { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SetupVM"/>
        /// class.
        /// </summary>
        public SetupVM()
        {
            // Set default values.
            Database = new DatabaseVM();
        }

        #endregion
    }
}
