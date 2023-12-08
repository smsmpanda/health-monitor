using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;

namespace HealthMonitor.UserControls
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
            s1x.Separator.StrokeThickness = 0;
            s1y.Separator.StrokeThickness = 0;

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "正常",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "故障",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "异常",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
            };

            SeriesCollection1 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "正常",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "故障",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "异常",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection1 { get; set; }
    }
}
