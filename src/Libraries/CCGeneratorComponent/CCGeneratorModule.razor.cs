using DocumentNumber.Portugal.CitizenCard.Generator;
using Microsoft.AspNetCore.Components;

namespace CCGeneratorComponent
{
    public partial class CCGeneratorModule
    {
        private readonly ICitizenCardNumberGenerator citizenCardNumberGenerator;

        [Parameter]
        public string CitizenCard { get; set; }
        [Parameter]
        public string Title { get; set; }

        public CCGeneratorModule()
        {
            this.citizenCardNumberGenerator = new CitizenCardNumberGenerator();
            this.CitizenCard = String.Empty;
            this.Title = String.Empty;
        }
        public async Task<string> GenerateRandomDocumentNumber()
        {
            await Task.CompletedTask;
            CitizenCard = citizenCardNumberGenerator.GenerateDocumentNumber();

            return CitizenCard;
        }
    }
}
