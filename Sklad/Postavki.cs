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
    public partial class Postavki : Form
    {
        public Postavki()
        {
            InitializeComponent();
        }

        private void поставкиBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.поставкиBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._1DataSet);

        }

        private void Postavki_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Товар". При необходимости она может быть перемещена или удалена.
            this.товарTableAdapter.Fill(this._1DataSet.Товар);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Товары_поставки". При необходимости она может быть перемещена или удалена.
            this.товары_поставкиTableAdapter.Fill(this._1DataSet.Товары_поставки);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Поставщики". При необходимости она может быть перемещена или удалена.
            this.поставщикиTableAdapter.Fill(this._1DataSet.Поставщики);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Поставки". При необходимости она может быть перемещена или удалена.
            this.поставкиTableAdapter.Fill(this._1DataSet.Поставки);

        }

        private void товары_поставкиDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn7"].Value = 1;
        }

        private void товары_поставкиDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0)) { return; }
            DataGridViewCell MyCell = товары_поставкиDataGridView[e.ColumnIndex, e.RowIndex];

            if (MyCell == null)
            {
                return;
            }

            if (MyCell.Value == null)
            {
                return;
            }

            DataTable tres = new DataTable();
            float cena = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Sklad.Properties.Settings._1ConnectionString"].ConnectionString;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand("SELECT Цена FROM Товар WHERE [Код товара] = " + товары_поставкиDataGridView.Rows[товары_поставкиDataGridView.CurrentCell.RowIndex].Cells[2].Value.ToString(), conn);

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

            int cnt = 0;
            cnt = int.Parse((grid.Rows[e.RowIndex]).Cells[3].Value.ToString());

            if (e.RowIndex != -1 && ((e.ColumnIndex == 2)))
            {
                (grid.Rows[e.RowIndex]).Cells[4].Value = cena*0.8;
            }
        }

        private void поставкиDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn3"].Value = DateTime.Today;
        }

        private void Postavki_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Validate();
            поставкиBindingSource.EndEdit();
            поставкиTableAdapter.Update(_1DataSet.Поставки);

            this.Validate();
            товары_поставкиBindingSource.EndEdit();
            товары_поставкиTableAdapter.Update(_1DataSet.Товары_поставки);
        }
    }
}
