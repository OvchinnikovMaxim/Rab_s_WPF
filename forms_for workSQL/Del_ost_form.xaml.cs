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
    /// Логика взаимодействия для Del_ost_form.xaml
    /// </summary>
    public partial class Del_ost_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public Del_ost_form()
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

        private void text_distrID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_search_distr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int contr_delOst = Convert.ToInt32(text_distrID.Text);
                DateTime date_delOst = (DateTime)date_ost.SelectedDate;

                string query = String.Empty;

                query = "DELETE FROM nefco.dbo.ost_db WHERE  contractor_id = " + contr_delOst + " AND cast (date as date) ='" + date_delOst.ToShortDateString() + "'";

                SqlCommand del_ost = new SqlCommand(query, connection);
                del_ost.ExecuteNonQuery();

                MessageBox.Show("Остатки удалены", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Не указан код дистрибьютора" + '\n' + ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не указан код дистрибьютора" + '\n' + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
