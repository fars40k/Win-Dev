using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Win_Dev.Assets.UserControls
{
    /// <summary>
    /// Interaction logic for LimitedTextBox.xaml
    /// </summary>
    public partial class LimitedTextBox : UserControl
    {
        public static readonly DependencyProperty LimitedTextProperty = 
            DependencyProperty.Register("LimitedText", 
                                        typeof(string), 
                                        typeof(LimitedTextBox),
                                        new FrameworkPropertyMetadata());       

        public string LimitedText
        {
            get { return (string)GetValue(LimitedTextProperty); }
            set { SetValue(LimitedTextProperty, value); }
        }

        public string Title { get; set; }

        public int MaxLength { get; set; }

        public LimitedTextBox()
        {
            InitializeComponent();           
        }
    }
}
