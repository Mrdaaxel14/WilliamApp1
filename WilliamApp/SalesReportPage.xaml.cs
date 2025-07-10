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
            string period = PeriodPicker.SelectedItem?.ToString() ?? "Día";
            string date = DatePicker.Date.ToString("d");
            // Simulación de datos (reemplazar con lógica real)
            double total = 0;
            if (period == "Día") total = 500.50;
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
            // Aquí se podría agregar lógica para cargar datos reales
        }
    }
}