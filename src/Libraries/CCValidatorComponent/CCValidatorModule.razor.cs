using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentNumber.Portugal.CitizenCard.Validator;

namespace CCValidatorComponent
{
    public partial class CCValidatorModule
    {
        private readonly CitizenCardValidator validator;

        [Parameter]
        public string CitizenCard { get; set; } = string.Empty;

        [Parameter]
        public bool IsValid { get; set; }

        public CCValidatorModule()
        {
            this.validator = new CitizenCardValidator();

        }
        public async Task<bool> Validate()
        {
            await Task.CompletedTask;
            IsValid = validator.Validate(CitizenCard);
            return IsValid;
        }
    }
}
