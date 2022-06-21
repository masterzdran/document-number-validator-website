using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Portugal.Nif.Validator;

namespace DocumentNumberValidatorWebsite
{
    public partial class NifValidatorModule
    {
       
        private string nif = string.Empty;
        private readonly NifValidator validator;
        private bool isValid;

        [Parameter]
        public string Nif { get => nif; set => nif = value; }

        [Parameter]
        public bool IsValid { get => isValid; set => isValid = value; }

        public NifValidatorModule()
        {
            this.validator = new NifValidator();

        }
        public async Task<bool> Validate()
        {
            await Task.CompletedTask;
            isValid = validator.Validate(Nif);
            return  isValid;
        }
    }
}
