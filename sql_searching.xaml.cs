using SQLquerys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для sql_searching.xaml
    /// </summary>
    public partial class sql_searching : Window
    {
        SqlConnection connection;

        Query q = new Query();

        DataTable dt;
        SqlDataAdapter adapter;

        public sql_searching()
        {
            InitializeComponent();

            combo_zavod.SelectedIndex = 0;            
        }


        #region форма добавления слова/фразы или очищения поиска
        private void add_word_Click(object sender, RoutedEventArgs e)
        {
            Word_search_form word_Search = new Word_search_form();
            word_Search.Owner = this;//При создании формы устанавливаем владельца
            word_Search.ShowDialog();
        }

        private void del_word_Click(object sender, RoutedEventArgs e)
        {
            list_search_word.Items.Remove(list_search_word.SelectedItem);
        }

        private void clear_word_Click(object sender, RoutedEventArgs e)
        {
            list_search_word.Items.Clear();
        } 
        #endregion

        /// <summary>
        /// Активация поля для поиска по SD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_search_SD_Checked(object sender, RoutedEventArgs e)
        {
            search_sd.IsEnabled = true;
        }

        /// <summary>
        /// Дезактивация поля для поиска по SD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_search_SD_Unchecked(object sender, RoutedEventArgs e)
        {
            search_sd.IsEnabled = false;
        }

        public void search_Click(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(conSTR(text_server.Text));
            connection.Open();
            
            try
            {
                
                if (radio_user_inf.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.User_inf(user_inf_surname.Text), connection);
                }
                if (radio_user_profile.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.user_inf_profile(Convert.ToInt32(user_inf_profile.Text)), connection);
                }
                if (radio_user_ncsd.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.NCSD(login_ncsd.Text, name_ncsd.Text, surname_ncsd.Text, Convert.ToInt32(IDuser_ncsd.Text)), connection);
                }
                if (radio_distr.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.Distr(name_distr.Text, Convert.ToInt32(id_distr.Text)), connection);
                }
                if (radio_login_distr.IsChecked == true)
                {
                    switch (combo_zavod.SelectedIndex)
                    {
                        case 0:
                            adapter = new SqlDataAdapter(q.login_distr(1), connection);
                            break;
                        case 1:
                            adapter = new SqlDataAdapter(q.login_distr(2), connection);
                            break;
                    }
                }
                if (radio_last_price.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.prod_in_last_price(Convert.ToInt32(last_price.Text)), connection);
                }
                if (radio_distr_price.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.distr_prices(Convert.ToInt32(distr_price.Text)), connection);
                }
                if (radio_prod_price.IsChecked == true)
                {
                    adapter = new SqlDataAdapter(q.prod_in_price(Convert.ToInt32(prod_price.Text)), connection);
                }
                if (radio_search_SD.IsChecked == true)
                {
                    int count_w = list_search_word.Items.Count - 1;

                    string query = String.Empty;

                    try
                    {
                        query = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; select * from sd.dbo.sd_history ";
                        query += "where value like '%" + list_search_word.Items[0] + "%' ";

                        for (int i = 1; i < count_w; i++)
                        {
                            query += "and value LIKE '%" + list_search_word.Items[i] + "%' ";
                        }

                        if (list_search_word.Items[count_w] != list_search_word.Items[0])
                        {
                            query += "and value LIKE '%" + list_search_word.Items[count_w] + "%' ";
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("не добавлено содержимое для поиска");
                    }

                    query += "ORDER BY 1 DESC";

                    adapter = new SqlDataAdapter(query, connection);
                }
                
                                
                dt = new DataTable();
                adapter.Fill(dt);                
                dataGR.ItemsSource = dt.DefaultView;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }


        #region только цифры
        private void user_inf_profile_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void text_server_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || e.Text == "."))
            {
                e.Handled = true;
            }

        }

        private void IDuser_ncsd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void id_distr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void last_price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void distr_price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void prod_price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
        #endregion

        /// <summary>
        /// Формирование строки подключения
        /// </summary>
        /// <param name="server">IP адрес сервера</param>
        /// <returns>строка подключения</returns>
        public string conSTR(string server)
        {
            SqlConnectionStringBuilder conSTR = new SqlConnectionStringBuilder();
            conSTR.DataSource = server;
            conSTR.IntegratedSecurity = true;

            return conSTR.ToString();
        }

    }
}
