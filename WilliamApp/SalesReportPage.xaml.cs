using Microsoft.Maui.Controls;

namespace WilliamApp
{
    public partial class SalesReportPage : ContentPage
    {
        public SalesReportPage()
        {
            InitializeComponent();
        }

        private void OnGenerateReportClicked(object sender, EventArgs e)
        {
            string period = PeriodPicker.SelectedItem?.ToString() ?? "D�a";
            string date = DatePicker.Date.ToString("d");
            // Simulaci�n de datos (reemplazar con l�gica real)
            double total = 0;
            if (period == "D�a") total = 500.50;
            else if (period == "Semana") total = 2500.75;
            else if (period == "Mes") total = 12000.25;

            TotalLabel.Text = $"Total Ventas: ${total:F2}";
            ReportContainer.Children.Clear();
            ReportContainer.Children.Add(new Label
            {
                Text = $"Reporte para {period} - {date}",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            });
            // Aqu� se podr�a agregar l�gica para cargar datos reales
        }
    }
}