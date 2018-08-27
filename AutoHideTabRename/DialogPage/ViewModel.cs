using AutoHideTabRename.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AutoHideTabRename
{
    public class ViewModel : Observable
    {
        // オプションページが閉じたときにまるごと上書き（キャンセルでも）
        public SettingsModel Settings { get => _Settings; set => Set(ref _Settings, value); }
        private SettingsModel _Settings;

        // 開いた時にOneTimeで強制選択 選択がないとＵＩがわかりにくいかと
        // （Editに入れてAddみたいに思われそう 空の時はどうしようもないが。。
        public int Index { get; } = 0;


        public ViewModel() => Load();

        public void Load()
        {
            Settings = SerializeHelper.LoadOrDefault<SettingsModel>();
            Settings.Distinct();
        }
        public void Save()
        {
            Settings.Distinct();
            SerializeHelper.Save(Settings);
        }

        public IReadOnlyDictionary<string, string> GetNames()
        {
            return Settings.IsEnabled
                 ? Settings.Items.ToDictionary(x => x.TargetName, x => x.NewName)
                 : new Dictionary<string, string>();
        }
    }
}
