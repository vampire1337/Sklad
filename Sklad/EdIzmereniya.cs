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
    public partial class EdIzmereniya : Form
    {
        public EdIzmereniya()
        {
            InitializeComponent();
        }

        private void единицы_измеренияBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.единицы_измеренияBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._1DataSet);

        }

        private void EdIzmereniya_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_1DataSet.Единицы_измерения". При необходимости она может быть перемещена или удалена.
            this.единицы_измеренияTableAdapter.Fill(this._1DataSet.Единицы_измерения);

        }
    }
}
