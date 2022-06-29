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
    /// Логика взаимодействия для Edit_user.xaml
    /// </summary>
    public partial class Edit_user : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private DataTable dt_regions;
        private DataTable dt_prf;
        private DataTable dt_distrs;

        private SqlDataAdapter adapter_regions;
        private SqlDataAdapter adapter_prf;
        private SqlDataAdapter adapter_distrs;

        private readonly int Zav;
        private readonly int prg_id;
        private readonly int curr_prf_id;
        private readonly List<int> curr_region;
        private readonly List<int> curr_distr;
        private readonly int USER_ID;
        private readonly string USER_NAME;

        private readonly Dictionary<int, string> edtUser_prf = new Dictionary<int, string>();
        private readonly Dictionary<int, string> region = new Dictionary<int, string>();
        private readonly Dictionary<int, string> contractors = new Dictionary<int, string>();

        public Edit_user(int zavod, TextBox text_server, Label label_status, int programm_id, int user_id, string user_name,int prf_id_curr, List<int> Region, List<int> Distr)
        {
            InitializeComponent();
            Zav = zavod;
            prg_id = programm_id;
            curr_prf_id = prf_id_curr;
            curr_region = Region;
            curr_distr = Distr;
            USER_ID = user_id;
            USER_NAME = user_name;
            connection = sc.Connection(text_server, label_status);
        }

        #region чекбокс Без ограничения по дистрибьюторам
        /// <summary>
        /// Поставлена галочка "Без ограничения по дистрибьюторам"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_all_distr_Checked(object sender, RoutedEventArgs e) => List_distrs.IsEnabled = false;

        /// <summary>
        /// Убрана галочка "Без ограничения по дистрибьюторам"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_all_distr_Unchecked(object sender, RoutedEventArgs e) => List_distrs.IsEnabled = true;
        #endregion

        /// <summary>
        /// Поиск контрагента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string search_contr = text_search.Text.ToString().ToUpper();
            foreach (var i in contractors.Values.Where(i => i.ToUpper().Contains(search_contr)))
            {
                for (int z = 0; z < List_distrs.Items.Count; z++)
                    if (((ListBoxItem)List_distrs.Items[z]).Content.ToString() == i)
                        List_distrs.ScrollIntoView(List_distrs.Items[z]);
            }
        }

        #region Выделение всех контрагентов
        /// <summary>
        /// "Выделить всех"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_distrs_Click(object sender, RoutedEventArgs e) => List_distrs.SelectAll();

        /// <summary>
        /// Снять выделение со всех
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void None_distrs_Click(object sender, RoutedEventArgs e) => List_distrs.SelectedItem = null;
        #endregion

        private void Edit_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int prf_k = edtUser_prf.Where(x => x.Value == combo_prf.Text.ToString()).FirstOrDefault().Key;
                int user_id_k = USER_ID;

                string user_k = new SqlCommand("SELECT user_id FROM nefco.dbo.user_inf WHERE id=" + user_id_k, connection).ExecuteScalar().ToString();
                

                _EDITUSER(user_k, curr_prf_id, prf_k);
                _USER_REGION(user_id_k);

                if (List_distrs.IsEnabled)
                {
                    _USER_CONTRACTOR(user_k, prf_k, user_id_k, 0);
                }
                else
                {
                    _USER_CONTRACTOR(user_k, prf_k, user_id_k, 1);
                }
            }
            catch (Exception z)
            {
                MessageBox.Show(z.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                string str = new SqlCommand("SELECT prgm_name FROM nefco.dbo.auth_prgm WHERE prgm_id=" + prg_id, connection).ExecuteScalar().ToString();
                MessageBox.Show("Пользователь изменен в программе "+ str + ", проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
        }

        private void Cansel_Click(object sender, RoutedEventArgs e) => Close();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Label_edit_name_user.Content = USER_NAME;



            // загрузка списка регионов
            adapter_regions = new SqlDataAdapter(Regions(), connection);
            dt_regions = new DataTable();
            adapter_regions.Fill(dt_regions);
            for (int i = 0; i < dt_regions.Rows.Count; i++)
            {
                list_region.Items.Add(new ListBoxItem() { Content = dt_regions.Rows[i][1].ToString() });
                region.Add(Convert.ToInt32(dt_regions.Rows[i][0].ToString()), dt_regions.Rows[i][1].ToString());
            }

            // текущие выбранные регионы
            foreach (int item in region.Keys)
            {
                for (int i = 0; i < curr_region.Count; i++)
                {
                    if (curr_region.ElementAt(i) == item)
                    {
                        string r = region.FirstOrDefault(x => x.Key == item).Value;
                        foreach (ListBoxItem j in list_region.Items)
                            if (j.Content.ToString().Contains(r))
                                j.IsSelected = true;
                    }
                }
            }

            /*foreach (int item in region.Keys)
             *{
             *    for (int i = 0; i < curr_region.Count; i++)
             *    {
             *        if (curr_region.ElementAt(i) == item)
             *        {
             *            string r = region.Where(x => x.Key == item).FirstOrDefault().Value;
             *            foreach (ListBoxItem j in list_region.Items)
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

            // загрузка списка профилей
            adapter_prf = new SqlDataAdapter(Profiles(prg_id), connection);
            dt_prf = new DataTable();
            adapter_prf.Fill(dt_prf);
            for (int i = 0; i < dt_prf.Rows.Count; i++)
            {
                combo_prf.Items.Add(dt_prf.Rows[i][1].ToString());
                edtUser_prf.Add(Convert.ToInt32(dt_prf.Rows[i][0].ToString()), dt_prf.Rows[i][1].ToString());
                // текущий профиль
                if (Convert.ToInt32(dt_prf.Rows[i][0].ToString())== curr_prf_id)
                {
                    combo_prf.SelectedIndex = i;
                }
            }

            #region загрузка списка контрагентов
            // Контрагенты без региона
            adapter_distrs = new SqlDataAdapter(No_region_distrs(Zav), connection);
            dt_distrs = new DataTable();
            adapter_distrs.Fill(dt_distrs);
            for (int i = 0; i < dt_distrs.Rows.Count; i++)
            {
                List_distrs.Items.Add(new ListBoxItem() { Content = dt_distrs.Rows[i][1].ToString() });
                contractors.Add(Convert.ToInt32(dt_distrs.Rows[i][0].ToString()), dt_distrs.Rows[i][1].ToString());
            }

            // Контрагенты с регионом
            foreach (int i in region.Keys)
            {
                List_distrs.Items.Add(new ListBoxItem() { Content = "-------------------------------" + region.Where(x => x.Key == i).FirstOrDefault().Value.ToUpper() });

                adapter_distrs = new SqlDataAdapter(Distrs(i, Zav), connection);
                dt_distrs = new DataTable();
                adapter_distrs.Fill(dt_distrs);
                for (int j = 0; j < dt_distrs.Rows.Count; j++)
                {
                    List_distrs.Items.Add(new ListBoxItem() { Content = dt_distrs.Rows[j][1].ToString() });
                    if (Convert.ToInt32(dt_distrs.Rows[j][0].ToString()) != 0)
                    {
                        contractors.Add(Convert.ToInt32(dt_distrs.Rows[j][0].ToString()), dt_distrs.Rows[j][1].ToString());
                    }
                }
            }
            #endregion
            
            foreach (int item in contractors.Keys)
            {
                for (int i = 0; i < curr_distr.Count; i++)
                {
                    if (curr_distr.ElementAt(i) == item)
                    {
                        string r = contractors.Where(x => x.Key == item).FirstOrDefault().Value;
                        foreach (ListBoxItem j in List_distrs.Items)
                        {
                            if (j.Content.ToString().Contains(r))
                            {
                                j.IsSelected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Список регионов
        /// </summary>
        /// <returns></returns>
        public string Regions()
        {
            string q = @"SELECT
                          region_id2,
                          region_name
                        FROM
                          nefco.dbo.region2";
            return q;
        }

        /// <summary>
        /// Список профилей выбранной программы
        /// </summary>
        /// <param name="programm_id">идентификатор программы</param>
        /// <returns></returns>
        public string Profiles(int programm_id)
        {
            string q = @"SELECT
                          prf_id,
                          prf_name
                        FROM
                          nefco.dbo.auth_prf
                        WHERE prgm_id=" + programm_id;
            return q;
        }

        /// <summary>
        /// Контрагенты на регионе
        /// </summary>
        /// <param name="region_id">код региона</param>
        /// <param name="zavod">код завода</param>
        /// <returns></returns>
        public string Distrs(int region_id, int zavod)
        {
            string q = @"SELECT
                          c.id as distr_id,
                          c.name + ' (' + ca.distr_id + ')' AS distr
                        FROM
                          nefco.dbo.co_contractor c
                           INNER JOIN nefco.dbo.co_contractor_attr_customer ca ON c.id = ca.contractor_id
                        WHERE
                          c.active = 1 AND
                          c.del = 0 AND
                          c.class IN (3,5,7,9,8,10,11,6,18) AND
                          ca.region_id2 = " + region_id + " and c.factory_id= " + zavod + " order by c.name";
            return q;
        }

        /// <summary>
        /// Контрагенты без региона
        /// </summary>
        /// <param name="zavod"></param>
        /// <returns></returns>
        public string No_region_distrs(int zavod)
        {
            string q = @"SELECT
                          c.id as distr_id,
                          c.name AS distr
                        FROM
                          nefco.dbo.co_contractor c
                            LEFT JOIN nefco.dbo.co_contractor_attr_customer ca ON c.id = ca.contractor_id
                        WHERE
                          c.active = 1 AND
                          c.del = 0 AND
                          c.class IN (6) 
                          and c.parent is null
                          and c.factory_id= " + zavod + " order by c.name";
            return q;
        }
               
        #region запросы для изменения пользователя
        /// <summary>
        /// Строка запроса на добавление нового пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="prf_id">идентификатор профиля</param>
        /// <returns></returns>
        public string EditUser_prf(string login, int prf_id, int new_prf_id)
        {
            string q = @"UPDATE
                          nefco.dbo.auth_usr_prf
                          set prf_id =" + new_prf_id + " WHERE usr_id='" + login + "' AND prf_id =" + prf_id;
            return q;
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="prf_id">идентификатор профиля</param>
        public void _EDITUSER(string login, int prf_id, int new_prf_id)
        {
            try
            {
                SqlCommand command_ADDUSER = new SqlCommand(EditUser_prf(login, prf_id, new_prf_id), connection);
                command_ADDUSER.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Очищение и добавление закрепления регионов за пользователем
        /// </summary>
        /// <param name="user_id">идентификатор пользователя</param>
        public void _USER_REGION(int user_id)
        {
            try
            {
                SqlCommand command_del_user_region = new SqlCommand("DELETE FROM nefco.dbo.auth_user_region WHERE user_id = " + user_id+";", connection);
                command_del_user_region.ExecuteNonQuery();

                if (list_region.SelectedItems.Count > 0)
                {
                    foreach (object item in list_region.SelectedItems)
                    {
                        string s = item.ToString().Substring(item.ToString().IndexOf(':') + 1).Trim();
                        int r = region.Where(x => x.Value.Contains(s)).FirstOrDefault().Key;
                        SqlCommand command_insert_user_region = new SqlCommand("INSERT INTO nefco.dbo.auth_user_region (user_id, region_id) VALUES (" + user_id + ", '" + r + "');", connection);
                        command_insert_user_region.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Очищение и добавление закрепления контрагентов за пользователем
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="prf_id">идентификатор профиля</param>
        /// <param name="user_id">идентификатор пользователя</param>
        /// <param name="all_distr">активность галочки "Без ограничения по дистрибьюторам"</param>
        public void _USER_CONTRACTOR(string login, int prf_id, int user_id, int all_distr)
        {
            try
            {
                SqlCommand command_del_user_contractor = new SqlCommand(@"DELETE
                                                    FROM
                                                      nefco.dbo.auth_user_contractor
                                                    WHERE
                                                      user_id = " + user_id +
                                                              "UPDATE auth_usr_prf SET all_distr = " + all_distr + " WHERE usr_id = '" + login + "' AND prf_id = " + prf_id + ";", connection);
                command_del_user_contractor.ExecuteNonQuery();

                if (List_distrs.SelectedItems.Count > 0)
                {
                    foreach (object item in List_distrs.SelectedItems)
                    {
                        string s = item.ToString().Substring(item.ToString().IndexOf(':') + 1).Trim();
                        int d = contractors.Where(x => x.Value.Contains(s)).FirstOrDefault().Key;
                        if (d != 0)
                        {
                            SqlCommand command_insert_user_contractor = new SqlCommand("INSERT INTO nefco.dbo.auth_user_contractor (user_id, contractor_id) VALUES (" + user_id + ", '" + d + "');", connection);
                            command_insert_user_contractor.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

       
    }
}
