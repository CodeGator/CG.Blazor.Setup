using CG.Blazor.Setup.Alerts;
using CG.Validations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CG.Blazor.Setup
{
    /// <summary>
    /// This class utility contains methods that help with managing setup
    /// change operations, that typically require the host to be restarted.
    /// </summary>
    public static class HostHelper
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods
        
        /// <summary>
        /// This method calls the <paramref name="builderDelegate"/> delegate
        /// to create a host builder, then it builds the host and runs while 
        /// watching for setup change alerts. When an alert is observed, the
        /// method restarts the host - since the services all probably need
        /// to be re-created with the new setup configuration.
        /// </summary>
        /// <param name="builderDelegate">The delegate for building the host 
        /// builder.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task to perform the operation.</returns>
        public static async Task RunAsSetupObserverAsync(
            Func<IHostBuilder> builderDelegate,
            CancellationToken cancellationToken = default
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(builderDelegate, nameof(builderDelegate));

            // We'll be notified whenever the setup is changed via the alert service,
            //   which will handle cancelling a token for us. We'll link to that token
            //   here so we can loop and restart the host if a setup change occures, or,
            //   we can exit normally if the host itself exited.

            // Loop until the token is cancelled.
            while (false == cancellationToken.IsCancellationRequested)
            {
                // Create a linked token, which is what the host will use.
                var hostToken = CancellationTokenSource.CreateLinkedTokenSource(
                    SetupChangedAlert.TokenSource.Token,
                    cancellationToken
                    );

                // Create the host.
                var host = builderDelegate().Build();

                // Get a logger.
                var logger = host.Services.GetRequiredService<ILogger<IHost>>();

                // Tell the world what we are doing.
                logger.LogInformation(
                    $"~~~~~ Starting the host. ~~~~~"
                    );

                // Run the host.
                await host.RunAsync(
                    hostToken.Token
                    ).ConfigureAwait(false);

                // Did the host stop naturally?
                if (!SetupChangedAlert.TokenSource.IsCancellationRequested)
                {
                    // Tell the world what we are doing.
                    logger.LogInformation(
                        $"~~~~~ Exiting the process. ~~~~~"
                        );

                    // If we get here then the host stopped on it's own so we
                    //   want to exit gracefully.
                    break;
                }

                // Tell the world what we are doing.
                logger.LogInformation(
                    $"~~~~~ Restarting the host. ~~~~~"
                    );

                // If we get here then the host stopped due to a setup change
                //   so we want to loop and restart it.

                // Reset the token source.
                SetupChangedAlert.TokenSource = new CancellationTokenSource();
            }
        }

        #endregion
    }
}
