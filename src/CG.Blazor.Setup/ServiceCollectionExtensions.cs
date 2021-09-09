using CG.DataAnnotations;
using CG.DataProtection;
using CG.Options;
using CG.Validations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CG.Blazor.Setup
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type.
    /// </summary>
    internal static partial class ServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method registers the setup model as a service and provides a 
        /// default implementation that is read from the appsetup.json file.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for 
        /// the operation.</param>
        /// <param name="modelType">The type of model to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> 
        /// parameter, for chaining calls together.
        /// </returns>
        public static IServiceCollection AddSetupConfiguration(
            this IServiceCollection serviceCollection,
            Type modelType
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection));

            // Register the type as a service.
            serviceCollection.AddSingleton(modelType, serviceProvider =>
            {
                // Open the setup configuration (if there is one).
                var builder = new ConfigurationBuilder();
                if (File.Exists("appsetup.json"))
                {
                    // For some reason, if I use AddJson here instead of explicitly 
                    //   opening the file and passing in the stream, like I'm doing
                    //   now, the underlying JSON never gets reloaded in the configuration.
                    //   Yes, I used the reloadOnChange parameter, still didn't work. 
                    // I even tried explicitly calling Reload on the setConfiguration
                    //   variable and that didn't work either. Not sure why, this is
                    //   just a workaround, for now.
                    // I don't like this though. I'm not sure what the lifetime of the
                    //   stream is. We could potentially end up with share violations
                    //   when we try to update the appsetup.json file, this way.
                    builder.AddJsonStream(
                        File.OpenRead("appsetup.json")
                        );
                }
                var setupConfiguration = builder.Build();

                // Create the model instance.
                var model = ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    modelType,
                    Array.Empty<object>()
                    );

                // Bind the model to the configuration.
                setupConfiguration.Bind(model);

                // Unprotect any properties.
                DataProtector.Instance()
                    .UnprotectProperties(model);

                // Validate the model - if possible.
                (model as OptionsBase)?.ThrowIfInvalid();

                // Return the unprotected model.
                return model;
            });

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
