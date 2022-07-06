using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace rab_stol.forms_for_workSQL.autorization_forms
{
    /// <summary>
    /// Логика взаимодействия для New_prf_form.xaml
    /// </summary>
    public partial class New_prf_form : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private readonly int prg_id;

        private DataTable dt_Prgm_tags;

        private SqlDataAdapter adapter_Prgm_tags;

        private readonly Dictionary<int, string> Prgm_Tags = new Dictionary<int, string>();

        public New_prf_form(TextBox text_server, Label label_status, int programm_id)
        {
            InitializeComponent();
            prg_id = programm_id;
            connection = sc.Connection(text_server, label_status);
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Активация кнопки для добавления нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Name_new_prf_TextChanged(object sender, TextChangedEventArgs e)
        {
            Add_prf.IsEnabled = !string.IsNullOrEmpty(Name_new_prf.Text.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // загрузка списка тэгов
            adapter_Prgm_tags = new SqlDataAdapter(Profile_tags(prg_id), connection);
            dt_Prgm_tags = new DataTable();
            adapter_Prgm_tags.Fill(dt_Prgm_tags);
            for (int i = 0; i < dt_Prgm_tags.Rows.Count; i++)
            {
                List_tags.Items.Add(new ListBoxItem() { Content = dt_Prgm_tags.Rows[i][1].ToString() });
                Prgm_Tags.Add(Convert.ToInt32(dt_Prgm_tags.Rows[i][0].ToString()), dt_Prgm_tags.Rows[i][1].ToString());
            }
        }

        /// <summary>
        /// Запрос списка тэгов
        /// </summary>
        /// <param name="programm"></param>
        /// <returns></returns>
        public string Profile_tags(int programm)
        {
            string s = "SELECT tag, tag_name FROM nefco.dbo.auth_prgm_tags WHERE prgm_id=" + programm;
            return s;
        }

        private void Add_prf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ADDPRF(Name_new_prf.Text, prg_id);

                _PRF_TAGS(_PRF_ID(Name_new_prf.Text, prg_id));
            }
            catch (Exception z)
            {
                MessageBox.Show(z.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                string str = new SqlCommand("SELECT prgm_name FROM nefco.dbo.auth_prgm WHERE prgm_id=" + prg_id, connection).ExecuteScalar().ToString();
                MessageBox.Show("Профиль добавлен в программу " + str + ", проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
        }

        /// <summary>
        /// Строка запроса на добавление нового профиля
        /// </summary>
        /// <param name="name">наименование</param>
        /// <param name="prg_id">идентификатор программы</param>
        /// <returns></returns>
        public string NEW_PRF(string name, int prg_id)
        {
            string s = "INSERT INTO nefco.dbo.auth_prf(prgm_id, prf_name) VALUES(" + prg_id + ", '" + name + "')";
            return s;
        }

        /// <summary>
        /// Добавление нового профиля
        /// </summary>
        /// <param name="name">наименование</param>
        /// <param name="prg_id">идентификатор программы</param>
        /// <returns>идентификатор профиля</returns>
        public void _ADDPRF(string name, int prg_id)
        {
            try
            {
                SqlCommand command_ADDPRF = new SqlCommand(NEW_PRF(name, prg_id), connection);
                command_ADDPRF.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат добавления профиля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Определение идентификатор добавленного профиля
        /// </summary>
        /// <param name="name">наименование</param>
        /// <param name="prg_id">идентификатор программы</param>
        /// <returns>идентификатор профиля</returns>
        public int _PRF_ID(string name, int prg_id)
        {
            SqlCommand command_scope = new SqlCommand("SELECT prf_id FROM nefco.dbo.auth_prf WHERE prf_name='" + name + "' AND prgm_id=" + prg_id, connection);
            return Convert.ToInt32(command_scope.ExecuteScalar());
        }

        /// <summary>
        /// Добавление тэгов
        /// </summary>
        /// <param name="prf_id">идентификатор профиля</param>
        public void _PRF_TAGS(int prf_id)
        {
            try
            {
                foreach (object item in List_tags.SelectedItems)
                {
                    string str = item.ToString().Substring(item.ToString().IndexOf(':') + 2);
                    int r = Prgm_Tags.Where(x => x.Value == str).FirstOrDefault().Key;
                    SqlCommand command_ins_prf_tag = new SqlCommand("INSERT INTO nefco.dbo.auth_prf_tags(prf_id, tag, status_dependent) VALUES(" + prf_id + ",  " + r + ", 0)", connection);
                    command_ins_prf_tag.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат добавления тэгов", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
        }
    }
}
