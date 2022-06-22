using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentNumber.Portugal.Niss.Validator;
using Microsoft.AspNetCore.Components;

namespace NissValidatorComponent
{
    public partial class NissValidatorModule
    {

        private readonly NissValidator validator;
        public NissValidatorModule()
        {
            this.validator = new NissValidator();
            this.Niss = String.Empty;

        }

        [Parameter]
        public string Niss { get; set; }


        [Parameter]
        public bool IsValid { get; set ; }



        public async Task<bool> Validate()
        {
            await Task.CompletedTask;
            IsValid = validator.Validate(Niss);
            return IsValid;
        }
    }
}
