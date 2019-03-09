using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace AutoHideTabRename
{
    [DataContract]
    public class SettingsModel : Observable
    {
        // 筋悪だがDistinctの都合上PropertyChanged
        [DataMember]
        public ObservableCollection<TabNameModel> Items { get => _Items; set => Set(ref _Items, value); }
        private ObservableCollection<TabNameModel> _Items;

        [DataMember]
        public bool IsEnabled { get => _IsEnabled; set => Set(ref _IsEnabled, value); }
        private bool _IsEnabled;


        public SettingsModel() => Init();

        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Init();
        private void Init()
        {
            IsEnabled = true;
            Items = new ObservableCollection<TabNameModel>
            {
                new TabNameModel("ソリューション エクスプローラー", "ソリューション"),
                new TabNameModel("チーム エクスプローラー", "チーム"),
                new TabNameModel("サーバー エクスプローラー", "サーバー"),
                new TabNameModel("SQL Server オブジェクト エクスプローラー", "SQL Server オブジェクト"),
                new TabNameModel("ソース管理エクスプローラー", "ソース管理"),
                new TabNameModel("タスク ランナー エクスプローラー", "タスク ランナー"),
                new TabNameModel("テスト エクスプローラー", "テスト"),
                new TabNameModel("ライブ プロパティ エクスプローラー", "ライブ プロパティ"),
            };
        }

        // 重複を削除 上が優先
        public void Distinct()
        {
            var d = Items.GroupBy(x => x.TargetName).Select(x => x.First());
            Items = new ObservableCollection<TabNameModel>(d);
        }
    }
}