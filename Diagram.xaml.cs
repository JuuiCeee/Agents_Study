using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UP_11
{
    /// <summary>
    /// Логика взаимодействия для Diagram.xaml
    /// </summary>
    public partial class Diagram : Window
    {
        private static List<Agents> _paymentsDiagram;
        public Diagram(List<Agents> paymentsUsers)
        {
            InitializeComponent();
            _paymentsDiagram = paymentsUsers;
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("PaymentsDiagram")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);
            CbChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (CbChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
                var categoriesList = NiceRustleEntities.GetContext().Agents.ToList();
                foreach (var category in categoriesList)
                {
                    if (category.YearSales != 0)
                        currentSeries.Points.AddXY(category.NameAgent, category.YearSales);
                }
            }
        }

        private void BtnCloseDiagram_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
