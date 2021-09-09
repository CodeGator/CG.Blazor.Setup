
using CG.Blazor.Forms.Attributes;
using CG.DataProtection;

namespace CG.Blazor.Setup.QuickStart.ViewModels.Setup.Database
{
    public class SqlServerModel
    {
        [RenderMudTextField]
        [ProtectedProperty]
        public string ConnectionString { get; set; } = "Server=.;Database=CG.Blazor.Setup.QuickStart;Trusted_Connection=True;";

        [RenderMudSlider(Label = "Command Timeout", Min = 0, Max = 90)]
        public int? CommandTimeout { get; set; } = 30;

        [RenderMudRadioGroup(Options = "None, CircuitBreaker, WaitAndRetry")]
        public string RetryPattern { get; set; } = "None";
    }
}
