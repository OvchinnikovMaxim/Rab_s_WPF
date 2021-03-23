using SQLquerys;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для New_class_price_form.xaml
    /// </summary>
    public partial class New_class_price_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public New_class_price_form()
        {
            InitializeComponent();

            combo_class_price.SelectedIndex = 0;
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

        private void btn_class_price_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zav;
                int type = 0;

                zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

                type = combo_class_price.SelectedIndex == 0 ? 20 : 19;

                string name = class_price_name.Text;
                name = name.Replace("'", "`");

                SqlCommand new_clas_price = new SqlCommand(q.New_priselist_clas(name, type, zav), connection);
                new_clas_price.ExecuteNonQuery();

                MessageBox.Show("Класс прайс-листа добавлен", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
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
