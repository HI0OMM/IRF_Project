using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace IRF_Beadando
{
    public partial class Form1 : Form
    {
        Database1Entities context = new Database1Entities();
        List<Table> Jatekosok;
        string[] headers = new string[]
{
                "ID",
                "Név",
                "Gólok",
                "Gólpasszok",
                "Játékperc",
                "Poszt",
                "Csapat",
                "Gól/Perc"
};

        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            JatekosComboBox.DisplayMember = "Jatekos_nev";

        }   
        private void LoadData()
        {
            Jatekosok = context.Tables.ToList();
            JatekosComboBox.DataSource = Jatekosok;
        }
        void CreateTable()
        {
            object[,] values = new object[Jatekosok.Count, headers.Length];
            for (int col = 1; col <= headers.Length; col++)
            {
                xlSheet.Cells[1, col] = headers[col - 1];
            }
            int counter = 0;
            foreach (Table t in Jatekosok)
            {
                values[counter, 0] = t.Jatekos_id;
                values[counter, 1] = t.Jatekos_nev;
                values[counter, 2] = t.Golok;
                values[counter, 3] = t.Golpasszok;
                values[counter, 4] = t.Jatekperc;
                values[counter, 5] = t.Poszt;
                values[counter, 6] = t.Csapat;
                values[counter, 7] = String.Format("={0}/{1}", GetCell(counter + 2, 5), GetCell(counter + 2, 3));
                
                counter++;
                xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
            }
            string GetCell(int x, int y)
            {
                string ExcelCoordinate = "";
                int dividend = y;
                int modulo;

                while (dividend > 0)
                {
                    modulo = (dividend - 1) % 26;
                    ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                    dividend = (int)((dividend - modulo) / 26);
                }
                ExcelCoordinate += x.ToString();

                return ExcelCoordinate;
                
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWB.ActiveSheet;
                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        
        }

        private void JatekosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosComboBox.SelectedItem).Golok.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosComboBox.SelectedItem).Golok.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosComboBox.SelectedItem).Golpasszok.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosComboBox.SelectedItem).Csapat.ToString();
        }
    }
}
