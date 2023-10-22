using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PEOpener.Infrastuture;

namespace PEOpener.Layout
{
    public partial class Navbar
    {
        [Inject]
        private IJSRuntime JSModule { get; set; }

        InputFile filePicker;

        private async Task OpenFileDialog(MouseEventArgs e)
        {
            await JSModule.InvokeAsync<object>("triggerClick", filePicker.Element);
        }

        private async void LoadFiles(InputFileChangeEventArgs e)
        {
            var file = e.GetMultipleFiles(1)[0];
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    await reader.BaseStream.CopyToAsync(memstream);
                    HexFile.LoadFile(file.Name, memstream.ToArray());
                    await JSModule.InvokeVoidAsync("createHexEditor", "hexEditor", HexFile.HexBytes);
                    SideBar._instatace.build(HexFile.getSections());
                }
            }
        }
    }
}
