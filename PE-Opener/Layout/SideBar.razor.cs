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
        private List<PeFileTree> treeSystem = new List<PeFileTree>();

        public static EventHandler<SideBarOnSectionChangeEventArgs> OnSectionChange = delegate { };

        protected override void OnInitialized()
        {
            HexFile.OnFileLoadComplete += HexFile_OnFileLoadComplete;
        }

        private void HexFile_OnFileLoadComplete(object? sender, EventArgs e)
        {
            build(HexFile.getSections());
        }

        public async void OnSelectedItemChanged(PeFileTree selectedItem)
        {
            if (selectedItem.Name == HexFile.FileName)
            {
                OnSectionChange?.Invoke(this, new SideBarOnSectionChangeEventArgs(HexFile.HexBytes));
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
                OnSectionChange?.Invoke(this, new SideBarOnSectionChangeEventArgs(bytes));
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
