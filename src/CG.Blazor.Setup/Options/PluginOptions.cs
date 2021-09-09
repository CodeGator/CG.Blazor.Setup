using CG.Options;
using System;

namespace CG.Blazor.Setup.Options
{
    /// <summary>
    /// This class contains configuration settings related to the setup plugin.
    /// </summary>
    public class PluginOptions : OptionsBase
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property indicates which type to use for the model.
        /// </summary>
        public string ModelType { get; set; }

        /// <summary>
        /// This property indicates whether the plugin should force the setup
        /// page to display if there is no appsetup.json file.
        /// </summary>
        public bool RedirectPages { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="PluginOptions"/>
        /// class.
        /// </summary>
        public PluginOptions()
        {
            // Set default values.
            //RaiseSetupChangedAlert = true;
        }

        #endregion
    }
}
