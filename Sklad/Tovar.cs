using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sklad
{
    public partial class Tovar : Form
    {
        public Tovar()
        {
            InitializeComponent();
        }

        private void товарBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.товарBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._1DataSet);

        }

        private void Tovar_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Единицы_измерения". При необходимости она может быть перемещена или удалена.
            this.единицы_измеренияTableAdapter.Fill(this._1DataSet.Единицы_измерения);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Товар". При необходимости она может быть перемещена или удалена.
            this.товарTableAdapter.Fill(this._1DataSet.Товар);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            товарBindingSource.Filter = "Название like '%" + textBox1.Text + "%'";
        }
    }
}
