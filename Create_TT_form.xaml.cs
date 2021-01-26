using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using SQLquerys;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для Create_TT_form.xaml
    /// </summary>
    public partial class Create_TT_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        DataTable dt_excel;

        Serv_conn sc = new Serv_conn();

        OpenFileDialog openFile = new OpenFileDialog();

        public Create_TT_form()
        {
            InitializeComponent();

            combo_zavod.SelectedIndex = 0;
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            connection = sc.Connection(text_server, label_status);
        }

        private void create_tt_form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (connection != null)
                    connection.Close();
            }
            catch (SqlException)
            {

            }
        }

        private void text_server_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }

        private void text_distr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_creat_tt_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void choice_file_Click(object sender, RoutedEventArgs e)
        {
            if (openFile.ShowDialog() == false)
            {
                return;
            }

            string filename = openFile.FileName;

            file_name.Content = filename.Substring(filename.LastIndexOf('\\') + 1);

            dt_excel = get_exceldata(filename);

            #region удаление пустых строк
            for (int i = dt_excel.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt_excel.Rows[i];
                if (dr[1].ToString() == String.Empty || dr[1].ToString() == "")
                {
                    dr.Delete();
                }
            }
            dt_excel.AcceptChanges();
            #endregion
        }

        /// <summary>
        /// запись значений из excel в таблицу даных
        /// </summary>
        /// <param name="filename">файл</param>
        /// <returns>таблица с данными</returns>
        public DataTable get_exceldata(string filename)
        {
            DataTable dt = new DataTable();
            dt.Clear();

            Excel.Application app = new Excel.Application();//открыть эксель
            Excel.Workbook workbook = app.Workbooks.Open(filename);//открыть файл
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];//получить 1 лист
            Excel.Range range = worksheet.UsedRange; // набор ячеек

            object[,] data = (object[,])range.Value2; // массив значений с листа равен по размеру листу

            const int cCnt = 7; // количество колонок
            int rCnt = range.Rows.Count; // количество строк

            int row, col;

            for (col = 1; col <= cCnt; col++) //по всем колонкам
            {
                dt.Columns.Add(col.ToString(), typeof(string));

                for (row = 1; row <= rCnt; row++) // по всем строкам
                {
                    dt.Rows.Add();
                    dt.Rows[row - 1][col - 1] = data[row, col];
                }
            }

            workbook.Close(false);//закрыть не сохраняя
            app.Quit();// выйти из экселя
            GC.Collect();// сборка мусора

            return dt;
        }

        private void text_server_TextChanged(object sender, TextChangedEventArgs e)
        {
            sc.TextChanged(connection, text_server, label_status);
        }
    }
}
