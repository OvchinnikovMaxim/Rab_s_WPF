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
    /// Логика взаимодействия для New_distr_form.xaml
    /// </summary>
    public partial class New_distr_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        public New_distr_form()
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

        private void contractor_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_new_distr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zav, MT, MI;

                zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

                MT = mobile.IsChecked == true ? 1 : 0;

                MI = integration.IsChecked == true ? 1 : 0;

                SqlCommand new_distr = new SqlCommand(q.new_distr(Convert.ToInt32(contractor_id.Text), zav, distr_login.Text, distr_login.Text, MT, MI), connection);

                new_distr.ExecuteNonQuery();

                MessageBox.Show("Учетные данные добавлены в базу данных", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                MessageBox.Show("Отправьте письмо с учетными данными на указанную почту дистрибьютора и в копию поставьте ответственного сотрудника Нэфиса. " + '\n' + "Шаблон письма: data\\новый дистр\\учетные данные для установки MAPDS WEB.eml ", "Не забудьте", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void mobile_Click(object sender, RoutedEventArgs e)
        {
            if (integration.IsChecked == false)
            {
                integration.IsChecked = true;
            }            
        }

        private void integration_Click(object sender, RoutedEventArgs e)
        {
            if (integration.IsChecked == false)
            {
                mobile.IsChecked = false;
            }
        }

        private void combo_zavod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            distr_login.Text = combo_zavod.SelectedIndex == 0 ? "distr" : "kdistr";
        }
    }
}
