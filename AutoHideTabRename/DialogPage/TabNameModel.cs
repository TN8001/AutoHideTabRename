using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AutoHideTabRename.Utility;

namespace AutoHideTabRename
{
    [DataContract]
    public class TabNameModel : Observable
    {
        [DataMember]
        public string TargetName { get => _TargetName; set => SetNotNullOrEmpty(ref _TargetName, value); }
        private string _TargetName;

        [DataMember]
        public string NewName { get => _NewName; set => SetNotNullOrEmpty(ref _NewName, value); }
        private string _NewName;


        public TabNameModel(string targetName, string newName)
            => (TargetName, NewName) = (targetName, newName);


        private void SetNotNullOrEmpty(ref string storage, string value, [CallerMemberName] string propertyName = null)
        {
            if(value.IsNullOrEmpty()) throw new ArgumentNullException(nameof(propertyName));
            Set(ref storage, value, propertyName);
        }

        public override string ToString() => $"{TargetName} -> {NewName}";
    }
}