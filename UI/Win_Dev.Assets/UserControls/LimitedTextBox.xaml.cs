using System.Windows;
using System.Windows.Controls;

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
