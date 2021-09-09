using CG.Validations;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Logging;
using System.IO;

namespace CG.Blazor.Setup.Rules
{
    /// <summary>
    /// This class represents a rule for rewriting urls during setup mode.
    /// </summary>
    internal class SetupModeRule : IRule
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains a logger.
        /// </summary>
        private readonly ILogger<SetupModeRule> _logger;

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SetupModeRule"/>
        /// class.
        /// </summary>
        /// <param name="logger">The logger to use with the rule.</param>
        public SetupModeRule(
            ILogger<SetupModeRule> logger
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(logger, nameof(logger));

            // Save the references.
            _logger = logger;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public void ApplyRule(RewriteContext context)
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(context, nameof(context));

            // Is there a setup file?
            if (File.Exists("appsetup.json"))
            {
                return; // Nothing to do.
            }
            
            // If we redirect these urls we break Blazor.
            if (context.HttpContext.Request.Path.StartsWithSegments("/_blazor") ||
                context.HttpContext.Request.Path.StartsWithSegments("/negotiate") ||
                context.HttpContext.Request.Path == "/service-worker.js"
                )
            {
                return; // Nothing to do.
            }

            // Rewrite everything except the setup page.
            if (!context.HttpContext.Request.Path.StartsWithSegments("/setup"))
            {
                // Tell the world what we're doing.
                _logger.LogInformation(
                    "Rewriting url '{Url}' to '/setup'",
                    context.HttpContext.Request.Path.Value
                    );

                // Rewrite the url.
                context.HttpContext.Request.Path = "/setup";
            }
        }

        #endregion
    }
}
