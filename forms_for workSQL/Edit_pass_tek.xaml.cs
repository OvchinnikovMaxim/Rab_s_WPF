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
    /// Логика взаимодействия для Edit_pass_tek.xaml
    /// </summary>
    public partial class Edit_pass_tek : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        public Edit_pass_tek()
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

        private void tekID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_edit_pass_Click(object sender, RoutedEventArgs e)
        {
            int tek = Convert.ToInt32(tekID.Text);

            string new_pas = new_pass.Text;

            string new_passHash = BCrypt.Net.BCrypt.HashPassword(new_pas);

            string pass_ccac = "UPDATE nefco.dbo.co_contractor_attr_transp SET password = '" + new_pas + "' WHERE contractor_id =" + tek;

            string pass_aucHash = "UPDATE auction.dbo.users SET password='" + new_passHash + "' WHERE username = (SELECT login FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + tek + ")";

            try
            {
                SqlCommand new_pass_ccac = new SqlCommand(pass_ccac, connection);
                new_pass_ccac.ExecuteNonQuery();

                SqlCommand new_pass_Hash = new SqlCommand(pass_aucHash, connection);
                new_pass_Hash.ExecuteNonQuery();

                MessageBox.Show("Пароль изменен", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

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
