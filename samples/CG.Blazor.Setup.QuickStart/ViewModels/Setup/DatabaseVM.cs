
using CG.Blazor.Forms.Attributes;
using CG.Blazor.Setup.QuickStart.ViewModels.Setup.Database;

namespace CG.Blazor.Setup.QuickStart.ViewModels.Setup
{
    /// <summary>
    /// This class is a model for the database portion of the setup.
    /// </summary>
    public class DatabaseVM
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the currently selected database type.
        /// </summary>
        [RenderMudRadioGroup(Options = "SqlServer, Mongo")]
        public string Selected { get; set; }

        /// <summary>
        /// This property contains the sqlserver portion of the setup.
        /// </summary>
        [RenderObject(VisibleExp = "x => x.Selected=\"SqlServer\"")]
        public SqlServerModel SqlServer { get; set; }

        /// <summary>
        /// This property contains the mongo portion of the setup.
        /// </summary>
        [RenderObject(VisibleExp = "x => x.Selected=\"Mongo\"")]
        public MongoModel Mongo { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="DatabaseVM"/>
        /// class.
        /// </summary>
        public DatabaseVM()
        {
            // Set default values.
            Selected = nameof(SqlServer);
            SqlServer = new SqlServerModel();
            Mongo = new MongoModel();
        }

        #endregion
    }
}
