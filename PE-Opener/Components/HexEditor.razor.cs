using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PEOpener.Infrastuture;
using PEOpener.Layout;

namespace PEOpener.Components
{
    public partial class HexEditor : ComponentBase
    {
        [Inject]
        private IJSRuntime JSModule { get; set; }
        public static event EventHandler OnHexEditorLoadComplete = delegate { };
        public static event EventHandler OnHexEditorLoadStart = delegate { };

        public HexEditor() 
        {
            HexFile.OnFileLoadComplete += async (o,e) => await createHexEditor(HexFile.HexBytes);
            SideBar.OnSectionChange += async (o,e) => await createHexEditor(e.Bytes);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await createHexEditor(new byte[]{ });
            }
        }

        private async Task createHexEditor(byte[] bytes)
        {
            OnHexEditorLoadStart?.Invoke(this, EventArgs.Empty);
            await JSModule.InvokeVoidAsync("createHexEditor", "hexEditor", bytes);
            OnHexEditorLoadComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}
