using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace ColorList
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            var data = new System.Collections.ObjectModel.ObservableCollection<ColorItem>();

            var properties = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            Debug.WriteLine($"properties: num={properties}");
            foreach (var info in properties)
            {
                var color = (Color)info.GetValue(null, null);
                var item = new ColorItem()
                {
                    Name = info.Name,
                    Color = color,
                    Brush = new SolidColorBrush(color),
                };
                data.Add(item);
            }

            ColorGrid.ItemsSource = data;
        }

        void ColorGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            var no = e.Row.GetIndex() + 1;
            e.Row.Header = no.ToString();
            Debug.WriteLine($"row={e.Row.Header.ToString()}");
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }
    }

    internal class ColorItem
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public SolidColorBrush Brush { get; set; }

        public uint ARGB
        {
            get { return (uint)(Color.A << 24 | Color.R << 16 | Color.G << 8 | Color.B); }
        }

        public string ARGBText
        {
            get { return string.Format("#{0:X08}", ARGB); }
        }
    }
}
