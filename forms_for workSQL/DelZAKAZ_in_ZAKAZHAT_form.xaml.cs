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
    /// Логика взаимодействия для DelZAKAZ_in_ZAKAZHAT_form.xaml
    /// </summary>
    public partial class DelZAKAZ_in_ZAKAZHAT_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        OtherSQLquery otherQuery = new OtherSQLquery();

        Serv_conn sc = new Serv_conn();

        public DelZAKAZ_in_ZAKAZHAT_form()
        {
            InitializeComponent();

            comment.Text = "Заказ удален. Пользователь: " + Environment.UserName + ". По обращению от. Причина: .";

            rec_comment.Text = "Заказ восстановлен. Пользователь: " + Environment.UserName + ". По обращению от. Причина: .";
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
        private void text_zakazID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void rec_text_zakazID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        } 
        #endregion

        #region работа с чекбоксами
        private void check_del_zakaz_Checked(object sender, RoutedEventArgs e)
        {
            text_zakazID.IsEnabled = true;
            btn_del_zakaz.IsEnabled = true;
            comment.IsEnabled = true;

            rec_text_zakazID.IsEnabled = false;
            btn_rec_zakaz.IsEnabled = false;
            rec_comment.IsEnabled = false;

            check_rec_zakaz.IsChecked = false;
        }

        private void check_del_zakaz_Unchecked(object sender, RoutedEventArgs e)
        {
            text_zakazID.IsEnabled = false;
            btn_del_zakaz.IsEnabled = false;
            comment.IsEnabled = false;
        }

        private void check_rec_zakaz_Unchecked(object sender, RoutedEventArgs e)
        {
            rec_text_zakazID.IsEnabled = false;
            btn_rec_zakaz.IsEnabled = false;
            rec_comment.IsEnabled = false;
        }

        private void check_rec_zakaz_Checked(object sender, RoutedEventArgs e)
        {
            rec_text_zakazID.IsEnabled = true;
            btn_rec_zakaz.IsEnabled = true;
            rec_comment.IsEnabled = true;

            text_zakazID.IsEnabled = false;
            btn_del_zakaz.IsEnabled = false;
            comment.IsEnabled = false;

            check_del_zakaz.IsChecked = false;
        }
        #endregion

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

        private void btn_rec_zakaz_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            SqlDataAdapter adapter;
            try
            {
                SqlCommand rec_zakaz = new SqlCommand(otherQuery.Recovery_zakaz(Convert.ToInt32(rec_text_zakazID.Text), rec_comment.Text), connection);
                rec_zakaz.ExecuteNonQuery();

                MessageBox.Show("Заказ восстановлен", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                adapter = new SqlDataAdapter(otherQuery.Display_zakaz(Convert.ToInt32(rec_text_zakazID.Text)), connection);

                dt = new DataTable();
                adapter.Fill(dt);
                display_rec.ItemsSource = dt.DefaultView;

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
