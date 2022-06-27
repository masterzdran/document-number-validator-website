using DocumentNumber.Portugal.Niss.Generator;
using Microsoft.AspNetCore.Components;

namespace NissGeneratorComponent
{
    public partial class NissGeneratorModule
    {
        private readonly NissGenerator nissGenerator;
        public NissGeneratorModule()
        {
            this.nissGenerator = new NissGenerator();
            this.DocumentNumber = String.Empty;
            this.Title = String.Empty;

        }

        [Parameter]
        public string DocumentNumber { get; set; }

        [Parameter]
        public string Title { get; set; }

        public async Task<string> GenerateRandomDocumentNumber()
        {
            await Task.CompletedTask;
            DocumentNumber = nissGenerator.GenerateDocumentNumber();

            return DocumentNumber;
        }
    }
}
