using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sklad
{
    public partial class Prodazhi : Form
    {
        public Prodazhi()
        {
            InitializeComponent();
        }

        private void продажиBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.продажиBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._1DataSet);

        }

        private void Prodazhi_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Товар". При необходимости она может быть перемещена или удалена.
            this.товарTableAdapter.Fill(this._1DataSet.Товар);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Покупатели". При необходимости она может быть перемещена или удалена.
            this.покупателиTableAdapter.Fill(this._1DataSet.Покупатели);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Товары_продажи". При необходимости она может быть перемещена или удалена.
            this.товары_продажиTableAdapter.Fill(this._1DataSet.Товары_продажи);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Продажи". При необходимости она может быть перемещена или удалена.
            this.продажиTableAdapter.Fill(this._1DataSet.Продажи);

        }

        private void Prodazhi_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Validate();
            продажиBindingSource.EndEdit();
            продажиTableAdapter.Update(_1DataSet.Продажи);

            this.Validate();
            товары_продажиBindingSource.EndEdit();
            товары_продажиTableAdapter.Update(_1DataSet.Товары_продажи);
        }

        private void продажиDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn3"].Value = DateTime.Today;
        }

        private void товары_продажиDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn7"].Value = 1;
        }

        private void товары_продажиDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // если таблица не содержит колонок или строк, то выход из процудуры
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0)) { return; }
            DataGridViewCell MyCell = товары_продажиDataGridView[e.ColumnIndex, e.RowIndex];

            if (MyCell == null)
            {
                return;
            }

            if (MyCell.Value == null)
            {
                return;
            }

            //выполняем запрос на определение цены на продаваемый товар
            DataTable tres = new DataTable();
            float cena = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Sklad.Properties.Settings._1ConnectionString"].ConnectionString;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand("SELECT Цена FROM Товар WHERE [Код товара] = " + товары_продажиDataGridView.Rows[товары_продажиDataGridView.CurrentCell.RowIndex].Cells[2].Value.ToString(), conn);

                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_name");

                    tres = ds.Tables["result_name"];
                    foreach (DataRow row in tres.Rows)
                    {
                        cena = float.Parse(row["Цена"].ToString());
                    }
                }
            }

            var grid = (sender as DataGridView);
            //подставляем цену в ячейку
            if (e.RowIndex != -1 && ((e.ColumnIndex == 2)))
            {
                (grid.Rows[e.RowIndex]).Cells[4].Value = cena;
            }
        }
    }
}
