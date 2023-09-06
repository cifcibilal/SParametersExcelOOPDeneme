using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SParametersExcelOOPDeneme
{
    public class ChartProcessor
    {
        private Chart chart;
        private TextBox textBoxMin;
        private TextBox textBoxMax;
        public ChartProcessor(Chart chart, TextBox textBoxMin, TextBox textBoxMax)
        {
            this.chart = chart;
            this.textBoxMin = textBoxMin;
            this.textBoxMax = textBoxMax;
        }
        public ChartProcessor()
        {
                
        }
        public void ShowDataOnChart(DataTable dataTable)
        {
            chart.Series.Clear();
            SetChatAxisProperties();

            string[] seriesNames = { "S11 - dB", "S21 - dB", "S12 - dB", "S22 - dB" };
            string[] columnName = { "Column1", "Column2", "Column3", "Column4", "Column5", "Column6" };

            Series series1 = new Series("S11 - dB");
            Series series2 = new Series("S21 - dB");
            Series series3 = new Series("S12 - dB");
            Series series4 = new Series("S22 - dB");
            chart.Series.Add(series2);
            chart.Series.Add(series3);
            chart.Series.Add(series4);
            chart.Series.Add(series1);
            series1.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series1.BorderWidth = 4; // Çizgi kalınlığı
            series2.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series2.BorderWidth = 4; // Çizgi kalınlığı
            series3.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series3.BorderWidth = 4; // Çizgi kalınlığı
            series4.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series4.BorderWidth = 4; // Çizgi kalınlığı

            //foreach (string seriesName in seriesNames)
            //{
            //    Series series = CreateChartSeries(seriesName);
            //    chart.Series.Add(series);
            //}

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                double xDegeri = Convert.ToDouble(dataTable.Rows[i]["Column1"]);
                for (int j = 0; j < 4; j++)
                {
                    double yDegeri = 0;
                    if (dataTable.Rows[i][columnName[j + 2]] != DBNull.Value)
                    {
                        if (double.TryParse(dataTable.Rows[i][columnName[j + 2]].ToString(), out double parsedValue)) 
                        {
                            yDegeri = parsedValue;
                        }
                        else
                        {

                        }
                    }
                    switch (j)
                    {
                        case 0:
                            series1.Points.AddXY(xDegeri, yDegeri);
                            break;
                        case 1:
                            series2.Points.AddXY(xDegeri, yDegeri);
                            break;
                        case 2:
                            series3.Points.AddXY(xDegeri, yDegeri);
                            break;
                        case 3:
                            series4.Points.AddXY(xDegeri, yDegeri);
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        private void SetChatAxisProperties()
        {
            chart.ChartAreas[0].AxisX.Title = "MHz";
            chart.ChartAreas[0].AxisX.Minimum = int.Parse(textBoxMin.Text);
            chart.ChartAreas[0].AxisX.Maximum = int.Parse(textBoxMax.Text);
            chart.ChartAreas[0].AxisY.Maximum = 20;
            chart.ChartAreas[0].AxisY.Minimum = -40;
            chart.ChartAreas[0].AxisY.Title = "dB";
        }
        private Series CreateChartSeries(string seriesNames)
        {
            Series series = new Series(seriesNames);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 4;
            return series;
        }
        public void SaveGrafik(ExcelPackage package, Chart chart, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets[sheetName];
            int rowIndex = 1;
            int colIndex = 10;

            using (MemoryStream ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                var picture = worksheet.Drawings.AddPicture("GrafikResmi", ms);
                picture.SetPosition(rowIndex, 0, colIndex, 0);
                picture.SetSize(750,350);
            }
        }
    }
}
