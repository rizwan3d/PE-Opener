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
            HexFile.OnFileLoadStart += HexFile_OnLoadStart;
            HexEditor.OnHexEditorLoadStart += HexFile_OnHexEditorLoadStart;

            HexEditor.OnHexEditorLoadComplete += HexFile_OnLoadComplete;
            HexFile.OnLoadingStatusChange += HexFile_OnLoadingStatusChange;
        }

        private void HexFile_OnLoadingStatusChange(object? sender, LoadingStatusEventArgs e)
        {
            ChangeStatus(e.Status);
        }

        private void HexFile_OnLoadStart(object? sender, EventArgs e)
        {
            isLoadComplete = false;
            StateHasChanged();
        }

        private void HexFile_OnHexEditorLoadStart(object? sender, EventArgs e)
        {
            isLoadComplete = false;
            ChangeStatus("Hex Editor");
        }

        private void HexFile_OnLoadComplete(object? sender, EventArgs e)
        {
            isLoadComplete = true;
            StateHasChanged();
        }

        private async void ChangeStatus(string status)
        {
            Thread.Sleep(500);
            this.status = status;
            StateHasChanged();
            Console.WriteLine(status);
        }
    }
}
