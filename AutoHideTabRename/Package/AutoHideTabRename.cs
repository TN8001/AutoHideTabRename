using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace AutoHideTabRename
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(SettingsPage), "AutoHide Tabs Rename", "General", 0, 0, true)]
#pragma warning disable VSSDK004 // Use BackgroundLoad flag in ProvideAutoLoad attribute for asynchronous auto load.
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
#pragma warning restore VSSDK004
    public sealed partial class AutoHideTabRename : AsyncPackage
    {
        public const string PackageGuidString = "465005c4-88e8-410b-a72c-4b37ac15bec6";

        private SettingsPage page;
        private TabMonitor monitor;
        private IVsUIShell7 uiShell7;
        private uint wfeCookie;
        private DTEEvents dteEvents;

        protected override async Task InitializeAsync(CancellationToken token, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(token);

            page = GetDialogPage(typeof(SettingsPage)) as SettingsPage;
            page.NameChanged += NameChanged;

            monitor = new TabMonitor(page.GetNames());

            uiShell7 = await GetServiceAsync(typeof(SVsUIShell)) as IVsUIShell7;
            Assumes.Present(uiShell7);
            wfeCookie = uiShell7.AdviseWindowFrameEvents(monitor);

            var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            Assumes.Present(dte);
            dteEvents = dte.Events.DTEEvents;
            dteEvents.ModeChanged += DteEvents_ModeChanged;
        }

        private void DteEvents_ModeChanged(vsIDEMode LastMode)
        {
            Debug.WriteLine("DteEvents_ModeChanged");

            monitor.ForceRename();
        }

        private void NameChanged(object sender, TabNameEventArgs e)
        {
            Debug.WriteLine("NameChanged");

            monitor.Rename(e.Names);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ThreadHelper.ThrowIfNotOnUIThread();

            if(disposing && uiShell7 != null)
            {
                page.NameChanged -= NameChanged;
                uiShell7.UnadviseWindowFrameEvents(wfeCookie);
                dteEvents.ModeChanged -= DteEvents_ModeChanged;
            }
        }
    }
}
