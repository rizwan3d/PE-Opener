using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;

namespace PEOpener.Layout
{
    public partial class SideBar
    {
        public class PeFileTree
        {
            public PeFileTree(string name, IconBase icon = null, string message = null, ThemeColor? severity = null)
            {
                Name = name;
                Icon = icon;
                Message = message;
                Severity = severity;
            }

            public string Name { get; }

            public IconBase Icon { get; set; }

            public string Message { get; set; }

            public ThemeColor? Severity { get; set; }

            public List<PeFileTree> SubTree { get; set; }
        }
    }
}
