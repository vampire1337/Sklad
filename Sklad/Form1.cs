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

using Excel = Microsoft.Office.Interop.Excel;

namespace Sklad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void единицыИзмеренияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EdIzmereniya f = new EdIzmereniya();
            f.ShowDialog();
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Postavshiki f = new Postavshiki();
            f.ShowDialog();
        }

        private void покупателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pokupateli f = new Pokupateli();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tovar f = new Tovar();
            f.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today.AddDays(-15);
            dateTimePicker2.Value = DateTime.Today.AddDays(15);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Parse("01.01." +  DateTime.Today.Year.ToString());
            dateTimePicker2.Value = DateTime.Parse("31.03." + DateTime.Today.Year.ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Parse("01.04." + DateTime.Today.Year.ToString());
            dateTimePicker2.Value = DateTime.Parse("30.06." + DateTime.Today.Year.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Parse("01.07." + DateTime.Today.Year.ToString());
            dateTimePicker2.Value = DateTime.Parse("30.09." + DateTime.Today.Year.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Parse("01.10." + DateTime.Today.Year.ToString());
            dateTimePicker2.Value = DateTime.Parse("31.12." + DateTime.Today.Year.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Postavki f = new Postavki();
            f.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Prodazhi f = new Prodazhi();
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // создаем пустую книгу и объявляем переменные
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            int index = 2;
            // формируем шапку для отчета
            xlWorkSheet.Cells[1, 1] = "Отчет по поставкам с " + dateTimePicker1.Text + " по " + dateTimePicker2.Text;
            xlWorkSheet.get_Range("A1", "A1").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, 4]].Merge();

            xlWorkSheet.Cells[index, 1] = "Товар";
            xlWorkSheet.Cells[index, 2] = "Количество";
            xlWorkSheet.Cells[index, 3] = "Средняя цена";
            xlWorkSheet.Cells[index, 4] = "Сумма";
            //задаем ширину столбцов
            xlWorkSheet.Columns[1].ColumnWidth = 30;
            xlWorkSheet.Columns[2].ColumnWidth = 15;
            xlWorkSheet.Columns[3].ColumnWidth = 15;
            xlWorkSheet.Columns[4].ColumnWidth = 15;

            index++;

            DataTable tres = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Sklad.Properties.Settings._1ConnectionString"].ConnectionString;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    //выполняем запрос на получение данных
                    OleDbCommand cmd = new OleDbCommand("SELECT Товар.Название, Sum([Товары поставки].Количество) AS [Кол],"
                        +" Avg([Товары поставки].Цена) AS [СрЦена], Sum([Товары поставки].[Количество]*[Товары поставки].[Цена]) AS Сумма"
                        +" FROM Поставки INNER JOIN(Товар INNER JOIN[Товары поставки] ON Товар.[Код товара] = [Товары поставки].[Код товара])"
                        +" ON Поставки.[Код поставки] = [Товары поставки].[Код поставки]"
                        + " WHERE(((Поставки.Дата) >= @StartDate And(Поставки.Дата) <= @StopDate))"
                        + " GROUP BY Товар.Название;", conn);

                    cmd.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value.ToString());
                    cmd.Parameters.AddWithValue("@StopDate", dateTimePicker2.Value.ToString());

                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_name");

                    tres = ds.Tables["result_name"];

                    //выводим результат запроса в Excel
                    foreach (DataRow row in tres.Rows)
                    {
                        xlWorkSheet.Cells[index, 1] = row["Название"];
                        xlWorkSheet.Cells[index, 2] = row["Кол"];
                        xlWorkSheet.Cells[index, 3] = row["СрЦена"];
                        xlWorkSheet.Cells[index, 4] = row["Сумма"];

                        index++;
                    }
                }
            }

            //автовыравнивание колонок
            //xlWorkSheet.Columns["A:B"].AutoFit();

            //границы таблицы
            Excel.Range xlWorkSheet_rng = xlWorkSheet.get_Range("A2", "D" + (index - 1).ToString());
            xlWorkSheet_rng.Borders.ColorIndex = 0;
            xlWorkSheet_rng.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlWorkSheet_rng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            //сделать первую строку жирной
            xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // создаем пустую книгу и объявляем переменные
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            int index = 2;

            xlWorkSheet.Cells[1, 1] = "Отчет по продажам с " + dateTimePicker1.Text + " по " + dateTimePicker2.Text;
            xlWorkSheet.get_Range("A1", "A1").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, 5]].Merge();

            xlWorkSheet.Cells[index, 1] = "Товар";
            xlWorkSheet.Cells[index, 2] = "Ед. изм.";
            xlWorkSheet.Cells[index, 3] = "Количество";
            xlWorkSheet.Cells[index, 4] = "Средняя цена";
            xlWorkSheet.Cells[index, 5] = "Сумма";
            xlWorkSheet.Columns[1].ColumnWidth = 30;
            xlWorkSheet.Columns[2].ColumnWidth = 15;
            xlWorkSheet.Columns[3].ColumnWidth = 15;
            xlWorkSheet.Columns[4].ColumnWidth = 15;

            index++;

            DataTable tres = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Sklad.Properties.Settings._1ConnectionString"].ConnectionString;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    OleDbCommand cmd = new OleDbCommand("SELECT Товар.Название, [Единицы измерения].Сокращение, Avg([Товары продажи].Количество) AS [Кол], Sum([Товары продажи].Стоимость) AS [СрЦена], Sum([Товары продажи].Количество*[Товары продажи].Стоимость) AS Сумма"
                        + " FROM[Единицы измерения] INNER JOIN(Товар INNER JOIN(Продажи INNER JOIN[Товары продажи] ON Продажи.[Код продажи] = [Товары продажи].[Код продажи]) ON Товар.[Код товара] = [Товары продажи].[Код товара]) ON[Единицы измерения].[Код единицы измерения] = Товар.[Код ед изм]"
                        + " WHERE(((Продажи.Дата) >= @StartDate And(Продажи.Дата) <= @StopDate))"
                        + " GROUP BY Товар.Название, [Единицы измерения].Сокращение;", conn);

                    cmd.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value.ToString());
                    cmd.Parameters.AddWithValue("@StopDate", dateTimePicker2.Value.ToString());

                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "result_name");

                    tres = ds.Tables["result_name"];

                    foreach (DataRow row in tres.Rows)
                    {
                        xlWorkSheet.Cells[index, 1] = row["Название"];
                        xlWorkSheet.Cells[index, 2] = row["Сокращение"];
                        xlWorkSheet.Cells[index, 3] = row["Кол"];
                        xlWorkSheet.Cells[index, 4] = row["СрЦена"];
                        xlWorkSheet.Cells[index, 5] = row["Сумма"];

                        index++;
                    }
                }
            }

            //автовыравнивание колонок
            //xlWorkSheet.Columns["A:B"].AutoFit();

            //границы таблицы
            Excel.Range xlWorkSheet_rng = xlWorkSheet.get_Range("A2", "E" + (index - 1).ToString());
            xlWorkSheet_rng.Borders.ColorIndex = 0;
            xlWorkSheet_rng.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlWorkSheet_rng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            //сделать первую строку жирной
            xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
