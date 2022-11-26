using System.Runtime.InteropServices.JavaScript;
using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Pages
{
    public partial class IndexPageBase : ComponentBase
    {
        protected int Width = 600;
        protected int Height = 600;
        public bool HasLoaded { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("Index",
                "../Pages/Index.razor.js");

            Width = GetWidth();
            Height = GetHeight();
            HasLoaded = true;
        }

        [JSImport("GetWidth", "Index")]
        internal static partial int GetWidth();


        [JSImport("GetHeight", "Index")]
        internal static partial int GetHeight();
    }
}
