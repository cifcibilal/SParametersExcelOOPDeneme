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
    {   /**
         * @brief Kaynak DataGridView'in sütun başlıklarını hedef DataGridView'e kopyalar.
         * 
         * @param dataGridViewHedef: Sütun başlıkları kopyalanacak hedef DataGridView nesnesi.
         * @param dataGridViewKaynak: Sütun başlıkları kopyalanacak kaynak DataGridView nesnesi.
         * 
         * @return void
         */
        public void DataGridViewHeaderTextCopy(DataGridView dataGridViewHedef,DataGridView dataGridViewKaynak)
        {
            foreach (DataGridViewColumn column in dataGridViewHedef.Columns)
            {
                //if (column.Index == 5)
                    //break;
                column.HeaderText = dataGridViewKaynak.Columns[column.Index].HeaderText;
            }
        }
        /**
         * @brief Verilen DataTable'dan sütun başlıklarını belirli bir DataGridView'e atar.
         * 
         * @param dataTable: Sütun başlıkları alınacak DataTable nesnesi.
         * @param endColumn: Atanacak sütun başlıklarının son sütun indeksi.
         * @param dataGridView: Sütun başlıkları atanacak DataGridView nesnesi.
         * 
         * @return void
         */
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
