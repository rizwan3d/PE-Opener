using Microsoft.AspNetCore.Components;
using PEOpener.Components;
using PEOpener.Infrastuture;

namespace PEOpener
{
    public partial class MainLayout
    {
        public ElementReference spiner;
        public bool isLoadComplete = true;

        public MainLayout()
        {
            HexEditor.OnHexEditorLoadComplete += HexFile_OnLoadComplete;
            HexEditor.OnHexEditorLoadStart += HexFile_OnLoadStart;
            HexFile.OnFileLoadStart += HexFile_OnLoadStart;
        }

        private void HexFile_OnLoadStart(object? sender, EventArgs e)
        {
            isLoadComplete = false;
            StateHasChanged();
        }

        private void HexFile_OnLoadComplete(object? sender, EventArgs e)
        {
            isLoadComplete = true;
            StateHasChanged();
        }
    }
}
