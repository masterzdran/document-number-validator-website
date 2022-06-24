using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Portugal.Nif.Validator;

namespace NifValidatorComponent
{
    public partial class NifValidatorModule
    {
        private readonly NifValidator validator;

        [Parameter]
        public string Nif { get; set; } = string.Empty;

        [Parameter]
        public bool IsValid { get; set; }

        [Parameter]
        public string Title { get; set; }

        public NifValidatorModule()
        {
            this.validator = new NifValidator();
            this.IsValid = false;
            this.Title = string.Empty;

        }
        public async Task<bool> Validate()
        {
            await Task.CompletedTask;
            IsValid = validator.Validate(Nif);
            return  IsValid;
        }
    }
}
