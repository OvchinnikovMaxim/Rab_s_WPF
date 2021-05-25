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

        Serv_conn sc = new Serv_conn();

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

        private void btn_search_id_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IDtek = Convert.ToInt32(id_tek.Text);

                #region отображение ТЭК
                label_zavod.Content = (string)new SqlCommand(@"SELECT  CASE factory_id
                                                                        WHEN  1 THEN 'НК'
                                                                        WHEN  2 THEN 'НБП'
                                                                        ELSE ''
                                                                       END AS 'завод' 
                                                              FROM nefco.dbo.co_contractor WHERE id = (SELECT contractor_id FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek + ")", connection).ExecuteScalar();

                edit_name_tek.Text = new SqlCommand("SELECT name FROM nefco.dbo.co_contractor WHERE id = (SELECT contractor_id FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek + ")", connection).ExecuteScalar().ToString();

                edit_addres_tek.Text = new SqlCommand("SELECT address FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_inn_tek.Text = new SqlCommand("SELECT inn FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_kpp_tek.Text = new SqlCommand("SELECT kpp FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_dogovor_tek.Text = new SqlCommand("SELECT contract_number FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_date_dogovor.SelectedDate = (DateTime)new SqlCommand("SELECT contract_date FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar();

                edit_name_bank.Text = new SqlCommand("SELECT bank_name FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_rasch_bank.Text = new SqlCommand("SELECT settlement_account FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_cor_bank.Text = new SqlCommand("SELECT loro_account FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString();

                edit_bik_bank.Text = new SqlCommand("SELECT bik FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + IDtek, connection).ExecuteScalar().ToString(); 
                #endregion

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
            try
            {
                int IDtek = Convert.ToInt32(id_tek.Text);
                string name = edit_name_tek.Text;
                string dogovor = edit_dogovor_tek.Text;
                string address = edit_addres_tek.Text;
                string r_s = edit_rasch_bank.Text;
                string bank = edit_name_bank.Text;
                string k_s = edit_cor_bank.Text;
                string bik = edit_bik_bank.Text;
                string inn = edit_inn_tek.Text;
                string kpp = edit_kpp_tek.Text;
                DateTime date_n = (DateTime)edit_date_dogovor.SelectedDate;
                int user = Convert.ToInt32(userID_edit.Text);

                SqlCommand search_userID = new SqlCommand("SELECT id FROM nefco.dbo.user_inf WHERE id=" + user, connection);

                if (user == Convert.ToInt32(search_userID.ExecuteScalar()))
                {
                    SqlCommand new_TEK = new SqlCommand(otherQuery.Edit_tek(IDtek, name, dogovor, address, r_s, bank, k_s, bik, inn, kpp, date_n, user), connection);
                    new_TEK.ExecuteNonQuery();

                    MessageBox.Show("Транспортная компания изменена", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    userID_edit.Foreground = Brushes.Red;
                    MessageBox.Show("Такого сотрудника нет в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
