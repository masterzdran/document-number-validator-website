using DocumentNumber.InternationalBankAccountNumber.Validator;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBANValidatorComponent
{
    public partial class IBANValidatorModule
    {
        private readonly InternationalBankAccountNumberValidator validator;

        [Parameter]
        public string Iban { get; set; } = string.Empty;

        [Parameter]
        public bool IsValid { get; set; }

        public IBANValidatorModule()
        {
            this.validator = new InternationalBankAccountNumberValidator();

        }
        public async Task<bool> Validate()
        {
            await Task.CompletedTask;
            IsValid = validator.Validate(Iban);
            return IsValid;
        }
    }
}
