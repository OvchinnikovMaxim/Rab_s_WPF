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
    /// Логика взаимодействия для Edit_prf_form.xaml
    /// </summary>
    public partial class Edit_prf_form : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private readonly int prg_id;
        private readonly int prf_id;
        private readonly string PRF_NAME;
        private readonly List<int> curr_tags;

        private DataTable dt_Prgm_tags;

        private SqlDataAdapter adapter_Prgm_tags;

        private readonly Dictionary<int, string> Prgm_Tags = new Dictionary<int, string>();

        public Edit_prf_form(TextBox text_server, Label label_status, int programm_id, string prf_name, List<int> tags, int prf)
        {
            InitializeComponent();
            PRF_NAME = prf_name;
            prf_id = prf;
            curr_tags = tags;
            prg_id = programm_id;
            connection = sc.Connection(text_server, label_status);
        }

        /// <summary>
        /// Активация кнопки для добавления нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Name_edit_prf_TextChanged(object sender, TextChangedEventArgs e)
        {
            Edit_prf.IsEnabled = !string.IsNullOrEmpty(Name_edit_prf.Text.ToString());
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name_edit_prf.Text = PRF_NAME;

            // загрузка списка тэгов
            adapter_Prgm_tags = new SqlDataAdapter(Profile_tags(prg_id), connection);
            dt_Prgm_tags = new DataTable();
            adapter_Prgm_tags.Fill(dt_Prgm_tags);
            for (int i = 0; i < dt_Prgm_tags.Rows.Count; i++)
            {
                List_tags.Items.Add(new ListBoxItem() { Content = dt_Prgm_tags.Rows[i][1].ToString() });
                Prgm_Tags.Add(Convert.ToInt32(dt_Prgm_tags.Rows[i][0].ToString()), dt_Prgm_tags.Rows[i][1].ToString());
            }

            // текущие выбранные тэги
            foreach (int item in Prgm_Tags.Keys)
            {
                for (int i = 0; i < curr_tags.Count; i++)
                {
                    if (curr_tags.ElementAt(i) == item)
                    {
                        string r = Prgm_Tags.FirstOrDefault(x => x.Key == item).Value;
                        foreach (ListBoxItem j in List_tags.Items)
                            if (j.Content.ToString().Contains(r))
                                j.IsSelected = true;
                    }
                }
            }

            /*foreach (int item in Prgm_Tags.Keys)
             *{
             *    for (int i = 0; i < curr_tags.Count; i++)
             *    {
             *        if (curr_tags.ElementAt(i) == item)
             *        {
             *            string r = Prgm_Tags.Where(x => x.Key == item).FirstOrDefault().Value;
             *            foreach (ListBoxItem j in List_tags.Items)
             *            {
             *                if (j.Content.ToString().Contains(r))
             *                {
             *                    j.IsSelected = true;
             *                }
             *            }
             *        }
             *    }
             *}
            */


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

        private void Edit_prf_Click(object sender, RoutedEventArgs e)
        {
            string s = PRF_NAME;
            _EDITPRF(Name_edit_prf.Text.ToString(), prf_id);

            _PRF_TAGS(prf_id);

            Close();
        }

        /// <summary>
        /// Строка запроса на добавление нового профиля
        /// </summary>
        /// <param name="name">наименование</param>
        /// <param name="prg_id">идентификатор программы</param>
        /// <returns></returns>
        public string EDIT_PRF(string name, int prf_id)
        {
            string s = "UPDATE nefco.dbo.auth_prf SET prf_name ='"+ name + "' WHERE prf_id =" + prf_id;
            return s;
        }

        /// <summary>
        /// Добавление нового профиля
        /// </summary>
        /// <param name="name">наименование</param>
        /// <param name="prg_id">идентификатор программы</param>
        /// <returns>идентификатор профиля</returns>
        public void _EDITPRF(string name, int prf_id)
        {
            try
            {
                SqlCommand command_EDITPRF = new SqlCommand(EDIT_PRF(name, prf_id), connection);
                command_EDITPRF.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат  профиля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Очищение и добавление тэгов
        /// </summary>
        /// <param name="prf_id">идентификатор профиля</param>
        public void _PRF_TAGS(int prf_id)
        {
            try
            {
                SqlCommand command_del_tags = new SqlCommand("DELETE FROM nefco.dbo.auth_prf_tags WHERE prf_id = " + prf_id + ";", connection);
                command_del_tags.ExecuteNonQuery();

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
