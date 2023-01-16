using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для Trip_to_fact.xaml
    /// </summary>
    public partial class Trip_to_fact : Window
    {
        SqlConnection connection;

        Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;

        public Trip_to_fact()
        {
            InitializeComponent();
        }

        #region подключение
        private void Btn_connect_Click(object sender, RoutedEventArgs e)
        {
            connection = sc.Connection(text_server, label_status);
        }

        private void Text_server_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }

        private void Text_server_TextChanged(object sender, TextChangedEventArgs e)
        {
            sc.TextChanged(connection, text_server, label_status);
        }
        #endregion

        #region только числа
        private void Lot_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }

        private void Trip_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextServer(e);
        }
        #endregion

        #region проверка
        /// <summary>
        /// проверка лота
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_lot_check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int lot_ID = Convert.ToInt32(lot_id.Text);
                adapter = new SqlDataAdapter("SELECT * FROM auction.dbo.lots WHERE id=" + lot_ID, connection);

                dt = new DataTable();
                adapter.Fill(dt);
                grid_lot.ItemsSource = dt.DefaultView;
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

        /// <summary>
        /// проверка рейса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_trip_check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int trip_ID = Convert.ToInt32(trip_id.Text);
                adapter = new SqlDataAdapter("SELECT * FROM nefco.dbo.tc_trip WHERE id=" + trip_ID, connection);

                dt = new DataTable();
                adapter.Fill(dt);
                grid_trip.ItemsSource = dt.DefaultView;
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
        #endregion

        private void Btn_to_fact_Click(object sender, RoutedEventArgs e)
        {
            int ID_trip = Convert.ToInt32(trip_to_fact.Text);
            string DATE_T = date_fact.Text.ToString() + " " + time_fact.Text.ToString();

            try
            {
                Trip_fact(ID_trip, DATE_T);

                if (lot_to_end.IsChecked == true)
                {
                    Lot_end(ID_trip);
                    MessageBox.Show("Лот переведен в завершенные, проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Рейс переведен в фактические, проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
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

        /// <summary>
        /// Перевод лота по рейсу в завершенные
        /// </summary>
        /// <param name="trip_id">номер рейса</param>
        public void Lot_end(int trip_id)
        {
            string query = "UPDATE auction.dbo.lots SET completed_at = DATEADD(HH, -3, (SELECT date_shipped_fact FROM nefco.dbo.tc_trip WHERE id = " + trip_id + ")) WHERE object_id = (SELECT id FROM nefco.dbo.tc_trip WHERE id = " + trip_id + ")";
            SqlCommand lot_to_end = new SqlCommand(query, connection);
            lot_to_end.ExecuteNonQuery();
        }

        /// <summary>
        /// Перевод рейса в факт
        /// </summary>
        /// <param name="trip_id">номер рейса</param>
        /// <param name="date_time">дата выезда с комбината</param>
        public void Trip_fact(int trip_id, string date_time)
        {

            string query_shipped = "UPDATE nefco.dbo.tc_trip SET date_shipped_fact = '" + date_time + "' WHERE id = " + trip_id;
            SqlCommand trip_to_Fact = new SqlCommand(query_shipped, connection);
            trip_to_Fact.ExecuteNonQuery();

            SqlCommand load_fact = new SqlCommand("SELECT date_load_fact FROM nefco.dbo.tc_trip WHERE id = " + trip_id, connection);

            if (load_fact.ExecuteScalar().ToString()==string.Empty)
            {
                SqlCommand trip_to_load = new SqlCommand("UPDATE nefco.dbo.tc_trip SET date_load_fact = date_shipped_fact WHERE id = " + trip_id, connection);
                trip_to_load.ExecuteNonQuery();
                MessageBox.Show("Установлена дата фактической отгрузки, проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
