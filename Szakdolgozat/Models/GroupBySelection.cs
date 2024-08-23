using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Models
{
    public class GroupBySelection : INotifyPropertyChanged
    {
        private string name;
        private SolidColorBrush color;
        private bool isSelected;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public SolidColorBrush Color
        {
            get => color;
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged();
                    Mediator.NotifySetLineSeriesNewColor(Name, Color);
                }
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
                Mediator.NotifyHideOrShowLineSeries(Name,IsSelected);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GroupBySelection(string name, bool isSelected, SolidColorBrush color)
        {
            Name = name;
            IsSelected = isSelected;
            Color = color;
        }
    }
}
