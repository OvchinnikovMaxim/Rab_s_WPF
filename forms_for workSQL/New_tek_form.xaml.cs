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
using System.Data;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для New_tek_form.xaml
    /// </summary>
    public partial class New_tek_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;

        public New_tek_form()
        {
            InitializeComponent();

            combo_zavod.SelectedIndex = 0;
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

        private void userID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btn_search_user_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_search_tek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_edit_pass_tek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_info_new_tek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_new_tek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void auction_act_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
