using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PEOpener.Components;
using PEOpener.Infrastuture;
using System.Text;

namespace PEOpener.Layout
{
    public partial class SideBar : ComponentBase
    {
        [Inject]
        private IJSRuntime JSModule { get; set; }

        private List<PeFileTree> treeSystem = new List<PeFileTree>();
        public static SideBar _instatace;

        protected override void OnInitialized()
        {
            if (_instatace is null) _instatace = this;
        }
        public async void OnSelectedItemChanged(PeFileTree selectedItem)
        {
            if (selectedItem.Name == HexFile.FileName)
            {
                await JSModule.InvokeVoidAsync("createHexEditor", "hexEditor", HexFile.HexBytes);
                return;
            }
            else if (selectedItem.Name == "Herader")
            {
                ShowData._instance.UpdateData(HexFile.FileHeader());
            }
            else if (selectedItem.Name == "Optional Header")
            {
                ShowData._instance.UpdateData(HexFile.OptionalHeader());
            }
            else if (selectedItem.Name.StartsWith("."))
            {
                var data = HexFile.GetSectionsByName(selectedItem.Name);
                ShowData._instance.UpdateData(data);
                var bytesString = data.Find(x => x.Key == "Bytes");
                var bytes = Encoding.ASCII.GetBytes(bytesString.Value);
                await JSModule.InvokeVoidAsync("createHexEditor", "hexEditor", bytes);

            }
        }
        public void build(List<string> sections)
        {
            treeSystem = new List<PeFileTree>();

            var top = new PeFileTree(HexFile.FileName, BootstrapIcon.Folder);
            treeSystem.Add(top);

            var Herader = new PeFileTree("Herader", BootstrapIcon.Envelope);
            var OptionalHeader = new PeFileTree("Optional Header", BootstrapIcon.Envelope);

            top.SubTree = new List<PeFileTree>();
            top.SubTree.Add(Herader);
            top.SubTree.Add(OptionalHeader);

            if (sections.Count > 0)
            {
                var Section = new PeFileTree("Sections", BootstrapIcon.Envelope);
                Section.SubTree = new List<PeFileTree>();
                foreach (var s in sections)
                {
                    Section.SubTree.Add(new PeFileTree(s, BootstrapIcon.Envelope));
                }
                top.SubTree.Add(Section);
            }

            OnSelectedItemChanged(new PeFileTree(HexFile.FileName, BootstrapIcon.Folder));
            StateHasChanged();
        }
    }
}
