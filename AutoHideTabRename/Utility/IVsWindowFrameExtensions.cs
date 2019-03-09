using System;
using Microsoft.VisualStudio.Platform.WindowManagement;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AutoHideTabRename.Utility
{
    internal static class IVsWindowFrameExtensions
    {
        public static bool IsToolWindow(this IVsWindowFrame frame)
        {
            if(frame == null) throw new ArgumentNullException(nameof(frame));

            return (frame as WindowFrame)?.IsToolWindow ?? false;
        }

        public static ToolWindowView GetToolWindowView(this IVsWindowFrame frame)
        {
            if(frame == null) throw new ArgumentNullException(nameof(frame));

            return (frame as WindowFrame)?.FrameView as ToolWindowView;
        }

        public static string GetCaption(this IVsWindowFrame frame)
        {
            if(frame == null) throw new ArgumentNullException(nameof(frame));

            ThreadHelper.ThrowIfNotOnUIThread();
            frame.GetProperty((int)__VSFPROPID.VSFPROPID_Caption, out var result);
            return result as string;
        }
    }
}
