using System;
using System.Windows.Data;
using Microsoft.VisualStudio.Platform.WindowManagement;

namespace AutoHideTabRename.Utility
{
    internal static class ToolWindowViewExtensions
    {
        public static string GetShortTitle(this ToolWindowView view)
        {
            if(view == null) throw new ArgumentNullException(nameof(view));

            var wft = view.Title as WindowFrameTitle;
            return wft?.ShortTitle;
        }

        public static void SetShortTitle(this ToolWindowView view, string title)
        {
            if(view == null) throw new ArgumentNullException(nameof(view));
            if(title.IsNullOrEmpty()) throw new ArgumentNullException(nameof(title));

            if(view.Title is WindowFrameTitle wft)
            {
                BindingOperations.ClearBinding(wft, WindowFrameTitle.ShortTitleProperty);
                wft.ShortTitle = title;
            }
        }
    }
}
