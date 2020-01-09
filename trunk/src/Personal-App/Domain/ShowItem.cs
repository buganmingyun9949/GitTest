using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Personal_App.ViewModel;

namespace Personal_App.Domain
{
    public class ShowItem: MainLoginVM
    {
        private string _name;
        private object _content;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;
        private Thickness _marginRequirement = new Thickness(16);

        public IEnumerable<DocumentationLink> Documentation { get; }
        public ShowItem(string name, object content, IEnumerable<DocumentationLink> documentation)
        {
            _name = name;
            Content = content;
            Documentation = documentation;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set
            {
                _horizontalScrollBarVisibilityRequirement = value;
                RaisePropertyChanged("HorizontalScrollBarVisibilityRequirement");
            }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set
            {
                _verticalScrollBarVisibilityRequirement = value;
                RaisePropertyChanged("VerticalScrollBarVisibilityRequirement");
            }
        }

        public Thickness MarginRequirement
        {
            get { return _marginRequirement; }
            set
            {
                _marginRequirement = value;
                RaisePropertyChanged("MarginRequirement");
            }
        }
        
    }
}
