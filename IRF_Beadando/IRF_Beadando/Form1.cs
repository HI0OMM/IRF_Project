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
using IRF_Beadando.Entities;
using System.IO;

namespace IRF_Beadando
{
    public partial class Form1 : Form
    {
        FociadatEntities context = new FociadatEntities();
        List<Table> Jatekosok;
        List<Table2> Posztok;
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
            LoadPictures();
            JatekosListBox.DisplayMember = "Jatekos_nev";
            PosztComboBox.DisplayMember = "Poszt_nev";
            

        }
        private void LoadPictures()
        {
            Gol Goal = new Gol();
            Goal.Left = button2.Left + button2.Width/2-29;
            Goal.Top = button2.Top-58;
            mainPanel.Controls.Add(Goal);

            Team Csapat = new Team();
            Csapat.Left = button4.Left + button4.Width / 2-29;
            Csapat.Top = button4.Top - 58;
            mainPanel.Controls.Add(Csapat);

            Assist Golpassz = new Assist();
            Golpassz.Left = button3.Left + button4.Width / 2 - 29;
            Golpassz.Top = button3.Top - 58;
            mainPanel.Controls.Add(Golpassz);


        }
        private void LoadData()
        {
            
            Jatekosok = context.Tables.ToList();
            JatekosListBox.DataSource = Jatekosok;
            Posztok = context.Table2.ToList();
            PosztComboBox.DataSource = Posztok;
            textBox1.Text = ((Table)JatekosListBox.SelectedItem).Gol.ToString();
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
                values[counter, 0] = t.Jatekos_Id;
                values[counter, 1] = t.Jatekos_nev;
                values[counter, 2] = t.Gol;
                values[counter, 3] = t.Golpassz;
                values[counter, 4] = t.Jatekperc;
                values[counter, 5] = t.Poszt_fk;
                values[counter, 6] = t.Csapat;
                values[counter, 7] = String.Format("={0}/{1}", GetCell(counter + 2, 5), GetCell(counter + 2, 3));
                
                counter++;
                xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
                
                Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
                headerRange.Font.Bold = true;
                headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                headerRange.Columns["A:H"].ColumnWidth = 30;
                headerRange.EntireColumn.AutoFit();
                headerRange.RowHeight = 40;
                headerRange.Interior.Color = Color.LightBlue;
                headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

                int lastRowID = xlSheet.UsedRange.Rows.Count;
                Excel.Range tableRange = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, headers.Length));
                tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                Excel.Range firstColRange = xlSheet.get_Range(GetCell(2, 1), GetCell(lastRowID, 1));
                firstColRange.Font.Bold = true;
                firstColRange.Interior.Color = Color.LightGray;
                firstColRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                Excel.Range lastColRange = xlSheet.get_Range(GetCell(2, headers.Length), GetCell(lastRowID, headers.Length));
                lastColRange.Interior.Color = Color.LightGreen;
                lastColRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                lastColRange.NumberFormat = "$????.??";
                
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
                xlWB.SaveAs(new FileInfo(@"c:\temp\Beadando.csv"));
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

       

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosListBox.SelectedItem).Gol.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosListBox.SelectedItem).Golpassz.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = ((Table)JatekosListBox.SelectedItem).Csapat.ToString();
        }
        private void JatekosListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JatekosListBox.DataSource != null)
            {
                textBox1.Text = ((Table)JatekosListBox.SelectedItem).Gol.ToString();
            }
            
        }
        
        
        
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var Id = ((Table2)PosztComboBox.SelectedItem).Poszt_Id;
            

            Jatekosok.RemoveAll(r => r.Poszt_fk == Id);
            JatekosListBox.DataSource = null;
            JatekosListBox.DataSource = Jatekosok;
            JatekosListBox.DisplayMember = "Jatekos_nev";











        }

        
    }
}
