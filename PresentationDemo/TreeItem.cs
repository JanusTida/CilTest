using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationDemo {
    class TreeItem:INotifyPropertyChanged {

        private bool _isChecked;
        public bool IsChecked {
            get => _isChecked;
            set {
                if(_isChecked == value) {
                    return;
                }

                _isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private string _text;
        public string Text {
            get => _text;
            set {
                if (_text == value) {
                    return;
                }

                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public ObservableCollection<TreeItem> Children { get; } = new ObservableCollection<TreeItem>();
        
    }

    
}
