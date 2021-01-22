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
            if (openFile.ShowDialog() == MessageBoxResult.Cancel)
            {
                return;
            }

            string filename = openFile.FileName;

            file_name.Content = filename.Substring(filename.LastIndexOf('\\') + 1);
        }
    }
}
