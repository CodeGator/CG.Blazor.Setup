
using CG.Blazor.Forms.Attributes;
using CG.DataProtection;

namespace CG.Blazor.Setup.QuickStart.ViewModels.Setup.Database
{
    public class MongoModel
    {
        [RenderMudTextField]
        [ProtectedProperty]
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    }
}
