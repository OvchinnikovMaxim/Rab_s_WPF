using SQLquerys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для Del_user.xaml
    /// </summary>
    public partial class Del_user : Window
    {
        SqlConnection connection;

        Query q = new Query();

        DataTable dt;
        SqlDataAdapter adapter;

        Serv_conn sc = new Serv_conn();
        public Del_user()
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radio_user_inf.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.User_inf(user_inf_surname.Text), connection);
                }
                if (radio_user_ncsd.IsChecked == true)
                {
                    string query = String.Empty;
                    query = @"SELECT 
							cu.id AS 'ИД сотрудника для ncsd',
							cu.login AS 'Логин',
							cu.password AS 'Пароль',	
							cp.id AS 'ИД физического лица',
							cp.name AS 'Имя',
							cp.surname AS 'Фамилия',
							cp.patronymic AS 'Отчество',
							CASE cp.factory_id
								WHEN  1 THEN 'НК'
								WHEN  2 THEN 'НБП'
								ELSE ''
							END AS 'завод',	
							e.id AS 'Код сотрудника',
							e.date_end AS 'Дата окончания работы',
							e.staff_id AS 'Код ставки - staff_id',
							e.del AS 'Удаленный сотрудник'
						FROM nefco.dbo.co_user cu
						  LEFT JOIN nefco.dbo.co_person cp ON cu.id=cp.user_id
						  LEFT JOIN nefco.dbo.employee e ON e.person_id=cp.id
						  WHERE cp.surname LIKE '%" + surname_ncsd.Text + "%' AND cu.del = 0";

                    adapter = new SqlDataAdapter(query, connection);
                }

                dt = new DataTable();
                adapter.Fill(dt);
                dataGR.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string query = String.Empty;
            string user = String.Empty;

            try
            {
                int n = Convert.ToInt32(id.Text);

                if (radio_user_inf.IsChecked == true)
                {
                    query = "UPDATE nefco.dbo.user_inf SET pass = (SELECT RTRIM(pass)+'_tfgk4jf' FROM nefco.dbo.user_inf WHERE id = " + n + "), date_end=CURRENT_TIMESTAMP WHERE id = " + n;
                    user = radio_user_inf.Content.ToString();
                }
                if (radio_user_ncsd.IsChecked == true)
                {
                    query = "UPDATE nefco.dbo.co_user SET del=1 WHERE id = " + n;
                    user = radio_user_ncsd.Content.ToString();
                }

                SqlCommand del_user = new SqlCommand(query, connection);
                del_user.ExecuteNonQuery();

                MessageBox.Show("Пользовотель " + user + " помечен на удаление", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
    }
}
