using SQLquerys;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для DelZAKAZ_in_ZAKAZHAT_form.xaml
    /// </summary>
    public partial class DelZAKAZ_in_ZAKAZHAT_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public DelZAKAZ_in_ZAKAZHAT_form()
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

        private void text_zakazID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_del_zakaz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand del_zakaz = new SqlCommand(q.del_zakaz_hat(Convert.ToInt32(text_zakazID.Text), comment.Text), connection);
                del_zakaz.ExecuteNonQuery();

                MessageBox.Show("Заказ удален", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

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
