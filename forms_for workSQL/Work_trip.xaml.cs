using SQLquerys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для Work_trip.xaml
    /// </summary>
    public partial class Work_trip : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;
        public Work_trip()
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

        #region толькочисла
        private void trip_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }

        private void trip_id_history_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        } 
        #endregion

        private void trip_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btn_log_trip_1c_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void btn_trip_history_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
