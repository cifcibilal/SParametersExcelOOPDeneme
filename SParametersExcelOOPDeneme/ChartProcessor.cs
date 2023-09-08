using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private TextBox textBoxdB;
        private Button btnEkle;
        public ChartProcessor(Chart chart, TextBox textBoxMin, TextBox textBoxMax, TextBox textBoxdB)
        {
            this.chart = chart;
            this.textBoxMin = textBoxMin;
            this.textBoxMax = textBoxMax;
            this.textBoxdB = textBoxdB;

        }
        public ChartProcessor(Chart chart, TextBox textBoxMin, TextBox textBoxMax)
        {
            this.chart = chart;
            this.textBoxMin = textBoxMin;
            this.textBoxMax = textBoxMax;
        }
        public ChartProcessor()
        {

        }
        /**
         * @brief Veri tablosundaki verileri grafik olarak gösterir.
         * 
         * @param dataTable:DataTable, Grafik üzerinde gösterilecek veri tablosu.
         * 
         * @return void
         */
        public void ShowDataOnChart(DataTable dataTable)
        {
            chart.Series.Clear();
            SetChatAxisProperties(dataTable);

            string[] seriesNames = { "S11 - dB", "S21 - dB", "S12 - dB", "S22 - dB" };
            string[] columnName = { "MHz", "", "S11 - dB", "S21 - dB", "S12 - dB", "S22 - dB" };

            Series series1 = new Series("S11 - dB");
            Series series2 = new Series("S21 - dB");
            Series series3 = new Series("S12 - dB");
            Series series4 = new Series("S22 - dB");
            chart.Series.Add(series2);
            chart.Series.Add(series3);
            chart.Series.Add(series4);
            chart.Series.Add(series1);
            series1.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series1.BorderWidth = 2; // Çizgi kalınlığı
            series2.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series2.BorderWidth = 2; // Çizgi kalınlığı
            series3.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series3.BorderWidth = 2; // Çizgi kalınlığı
            series4.ChartType = SeriesChartType.Line; // Çizgi grafiği
            series4.BorderWidth = 2; // Çizgi kalınlığı

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                double xDegeri = Convert.ToDouble(dataTable.Rows[i][dataTable.Columns[0].ColumnName]);
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
        /**
         * @brief Grafik eksen özelliklerini belirler.
         * 
         * @param dataTable:DataTable, Grafik üzerinde gösterilecek veri tablosu.
         * 
         * @return void
         */
        private void SetChatAxisProperties(DataTable dataTable)
        {
            string[] columnName = { "MHz", "", "S11 - dB", "S21 - dB", "S12 - dB", "S22 - dB" };

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (DataRow row in dataTable.Rows)
            {
                double xDegeri = Convert.ToDouble(row[dataTable.Columns[0].ColumnName]);

                if (xDegeri < minX)
                    minX = xDegeri;

                if (xDegeri > maxX)
                    maxX = xDegeri;

                for (int j = 0; j < 4; j++)
                {
                    if (row[columnName[j + 2]] != DBNull.Value)
                    {
                        if (double.TryParse(row[columnName[j + 2]].ToString(), out double parsedValue))
                        {
                            double yDegeri = parsedValue;

                            if (yDegeri < minY)
                                minY = yDegeri;

                            if (yDegeri > maxY)
                                maxY = yDegeri;
                        }
                    }
                }
            }

            chart.ChartAreas[0].AxisX.Title = "MHz";
            chart.ChartAreas[0].AxisX.Minimum = int.Parse(textBoxMin.Text) - 5;
            chart.ChartAreas[0].AxisX.Maximum = int.Parse(textBoxMax.Text) + 5;
            chart.ChartAreas[0].AxisY.Maximum = maxY + 5;
            chart.ChartAreas[0].AxisY.Minimum = minY - 5;
            chart.ChartAreas[0].AxisY.Title = "dB";
        }
        /**
         * @brief Yeni bir grafik serisi oluşturur.
         * 
         * @param seriesNames:string, Oluşturulan serinin adı.
         * @return: Oluşturulan grafik serisi.
         */
        private Series CreateChartSeries(string seriesNames)
        {
            Series series = new Series(seriesNames);
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 4;
            return series;
        }
        /**
         * @brief Grafik görüntüsünü belirtilen Excel sayfasına kaydeder.
         * 
         * @param package:ExcelPackage, Kaydedilecek Excel paketi.
         * @param chart:Chart, Kaydedilecek grafik nesnesi.
         * @param sheetName:string, Verinin kaydedileceği sayfanın adı.
         */
        public void SaveGrafik(ExcelPackage package, Chart chart, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets[sheetName];
            int rowIndex = 1;
            int colIndex = 7;

            using (MemoryStream ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                var picture = worksheet.Drawings.AddPicture("GrafikResmi", ms);
                picture.SetPosition(rowIndex, 0, colIndex, 0);
                picture.SetSize(750, 350);
            }
        }
        /**
         * @brief Grafiğe limitline çizgisi ekler.
         * 
         * @param limitlineName:string, Eklenecek limitline adı.
         * 
         * @return void.
         */
        public void LimitLineEkle(string limitLineName)
        {
            double x1 = double.Parse(textBoxMin.Text);
            double x2 = double.Parse(textBoxMax.Text);
            double y = double.Parse(textBoxdB.Text);

            Series existingSeries = chart.Series.FindByName(limitLineName);

            if (existingSeries != null)
            {
                chart.Series.Remove(existingSeries);
            }

            Series series = new Series();
            series.Name = limitLineName;
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 1;

            if (series.Name.Equals("S11_Limitline"))
            {
                series.Color = Color.Black;
            }
            else if (series.Name.Equals("S21_Limitline"))
            {
                series.Color = Color.MidnightBlue;
            }
            else if (series.Name.Equals("S12_Limitline"))
            {
                series.Color = Color.DarkRed;
            }
            else if (series.Name.Equals("S22_Limitline"))
            {
                series.Color = Color.DarkGreen;
            }
            series.BorderDashStyle = ChartDashStyle.Dash;
            series.Points.AddXY(x1, y);
            series.Points.AddXY(x2, y);

            chart.Series.Add(series);

            chart.Invalidate();
        }
    }
}
