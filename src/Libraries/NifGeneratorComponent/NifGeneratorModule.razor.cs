using DocumentNumber.Portugal.Nif.Generator;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifGeneratorComponent
{
    public partial class NifGeneratorModule
    {
        private readonly NifGenerator generator;

        [Parameter]
        public string Nif { get; set; } = string.Empty;

        [Parameter]
        public int StartWithNumber { get; set; }

        [Parameter]
        public string Title { get; set; }

        public NifGeneratorModule()
        {
            this.generator = new NifGenerator();
            this.StartWithNumber = 1;
            this.Title = string.Empty;

        }
        public async Task<string> GenerateRandomDocumentNumber()
        {
            await Task.CompletedTask;
            this.Nif = generator.GenerateDocumentNumber(StartWithNumber); 
            return this.Nif;
        }
    }
}
