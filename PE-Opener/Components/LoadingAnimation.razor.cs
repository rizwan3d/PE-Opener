using Microsoft.AspNetCore.Components;
using PEOpener.Infrastuture;

namespace PEOpener.Components
{
    public partial class LoadingAnimation : ComponentBase
    {

        public ElementReference spiner;
        public bool isLoadComplete = true;
        public string status = string.Empty;

        public LoadingAnimation()
        {
            HexEditor.OnHexEditorLoadComplete += HexFile_OnLoadComplete;
            HexEditor.OnHexEditorLoadStart += HexFile_OnLoadStart;
            HexFile.OnFileLoadStart += HexFile_OnLoadStart2;
            HexFile.OnFileLoadStart += HexFile_OnLoadStart;
            HexFile.OnLoadingStatusChange += HexFile_OnLoadingStatusChange;
        }

        private void HexFile_OnLoadingStatusChange(object? sender, LoadingStatusEventArgs e)
        {
            this.InvokeAsync(() => {
                status = e.Status;
                this.StateHasChanged();
            });
        }

        private void HexFile_OnLoadStart(object? sender, EventArgs e)
        {
            isLoadComplete = false;
            StateHasChanged();
        }

        private void HexFile_OnLoadStart2(object? sender, EventArgs e)
        {
            status  = "Hex Editor";
            StateHasChanged();
        }

        private void HexFile_OnLoadComplete(object? sender, EventArgs e)
        {
            isLoadComplete = true;
            StateHasChanged();
        }
    }
}
