using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PEOpener.Components
{
    public partial class HexEditor : ComponentBase
    {
        [Inject]
        private IJSRuntime JSModule { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSModule.InvokeVoidAsync("createHexEditor", "hexEditor", new { });
            }
        }
    }
}
