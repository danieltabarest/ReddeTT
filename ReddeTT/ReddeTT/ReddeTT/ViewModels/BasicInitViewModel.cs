using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ReddeTT.Annotations;
using ReddeTT.Models;

namespace ReddeTT.ViewModels
{
    public class BasicInitViewModel : INotifyPropertyChanged
    {
        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(nameof(User));}
        }
        public List<string> Interests { get; set; }

        private string _selectedInterest;
        public string SelectedInterest
        {
            get { return _selectedInterest; }
            set {
                _selectedInterest = value;
                OnPropertyChanged(nameof(SelectedInterest));
            }
        }

        public BasicInitViewModel()
        {
            User = App.ReddeTT.CurrentUser;
            Interests = new List<string>() {"Women", "Men", "Everyone", "BFF"};
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
