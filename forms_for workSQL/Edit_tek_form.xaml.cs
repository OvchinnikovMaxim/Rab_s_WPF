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
    /// Логика взаимодействия для Edit_tek_form.xaml
    /// </summary>
    public partial class Edit_tek_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        readonly OtherSQLquery otherQuery = new OtherSQLquery();

        SqlDataAdapter adapter;

        DataTable dt;
        public Edit_tek_form()
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
        private void id_tek_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void userID_edit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        } 
        #endregion

        private void userID_edit_TextChanged(object sender, TextChangedEventArgs e)
        {
            userID_edit.Foreground = Brushes.Green;
        }

        private void btn_search_id_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter(q.search_TEK(id_tek.Text), connection);

                //dt = new DataTable();
                
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

        private void btn_edit_tek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_search_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter(q.User_inf(search_user.Text), connection);

                dt = new DataTable();
                adapter.Fill(dt);
                data_info.ItemsSource = dt.DefaultView;
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
