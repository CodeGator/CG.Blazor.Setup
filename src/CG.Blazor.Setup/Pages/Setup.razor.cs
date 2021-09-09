using CG.Alerts;
using CG.Blazor.Setup.Alerts;
using CG.Blazor.Setup.Options;
using CG.DataAnnotations;
using CG.DataProtection;
using CG.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CG.Blazor.Setup.Pages
{
    /// <summary>
    /// This class is the code-behind for the <see cref="Setup"/> 
    /// razor page.
    /// </summary>
    public partial class Setup
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to the plugin options.
        /// </summary>
        [Inject]
        protected IOptions<PluginOptions> PluginOptions { get; set; }

        /// <summary>
        /// This property contains a reference to the logger for the page.
        /// </summary>
        [Inject]
        protected ILogger<Setup> Logger { get; set; }

        /// <summary>
        /// This property contains a reference to a service provider.
        /// </summary>
        [Inject]
        protected IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// This property contains a reference to a data protector.
        /// </summary>
        [Inject] 
        protected IDataProtectionProvider DataProtectionProvider { get; set; }

        /// <summary>
        /// This property contains a reference to an alert service.
        /// </summary>
        [Inject]
        protected IAlertService AlertService { get; set; }

        /// <summary>
        /// This property contains a reference to the model.
        /// </summary>
        protected object Model { get; set; }

        /// <summary>
        /// This property contains error information for the UI.
        /// </summary>
        protected string Error { get; set; }

        /// <summary>
        /// This property contains information for the UI.
        /// </summary>
        protected string Information { get; set; }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is called by the edit form when the user submits valid
        /// data.
        /// </summary>
        /// <param name="editContext">The edit content to use for the operation.</param>
        protected async Task OnValidSubmit(EditContext editContext)
        {
            try
            {
                // No information, no error.
                Error = string.Empty;
                Information = string.Empty;

                // Make a quick clone of the model.
                var protectedModel = Model.QuickClone();

                // Protect any secrets in the model.
                DataProtector.Instance().ProtectProperties(
                    protectedModel
                    );

                // Serialize the model to JSON.
                var json = JsonSerializer.Serialize(
                    protectedModel,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true // Make purdy JSON.
                    });

                // Write out the JSON.
                await File.WriteAllTextAsync(
                    "appsetup.json", 
                    json
                    ).ConfigureAwait(false);

                // Tell the UI what we did.
                Information = "Setup was changed.\r\n" +
                    "NOTE: The alert was raised to restart the host.\r\n";

                // Give the UI time to update.
                await Task.Delay(
                    2000
                    ).ConfigureAwait(false);

                // Tell the world what we just did.
                await AlertService.RaiseAsync<SetupChangedAlert>()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Alert the UI.
                Error = $"Failed to save changes to the setup!";

                // Tell the world what happened.
                Logger.LogError(
                    ex,
                    "Failed to save changes to the setup! " +
                    "See internal exception(s) for more detail."
                    );
            }
        }

		// *******************************************************************

        /// <summary>
        /// This method is called to initialize the component.
        /// </summary>
		protected override void OnInitialized()
		{
            try
            {
                // No information, no error.
                Error = string.Empty;

                // Get the type for the model.
                var modelType = Type.GetType(
                    PluginOptions.Value.ModelType,
                    true
                    );

                // Get the model.
                Model = ServiceProvider.GetRequiredService(
                    modelType
                    );

                // Give the base class a chance.
                base.OnInitialized();
            }
            catch (Exception ex)
            {
                // Alert the UI.
                Error = $"Failed to initialize!";

                // Log the error.
                Logger.LogError(
                    ex,
                    "Failed to initialize! " +
                    "See internal exception(s) for more detail."
                    );
            }
		}

		#endregion
	}
}
