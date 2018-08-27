using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace AutoHideTabRename
{
    public delegate void TabNameEventHandler(object sender, TabNameEventArgs e);

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("E294381C-B0B4-472E-8267-6D9F819F1F58")]
    public sealed class SettingsPage : UIElementDialogPage
    {
        public event TabNameEventHandler NameChanged;

        protected override UIElement Child => view;

        private readonly SettingsControl view = new SettingsControl();
        private readonly ViewModel vm = new ViewModel();

        private bool saved = true;


        public SettingsPage() => view.DataContext = vm;

        public IReadOnlyDictionary<string, string> GetNames() => vm.GetNames();

        public override void LoadSettingsFromStorage()
        {
            Debug.WriteLine("LoadSettingsFromStorage");

            vm.Load();
            base.LoadSettingsFromStorage();

            if(saved)
            {
                NameChanged?.Invoke(this, new TabNameEventArgs(GetNames()));
                saved = false;
            }
        }
        public override void SaveSettingsToStorage()
        {
            Debug.WriteLine("SaveSettingsToStorage");

            saved = true;
            vm.Save();

            base.SaveSettingsToStorage();
        }
    }
}
