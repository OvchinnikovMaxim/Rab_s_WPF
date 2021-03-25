using SQLquerys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для New_user_inf_form.xaml
    /// </summary>
    public partial class New_user_inf_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;

        public New_user_inf_form()
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

        private void btn_new_userINF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zav = 0;
                zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

                SqlCommand new_user_inf = new SqlCommand(q.user_inf_new(text_login.Text, text_fio.Text, text_pass.Text, zav), connection);
                new_user_inf.ExecuteNonQuery();

                MessageBox.Show("Новый сотрудник добавлен в базу данных", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter("select * from nefco.dbo.user_inf where user_id like '%" + login_search.Text + "%'", connection);

                dt = new DataTable();
                adapter.Fill(dt);
                dataGR.ItemsSource = dt.DefaultView;
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
