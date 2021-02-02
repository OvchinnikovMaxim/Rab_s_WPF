using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SQLquerys;
using System.Data.SqlClient;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для CopyTT_form.xaml
    /// </summary>
    public partial class CopyTT_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public CopyTT_form()
        {
            InitializeComponent();
        }

        #region подключение
        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            connection = sc.Connection(text_server, label_status);
        }

        private void text_server_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }

        private void text_server_TextChanged(object sender, TextChangedEventArgs e)
        {
            sc.TextChanged(connection, text_server, label_status);
        }
        #endregion

        #region только числа
        private void distrID_out_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void distrID_in_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void sectorID_out_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void sectorID_in_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        } 
        #endregion

        private void btn_copyTT_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
