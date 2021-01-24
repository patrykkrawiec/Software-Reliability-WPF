using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Software_Reliability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Threading;

namespace Software_Reliability_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        JsonData jsonData;
        ModelInput input;
        public MainWindow()
        {
            InitializeComponent();
            File_helper.Content=" Wybierz plik z danymi w formacie JSON.Format musi być następujący: \n { \"timeValues\":[3203,3267,3160,3269,3038,3255,3106,3261,3441,3225,3201,3162]}";
            input = new ModelInput();
            JelinskiMorandaFiChart = new SeriesCollection{};
            JelinskiMorandaNChart = new SeriesCollection{};
            JelinskiMorandaETChart = new SeriesCollection{};

            SchickWolvertonFiChart = new SeriesCollection { };
            SchickWolvertonNChart = new SeriesCollection { };
            SchickWolvertonETChart = new SeriesCollection { };
            AxisMin = 0;
            AxisMax = 10;
            AxisMinSchickWolverton = 0;
            AxisMaxSchickWolverton = 10;
            AxisUnit = 1000;
            AxisStep = 1;

            var mapper = Mappers.Xy<ChartPoint>()
           .X(model => model.xvalue)
           .Y(model => model.yvalue);
            Charting.For<ChartPoint>(mapper);
            DataContext = this;
        }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        #region
        private double _axisMaxSchickWolverton;
        private double _axisMinSchickWolverton;
        public double AxisMaxSchickWolverton
        {
            get { return _axisMaxSchickWolverton; }
            set
            {
                _axisMaxSchickWolverton = value;
                OnPropertyChanged("AxisMaxSchickWolverton");
            }
        }
        public double AxisMinSchickWolverton
        {
            get { return _axisMinSchickWolverton; }
            set
            {
                _axisMinSchickWolverton = value;
                OnPropertyChanged("AxisMinSchickWolverton");
            }
        }
        #endregion

        #region JelinskiMorandaAxis
        private double _axisMax;
        private double _axisMin;
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        #endregion
        public string[] Labels { get; set; }
        #region JelinskiMorandaCharts
        private SeriesCollection _JelinskiMorandaFiChart;
        private SeriesCollection _JelinskiMorandaNChart;
        private SeriesCollection _JelinskiMorandaETChart;
        public SeriesCollection JelinskiMorandaFiChart
        {
            get
            {
                return _JelinskiMorandaFiChart;
            }
            set
            {
                _JelinskiMorandaFiChart = value;
                OnPropertyChanged("JelinskiMorandaFiChart");
            }
        }
        public SeriesCollection JelinskiMorandaNChart
        {
            get
            {
                return _JelinskiMorandaNChart;
            }
            set
            {
                _JelinskiMorandaNChart = value;
                OnPropertyChanged("JelinskiMorandaNChart");
            }
        }
        public SeriesCollection JelinskiMorandaETChart
        {
            get
            {
                return _JelinskiMorandaETChart;
            }
            set
            {
                _JelinskiMorandaETChart = value;
                OnPropertyChanged("JelinskiMorandaETChart");
            }
        }

        #endregion

        #region SchickWolvertonCharts
        private SeriesCollection _SchickWolvertonFiChart;
        private SeriesCollection _SchickWolvertonNChart;
        private SeriesCollection _SchickWolvertonETChart;

        public SeriesCollection SchickWolvertonFiChart
        {
            get
            {
                return _SchickWolvertonFiChart;
            }
            set
            {
                _SchickWolvertonFiChart = value;
                OnPropertyChanged("SchickWolvertonFiChart");
            }
        }
        public SeriesCollection SchickWolvertonNChart
        {
            get
            {
                return _SchickWolvertonNChart;
            }
            set
            {
                _SchickWolvertonNChart = value;
                OnPropertyChanged("SchickWolvertonNChart");
            }
        }
        public SeriesCollection SchickWolvertonETChart
        {
            get
            {
                return _SchickWolvertonETChart;
            }
            set
            {
                _SchickWolvertonETChart = value;
                OnPropertyChanged("SchickWolvertonETChart");
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void JelinskiMoranda_Spectrum_Solve_Button_Clicked(object sender, RoutedEventArgs e)
        {       
            
            String startValueString = JelinskiMorandaStartValue_TextBox.Text.ToString();
            double startValue = Convert.ToDouble(startValueString);

            String stopValueString = JelinskiMorandaStopValue_TextBox.Text.ToString();
            double stopValue = Convert.ToDouble(stopValueString);

            String stepValueString = JelinskiMorandaStepValue_TextBox.Text.ToString();
            double stepValue = Convert.ToDouble(stepValueString);

               
            JelinskiMorandaSpectrumSolver jelinskiMorandaSpectrumSolver;
            jelinskiMorandaSpectrumSolver = await Task.Run(() =>
            {
                jelinskiMorandaSpectrumSolver = new JelinskiMorandaSpectrumSolver(startValue, stopValue, stepValue, input);
                return jelinskiMorandaSpectrumSolver;

            });
            MessageBox.Show(jelinskiMorandaSpectrumSolver.fiPoints.Count.ToString());


            JelinskiMorandaFiChart.Clear();
            JelinskiMorandaNChart.Clear();
            JelinskiMorandaETChart.Clear();

            JelinskiMorandaFiChart.Add(new LineSeries
            {
                Title = "FI Values",

                Values = jelinskiMorandaSpectrumSolver.fiPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 5,
                PointForeground = Brushes.Gray
            });
            
            JelinskiMorandaNChart.Add(new LineSeries
            {
                Title = "N Values",

                Values = jelinskiMorandaSpectrumSolver.nPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 5,
                PointForeground = Brushes.Gray
            });

            JelinskiMorandaETChart.Add(new LineSeries
            {
                Title = "ET Values",

                Values = jelinskiMorandaSpectrumSolver.etPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 5,
                PointForeground = Brushes.Gray
            });
            
        }

        private void JelinskiMorandaSingeValueSolve_Button_Click(object sender, RoutedEventArgs e)
        {
            String value = JelinskiMorandaSingleValue_TextBox.Text.ToString();
            input.epsilon = Convert.ToDouble(value);
            JelinskiMorandaModel jelinskiMorandaModel_singleValue = new JelinskiMorandaModel(input);
            JelinskiMorandaSingleValueResult_TextBox.Text += "ET = " + jelinskiMorandaModel_singleValue.ET + " N = " + jelinskiMorandaModel_singleValue.N + " FI = " +  jelinskiMorandaModel_singleValue.fi+"\n";
        }

        private void SchickWolvertonSingeValueSolve_Button_Click(object sender, RoutedEventArgs e)
        {
            String value = SchickWolvertonSingleValue_TextBox.Text.ToString();
            input.epsilon = Convert.ToDouble(value);
            SchickWolvertonModel SchickWolvertonModel_singleValue = new SchickWolvertonModel(input);
            SchickWolvertonSingleValueResult_TextBox.Text += "ET = " + SchickWolvertonModel_singleValue.ET + " N = " + SchickWolvertonModel_singleValue.N + " FI = " + SchickWolvertonModel_singleValue.fi + "\n";

        }

        private async void SchickWolverton_Spectrum_Solve_Button_Click(object sender, RoutedEventArgs e)
        {
            String startValueString = SchickWolvertonStartValue_TextBox.Text.ToString();
            double startValue = Convert.ToDouble(startValueString);

            String stopValueString = SchickWolvertonStopValue_TextBox.Text.ToString();
            double stopValue = Convert.ToDouble(stopValueString);

            String stepValueString = SchickWolvertonStepValue_TextBox.Text.ToString();
            double stepValue = Convert.ToDouble(stepValueString);


            SchickWolvertonSpectrumSolver schickWolvertonSpectrumSolver;
            schickWolvertonSpectrumSolver = await Task.Run(() =>
            {
                schickWolvertonSpectrumSolver = new SchickWolvertonSpectrumSolver(startValue, stopValue, stepValue, input);
                return schickWolvertonSpectrumSolver;

            });
            MessageBox.Show(schickWolvertonSpectrumSolver.fiPoints.Count.ToString());
            SchickWolvertonFiChart.Clear();
            SchickWolvertonNChart.Clear();
            SchickWolvertonETChart.Clear();
            
            SchickWolvertonFiChart.Add(new LineSeries
            {
                Title = "FI Values",
                
                Values = schickWolvertonSpectrumSolver.fiPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = null,
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            SchickWolvertonNChart.Add(new LineSeries
            {
                Title = "N Values",

                Values = schickWolvertonSpectrumSolver.nPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = null,
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            SchickWolvertonETChart.Add(new LineSeries
            {
                Title = "ET Values",

                Values = schickWolvertonSpectrumSolver.etPoints.AsChartValues(),
                LineSmoothness = 0, 
                PointGeometry = null,
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            }); 



        }

        private void OpenDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "JSON Files (*.json)|*.json" }; ;
        
            Nullable<bool> result =     openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                jsonData = new JsonData();
                if (jsonData.ReadFile(filename)==-1)
                {

                    MessageBox.Show("Nieprawidłowy plik z danymi", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error) ;
                }
                else
                {   
                    JMModel_Tab.IsEnabled = true;
                    SWModel_Tab.IsEnabled = true;
                    OpenDialog_Tab.IsEnabled = false;
                    input = jsonData.getInputFile();
                    MessageBox.Show("Sukces ! \n Załadowano "+ input.times.Count +" wartości","Informacja",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
        }
    }
}
