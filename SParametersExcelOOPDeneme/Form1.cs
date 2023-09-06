using System;
using System.Data;
using System.Windows.Forms;
using OfficeOpenXml;

namespace SParametersExcelOOPDeneme
{
    public partial class Form1 : Form
    {
        ExcelManager excelManager = new ExcelManager();
        DataFilter filter = new DataFilter();
        public Form1()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnExcelOpen_Click(object sender, EventArgs e)
        {
            string fileName;

            DataTable dataTable = new DataTable();

            fileName = groupBox1.Text = excelManager.OpenExcelFileDialog();

            if (fileName != null)
            {
                dataTable = excelManager.ReaderExcelFile(fileName);
            }
            else
            {
                dataTable = null;
            }
            dataGridViewTumVeriler.DataSource = dataTable;
            UIHelper uIHelper = new UIHelper();
            uIHelper.dataGridViewColumnHeaderText(dataTable, 5,dataGridViewTumVeriler);

            foreach (var item in excelManager.GetSheetNames(groupBox1.Text))
                comboBoxSheetNames.Items.Add(item);
            comboBoxSheetNames.SelectedIndex = 0;

        }

        private void comboBoxSheetNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            string selectedSheetName = comboBoxSheetNames.SelectedItem.ToString();

            if (selectedSheetName != null)
            {
                dataTable = excelManager.ReaderExcelFile(groupBox1.Text, selectedSheetName);
            }
            else
            {
                dataTable = null;
            }
            dataGridViewTumVeriler.DataSource = dataTable;
        }

        private void btnSorgula_Click(object sender, EventArgs e)
        {
            dataGridViewSorgulanmisVeriler.Columns.Clear();

            DataFilter filter = new DataFilter();
            DataTable originalData = (DataTable)dataGridViewTumVeriler.DataSource;

            string selectedMinValue = textBoxMinMHz.Text;
            string selectedMaxValue = textBoxMaxMHz.Text;

            double minMHz = string.IsNullOrEmpty(selectedMinValue) ? 0 : double.Parse(selectedMinValue);
            double maxMHz = string.IsNullOrEmpty(selectedMaxValue) ? 0 : double.Parse(selectedMaxValue);

            try
            {
                DataTable filteredData = filter.FilterByMHz(originalData, minMHz, maxMHz);
                dataGridViewSorgulanmisVeriler.DataSource= filteredData;

                UIHelper uIHelper = new UIHelper();
                uIHelper.DataGridViewHeaderTextCopy(dataGridViewSorgulanmisVeriler,dataGridViewTumVeriler);
                
                ChartProcessor chartProcessor = new ChartProcessor(chart1,textBoxMinMHz,textBoxMaxMHz);
                chartProcessor.ShowDataOnChart(filteredData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string saveSheetName = textBoxSaveName.Text;

            string filePath = groupBox1.Text;

            string selectedMinValue = textBoxMinMHz.Text;
            string selectedMaxValue = textBoxMaxMHz.Text;

            double minMHz = string.IsNullOrEmpty(selectedMinValue) ? 0 : double.Parse(selectedMinValue);
            double maxMHz = string.IsNullOrEmpty(selectedMaxValue) ? 0 : double.Parse(selectedMaxValue);

            DataFilter filterSave = new DataFilter();

            filterSave.SaveFilteredData((DataTable)dataGridViewTumVeriler.DataSource, filePath,saveSheetName,minMHz,maxMHz);

            ChartProcessor chartProcessor = new ChartProcessor();
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(groupBox1.Text)))
            {
                chartProcessor.SaveGrafik(package, chart1, saveSheetName);
                package.Save();
            }

            comboBoxSheetNames.Items.Clear();
            foreach (var item in excelManager.GetSheetNames(groupBox1.Text))
                comboBoxSheetNames.Items.Add(item);

        }
    }
}
