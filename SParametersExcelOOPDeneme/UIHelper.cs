using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SParametersExcelOOPDeneme
{
    public class UIHelper
    {
        public void DataGridViewHeaderTextCopy(DataGridView dataGridViewHedef,DataGridView dataGridViewKaynak)
        {
            foreach (DataGridViewColumn column in dataGridViewHedef.Columns)
            {
                //if (column.Index == 5)
                    //break;
                column.HeaderText = dataGridViewKaynak.Columns[column.Index].HeaderText;
            }
        }
        public void dataGridViewColumnHeaderText(DataTable dataTable, int endColumn,DataGridView dataGridView)
        {
            if (dataTable.Rows.Count > 2)
            {
                for (int i = 0; i < endColumn; i++)
                {
                    dataGridView.Columns[i].HeaderText = dataTable.Rows[1][i].ToString();
                }
            }
        }
      
    }
}
