using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace AutoHideTabRename
{
    public partial class SettingsControl : UserControl
    {
        private IList items => ((ViewModel)DataContext).Settings.Items;


        public SettingsControl() => InitializeComponent();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            items.Add(new TabNameModel("Target Name", "New Name"));
            listBox.SelectedIndex = items.Count - 1;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem is TabNameModel tab)
            {
                var i = listBox.SelectedIndex;
                items.Remove(tab);
                listBox.SelectedIndex = i > items.Count - 1 ? items.Count - 1 : i;
            }
        }
    }
}
