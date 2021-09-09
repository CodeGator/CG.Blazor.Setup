
using CG.Blazor.Setup.QuickStart.ViewModels;
using FluentValidation;

namespace CG.Blazor.Setup.QuickStart.Validators
{
    /// <summary>
    /// This class represents a validator for the <see cref="SetupVM"/> class.
    /// </summary>
    public class SetupVMValidator : AbstractValidator<SetupVM>
    {
        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SetupVMValidator"/>
        /// class.
        /// </summary>
        public SetupVMValidator()
        {
            // Ensure the Database property is populated.
            RuleFor(x => x.Database)
                .NotNull()
                .NotEmpty()
                .WithMessage("Database settings are missing!");

            // Rules for validating the Database property.
            RuleSet("Database", () =>
            {
                // Ensure a database type is selected.
                RuleFor(x => x.Database.Selected)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Must select a database type!");
                
                // Ensure the Mongo and SqlServer properties are populated.
                RuleFor(x => x.Database.Mongo)
                    .NotNull()
                    .WithMessage("Mongo settings are missing!");
                RuleFor(x => x.Database.SqlServer)
                    .NotNull()
                    .WithMessage("SqlServer settings are missing!");

                // If SqlServer is selected, ensure the connection is populated.
                RuleFor(x => x.Database.SqlServer.ConnectionString)
                    .NotNull()
                    .NotEmpty()
                    .When(x => x.Database.Selected == "SqlServer")
                    .WithMessage("SqlServer connnection string is required!");

                // If Mongo is selected, ensure the connection is populated.
                RuleFor(x => x.Database.Mongo.ConnectionString)
                    .NotNull()
                    .NotEmpty()
                    .When(x => x.Database.Selected == "Mongo")
                    .WithMessage("Mongo connnection string is required!");
            });
        }

        #endregion
    }
}
