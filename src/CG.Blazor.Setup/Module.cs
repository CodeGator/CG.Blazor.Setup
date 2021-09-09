using CG.Blazor.Plugins;
using CG.Blazor.Setup.Alerts;
using CG.Blazor.Setup.Options;
using CG.Blazor.Setup.Rules;
using CG.DataProtection;
using CG.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CG.Blazor.Setup
{
    /// <summary>
    /// This class represents the plugin module's startup logic.
    /// </summary>
    public class Module : ModuleBase
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public override void ConfigureServices(
            IServiceCollection serviceCollection,
            IConfiguration configuration
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Configure (and unprotect) the plugin options.
            serviceCollection.ConfigureOptions<PluginOptions>(
                DataProtector.Instance(), // <-- default data protector.
                configuration,
                out var options
                );

            // We'll need the model.
            serviceCollection.AddSetupConfiguration(
                Type.GetType(options.ModelType, true)
                );

            // We'll rewrite urls, as the need arises.
            serviceCollection.AddSingleton<SetupModeRule>();
            
            // We'll raise alerts, as the need arises.
            serviceCollection.AddAlertServices(options =>
            {
                options.AddCustomAlertType<SetupChangedAlert>();
            });

            // We'll generate our own forms.
            serviceCollection.AddFormGeneration();
        }

        // *******************************************************************

        /// <inheritdoc/>
        public override void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(app, nameof(app))
                .ThrowIfNull(env, nameof(env));

            // Get the plugin options.
            var pluginOptions = app.ApplicationServices.GetRequiredService<
                IOptions<PluginOptions>
                >();

            // Should we wire up a url rewriter rule?
            if (pluginOptions.Value.RedirectPages)
            {
                // Get the setup rule.
                var rule = app.ApplicationServices.GetRequiredService<
                    SetupModeRule
                    >();

                // Add the rule to the framework.
                app.UseRewriter(
                    new RewriteOptions()
                        .Add(rule)
                    );
            }

            // Start the alert services.
            app.UseAlertServices(env);
        }

        #endregion
    }
}
