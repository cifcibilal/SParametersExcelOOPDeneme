using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace SParametersExcelOOPDeneme
{
    public class DataFilter
    {
        public DataTable FilterByMHz(DataTable originalTable, double minMHz, double maxMHz)
        {
            DataTable filteredTable = originalTable.Clone();

            string mhzColumnName = "Column1";
            DataColumn mhzColumn = originalTable.Columns[mhzColumnName];

            if (mhzColumn == null)
            {
                throw new ArgumentException("Sütun başlığı 'Column1' bulunamadı.");
            }

            foreach (DataRow row in originalTable.Rows)
            {
                if (mhzColumn.DataType == typeof(double))
                {
                    double MHz = Convert.ToDouble(row[mhzColumn]);
                    if (MHz >= minMHz && MHz <= maxMHz)
                    {
                        filteredTable.ImportRow(row);
                    }
                }
                else if (mhzColumn.DataType == typeof(string))
                {
                    string mhzValue = row[mhzColumn].ToString();
                    if (double.TryParse(mhzValue, out double MHz))
                    {
                        if (MHz >= minMHz && MHz <= maxMHz)
                        {
                            filteredTable.ImportRow(row);
                        }
                    }
                }
            }
            return filteredTable;
        }
        public void SaveFilteredData(DataTable originalData,string saveFilePath, string saveSheetName, double minMHz, double maxMHz)
        {
            DataTable filteredData = FilterByMHz(originalData,minMHz,maxMHz);

            using (var package = new ExcelPackage(new FileInfo(saveFilePath)))
            {
                ExcelWorksheet exitisingWorkSheet = package.Workbook.Worksheets[saveSheetName];

                if (exitisingWorkSheet != null)
                {
                    DialogResult result = MessageBox.Show("Girilen sayfa adında bir sayfa zaten var. Verileri üzerine yazmak istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        package.Workbook.Worksheets.Delete(exitisingWorkSheet);                      
                        SaveFilteredDataToWorksheet(package,filteredData,saveSheetName);
                        package.Save();
                        MessageBox.Show("Sorgulanmış veriler sayfaya kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("veriler sayfaya kaydedilmedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    SaveFilteredDataToWorksheet(package,filteredData,saveSheetName);
                    package.Save();
                    MessageBox.Show("Sorgulanmış veriler sayfaya kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        }
        private void SaveFilteredDataToWorksheet(ExcelPackage package, DataTable filteredData, string sheetName)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName); // Yeni bir sayfa oluştur

            // Başlık satırını ekle
            for (int col = 0; col < 6; col++)
            {
                //worksheet.Cells[1, col + 1].Value = filteredData.Columns[col].ColumnName;
                worksheet.Cells[1, 1].Value = "MHz";
                //worksheet.Cells[1, 2].Value = "";
                worksheet.Cells[1, 2].Value = "S11 - dB";
                worksheet.Cells[1, 3].Value = "S21 - dB";
                worksheet.Cells[1, 4].Value = "S12 - dB";
                worksheet.Cells[1, 5].Value = "S22 - dB";
            }
            // Verileri ekle
            for (int row = 0; row < filteredData.Rows.Count; row++)
            {
                for (int col = 0; col < filteredData.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = Convert.ToDouble(filteredData.Rows[row][col]);
                }
            }
            worksheet.Column(2).Hidden = true;
        }
    }
}
