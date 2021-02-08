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
using System.Diagnostics;
using SQLquerys;
using System.Data.SqlClient;
using System.Data;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для New_tek_form.xaml
    /// </summary>
    public partial class New_tek_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        DataTable dt;

        SqlDataAdapter adapter;

        public New_tek_form()
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

        private void userID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

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
                adapter = new SqlDataAdapter(q.User_inf(search_tek.Text), connection);

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
                    int zav;
                    zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

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
                        SqlCommand new_TEK = new SqlCommand(q.new_TEK(name, zav, dogovor, address, r_s, bank, k_s, bik, inn, kpp, date_n, pass, log, user), connection);
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
    }
}
