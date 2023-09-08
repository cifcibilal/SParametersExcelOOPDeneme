using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SParametersExcelOOPDeneme
{
    public class ExcelManager
    {
        OpenFileDialog fileDialog;
        public ExcelManager()
        {
            fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.Filter = "Excel Files|*.xlsx";
        }
        /**
        * @brief Excel dosyası seçme iletişim kutusunu gösterir ve seçilen dosyanın yolunu döndürür.
        * 
        * Bu fonksiyon, kullanıcıya bir Excel dosyası seçme imkanı sunar. Kullanıcı bir dosya seçtiğinde,
        * seçilen dosyanın yolunu döndürür. Eğer kullanıcı bir dosya seçmez veya işlemi iptal ederse,
        * null değeri döner.
        * 
        * @return Seçilen Excel dosyasının yolu (filePath) veya null.
        */
        public string OpenExcelFileDialog()
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
        /**
        * @brief Verilen Excel dosyasını okuyarak içeriği bir DataTable'a dönüştürür.
        * 
        * Bu metod, belirtilen Excel dosyasını açar ve içeriğini bir DataTable'a aktarır. İlgili sayfa (worksheet) seçeneği ile
        * belirtilen sayfa veya varsayılan olarak ilk sayfa kullanılır. Excel dosyasının her satırı bir DataRow'a ve her hücre değeri
        * bir DataColumn'a dönüştürülür. Sayfanın başlıkları (column headers) DataColumn adları olarak kullanılır.
        * 
        * @param filePath:string, Okunacak Excel dosyasının yolunu içeren bir dize.
        * @param selectedSheet:string, Okunacak sayfanın adını içeren bir dize. Varsayılan olarak ilk sayfa ("0") kullanılır.
        * 
        * @return: Excel dosyasının içeriğini temsil eden DataTable.
        */
        public DataTable ReaderExcelFile(string filePath, string selectedSheet = "0")
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet excelWorksheet;
                    if (selectedSheet.Equals("0"))
                    {
                        excelWorksheet = package.Workbook.Worksheets[0];
                    }
                    else
                    {
                        excelWorksheet = package.Workbook.Worksheets[selectedSheet];
                    }

                    int startRow = 1;
                    int endRow = excelWorksheet.Dimension.Rows;
                    int endColum = excelWorksheet.Dimension.Columns;
                    //Excel dosyasına göre endRow & endColumn değerleri kontrol edilmelidir.
                    //UIHelper sınıfı method çağrılarıda buna göre düzenlenmelidir !
                    endRow = 604;
                    endColum = 6;
                    string columnHeader;
                    for (int columnIndex = 1; columnIndex <= endColum; columnIndex++)
                    {
                        if (excelWorksheet.Cells[startRow, columnIndex].Text == "")
                        {
                            if (excelWorksheet.Cells[startRow, columnIndex].Text == "" && excelWorksheet.Cells[startRow, 1].Text == "")
                                columnHeader = excelWorksheet.Cells[startRow + 2, columnIndex].Text;
                            else
                                columnHeader = excelWorksheet.Cells[startRow, columnIndex].Text;
                        }
                        else
                        {
                            columnHeader = excelWorksheet.Cells[startRow, columnIndex].Text;
                        }


                        dataTable.Columns.Add(columnHeader);
                    }
                    for (var rowNumber = (startRow + 1); rowNumber <= endRow; rowNumber++)
                    {
                        var row = excelWorksheet.Cells[rowNumber, 1, rowNumber, endColum];
                        var newRow = dataTable.NewRow();
                        foreach (var cell in row)
                        {
                            if (double.TryParse(cell.Text, out double numericValue))
                            {
                                newRow[cell.Start.Column - 1] = numericValue;
                            }
                            else
                            {
                                newRow[cell.Start.Column - 1] = cell.Text;
                            }
                        }
                        dataTable.Rows.Add(newRow);
                    }
                        //dataTable.Columns.RemoveAt(1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ReaderExcelFile içerisinde hata oluştu: " + e.Message);
            }
            return dataTable;
        }
        /**
        * @brief Verilen Excel dosyasındaki tüm sayfa adlarını döndürür.
        * 
        * @param filePath:string, Sayfa adlarını almak istediğiniz Excel dosyasının yolunu içeren bir dize.
        * @return: Excel dosyasındaki tüm sayfa adlarını içeren bir liste.
        */
        public List<string> GetSheetNames(string filePath)
        {
            List<string> sheetNames = new List<string>();
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    if (package != null)
                    {
                        foreach (var sheet in package.Workbook.Worksheets)//hatalı
                        {
                            sheetNames.Add(sheet.Name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("GetSheetNames içerisinde hata oluştu: " + e.Message);
            }
            return sheetNames;
        }
    }
}