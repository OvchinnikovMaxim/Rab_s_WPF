using Microsoft.Win32;
using SQLquerys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

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

            text_info.Text = '\n' + "Прежде чем выбрать файл через кнопку \"Выбор файла\":" +
                             '\n' + "1) Проверить расстановку столбцов с шаблоном (шаблон в папке data\\создание ТТ в той же директории что и запущенная программа)." +
                             '\n' + "2) Необходимо в самом файле в столбцы \"Канал\" и \"Тип\" поставить их коды." +
                             '\n' + "Каналы смотреть в таблице NEFCO.DBO.CLIENT_CARD_CATEGORY." +
                             '\n' + "Типы смотреть в таблице NEFCO.DBO.CLIENT_CARD_TYPE." /*+
                             '\n' +
                             '\n' + "Либо установить надстройку для Excel из папки data\\надстройка в EXCEL при заведении ТТ"*/;
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

        private void text_server_TextChanged(object sender, TextChangedEventArgs e)
        {
            sc.TextChanged(connection, text_server, label_status);
        }

        private void btn_creat_tt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zav, typeTT, kanal, sector;
                zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

                int contractorID = Convert.ToInt32(text_distr.Text);

                string inn, name, address;

                for (int i = 1; i < dt_excel.Rows.Count; i++)
                {
                    inn = dt_excel.Rows[i][3].ToString();
                    name = dt_excel.Rows[i][1].ToString();
                    name = name.Replace("'", "`");

                    address = dt_excel.Rows[i][2].ToString();

                    typeTT = Convert.ToInt32(dt_excel.Rows[i][6]);
                    kanal = Convert.ToInt32(dt_excel.Rows[i][4]);
                    sector = Convert.ToInt32(dt_excel.Rows[i][5]);

                    SqlCommand cr_TT = new SqlCommand(q.create_newTT(zav, inn, name, address, typeTT, kanal, sector, contractorID), connection);
                    cr_TT.ExecuteNonQuery();
                }

                int countTT = dt_excel.Rows.Count - 1;
                MessageBox.Show("Количество созданных карточек = " + countTT, "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

    }
}
