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
    /// Логика взаимодействия для New_tek_form.xaml
    /// </summary>
    public partial class New_tek_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;

        public New_tek_form()
        {
            InitializeComponent();

            combo_zavod.SelectedIndex = 0;
            combo_class.SelectedIndex = 0;
            date_dogovor.SelectedDate = DateTime.Now;
            link_auction_act.ToolTip= "Имя пользователя: ssrs\\administrator\r\n" +
                                          "Пароль: Byrke5l8byu\r\n" +
                                          "tk contractor - код новой ТЭК\r\n" +
                                          "ReportParameter3 = 1";
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
        private void userID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void text_from_railway_trans_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void text_to_railway_trans_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        } 
        #endregion

        private void btn_search_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter(q.User_inf(search_user.Text), connection);

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

        private void btn_search_tek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter(q.search_TEK(search_tek.Text), connection);

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

        private void btn_edit_pass_tek_Click(object sender, RoutedEventArgs e)
        {
            Edit_pass_tek edit_Pass = new Edit_pass_tek();
            edit_Pass.Show();
        }

        private void btn_info_new_tek_Click(object sender, RoutedEventArgs e)
        {
            Create_tek_info create_Tek = new Create_tek_info();
            create_Tek.Show();
        }

        private void btn_new_tek_Click(object sender, RoutedEventArgs e)
        {
            if (name_tek.Text == String.Empty || userID.Text == String.Empty)
            {
                name_tek.Foreground = Brushes.Red;
                userID.Foreground = Brushes.Red;
            }
            else
            {
                try
                {
                    int zav, clas;
                    zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;
                    clas = combo_class.SelectedIndex == 0 ? 20 : 21;
                    string name = name_tek.Text;
                    string dogovor = dogovor_tek.Text;
                    string address = addres_tek.Text;
                    string r_s = rasch_bank.Text;
                    string bank = name_bank.Text;
                    string k_s = cor_bank.Text;
                    string bik = bik_bank.Text;
                    string inn = inn_tek.Text;
                    string kpp = kpp_tek.Text;
                    DateTime date_n = (DateTime)date_dogovor.SelectedDate;                    
                    string pass = password.Text;
                    string log = login.Text;
                    int user = Convert.ToInt32(userID.Text);

                    SqlCommand search_userID = new SqlCommand("SELECT id FROM nefco.dbo.user_inf WHERE id=" + user, connection);

                    if (user == Convert.ToInt32(search_userID.ExecuteScalar()))
                    {
                        SqlCommand new_TEK = new SqlCommand(q.new_TEK(name, zav, dogovor, address, r_s, bank, k_s, bik, inn, kpp, date_n, pass, log, user, clas), connection);
                        new_TEK.ExecuteNonQuery();

                        MessageBox.Show("Транспортная компания добавлена", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        userID.Foreground = Brushes.Red;
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

        private void auction_act_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://192.168.2.71/NefcoReports/Pages/Report.aspx?ItemPath=%2fauction_login_act");
        }

        private void userID_TextChanged(object sender, TextChangedEventArgs e)
        {
            userID.Foreground = Brushes.Green;
        }

        private void name_tek_TextChanged(object sender, TextChangedEventArgs e)
        {
            name_tek.Foreground = Brushes.Green;
        }

        private void combo_class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            group_uslugi.IsEnabled = combo_class.SelectedIndex == 1;
        }

        private void btn_copy_service_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int trans_from = Convert.ToInt32(text_from_railway_trans.Text);
                int trans_to = Convert.ToInt32(text_to_railway_trans.Text);

                SqlCommand search_trans_from = new SqlCommand("SELECT contractor_id FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + trans_from, connection);
                SqlCommand search_trans_to = new SqlCommand("SELECT contractor_id FROM nefco.dbo.co_contractor_attr_transp WHERE contractor_id=" + trans_to, connection);

                string query = @"INSERT INTO nefco.dbo.tc_trip_railway_transpcompany_service(transp_company,service_id)" +
                "SELECT " + trans_to + ", service_id FROM nefco.dbo.tc_trip_railway_transpcompany_service WHERE transp_company=" + trans_from + ";";

                if (trans_from == Convert.ToInt32(search_trans_from.ExecuteScalar()) && trans_to == Convert.ToInt32(search_trans_to.ExecuteScalar()))
                {
                    SqlCommand copy_services = new SqlCommand(query, connection);
                    copy_services.ExecuteNonQuery();

                    MessageBox.Show("Услуги скопированы", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Одной из кодов не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Edit_tek_Click(object sender, RoutedEventArgs e)
        {
            Edit_tek_form edit_Tek = new Edit_tek_form();
            edit_Tek.Show();
        }
    }
}
