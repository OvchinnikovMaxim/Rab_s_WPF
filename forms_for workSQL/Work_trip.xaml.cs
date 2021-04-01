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

        OtherSQLquery otherSQLquery = new OtherSQLquery();

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
            label_info_trip.Visibility = Visibility.Hidden;
        }

        private void btn_log_trip_1c_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand log_trip_1c = new SqlCommand(otherSQLquery.Trip_log_in_1c(Convert.ToInt32(trip_id.Text)), connection);
                log_trip_1c.ExecuteNonQuery();

                label_info_trip.Visibility = Visibility.Visible;
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

        private void btn_trip_history_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter(otherSQLquery.Trip_history(Convert.ToInt32(trip_id_history.Text)), connection);

                dt = new DataTable();
                adapter.Fill(dt);
                data_trip_history.ItemsSource = dt.DefaultView;
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

        
    }
}
