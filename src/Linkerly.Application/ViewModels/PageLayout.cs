using Microsoft.AspNetCore.Components;

namespace Linkerly.Application.ViewModels;

public class PageLayout : LayoutComponentBase
{
    public bool IsSidebarVisible { get; set; }

    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }
}