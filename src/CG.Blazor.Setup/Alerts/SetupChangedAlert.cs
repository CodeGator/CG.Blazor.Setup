using CG.Alerts;
using System.Threading;

namespace CG.Blazor.Setup.Alerts
{
    /// <summary>
    /// This alert is raised whenever the setup is changed.
    /// </summary>
    public class SetupChangedAlert : AlertEventBase
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to a shared token source.
        /// </summary>
        public static CancellationTokenSource TokenSource { get; set; } 
            = new CancellationTokenSource();

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is called whenever the alert is raised.
        /// </summary>
        /// <param name="args">The arguments for the alert.</param>
        protected override void OnInvoke(
            params object[] args
            )
        {
            // Cancel the token source.
            TokenSource?.Cancel();

            // Give the base class a chance.
            base.OnInvoke(args);
        }

        #endregion
    }
}
