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
    /// Логика взаимодействия для New_user.xaml
    /// </summary>
    public partial class New_user : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private DataTable dt_users;
        private DataTable dt_regions;
        private DataTable dt_prf;
        private DataTable dt_distrs;

        private SqlDataAdapter adapter_users;
        private SqlDataAdapter adapter_regions;
        private SqlDataAdapter adapter_prf;
        private SqlDataAdapter adapter_distrs;

        private readonly int Zav;
        private readonly int prg_id;

        private readonly Dictionary<string, string> newUser = new Dictionary<string, string>();
        private readonly Dictionary<int, string> newUser_prf = new Dictionary<int, string>();
        private readonly Dictionary<int, string> newUser_id = new Dictionary<int, string>();
        private readonly Dictionary<int, string> region = new Dictionary<int, string>();
        private readonly Dictionary<int, string> contractors = new Dictionary<int, string>();

        public New_user(int zavod, TextBox text_server, Label label_status, int programm_id)
        {
            InitializeComponent();
            Zav = zavod;
            prg_id = programm_id;
            connection = sc.Connection(text_server, label_status);
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // загрузка списка пользователей
            adapter_users = new SqlDataAdapter(Users(Zav), connection);
            dt_users = new DataTable();
            adapter_users.Fill(dt_users);
            for (int i = 0; i < dt_users.Rows.Count; i++)
            {
                combo_new_user.Items.Add( dt_users.Rows[i][1].ToString());
                newUser.Add(dt_users.Rows[i][0].ToString(), dt_users.Rows[i][1].ToString());
                newUser_id.Add(Convert.ToInt32(dt_users.Rows[i][2].ToString()), dt_users.Rows[i][0].ToString());
            }

            // загрузка списка регионов
            adapter_regions = new SqlDataAdapter(Regions(), connection);
            dt_regions = new DataTable();
            adapter_regions.Fill(dt_regions);
            for (int i = 0; i < dt_regions.Rows.Count; i++)
            {
                list_region.Items.Add(dt_regions.Rows[i][1].ToString());
                region.Add(Convert.ToInt32(dt_regions.Rows[i][0].ToString()), dt_regions.Rows[i][1].ToString());
            }

            // загрузка списка профилей
            adapter_prf = new SqlDataAdapter(Profiles(prg_id), connection);
            dt_prf = new DataTable();
            adapter_prf.Fill(dt_prf);
            for (int i = 0; i < dt_prf.Rows.Count; i++)
            {
                combo_prf.Items.Add(dt_prf.Rows[i][1].ToString());
                newUser_prf.Add(Convert.ToInt32(dt_prf.Rows[i][0].ToString()), dt_prf.Rows[i][1].ToString());
            }

            #region загрузка списка контрагентов
            adapter_distrs = new SqlDataAdapter(No_region_distrs(Zav), connection);
            dt_distrs = new DataTable();
            adapter_distrs.Fill(dt_distrs);
            for (int i = 0; i < dt_distrs.Rows.Count; i++)
            {
                List_distrs.Items.Add(new ListBoxItem() { Content = dt_distrs.Rows[i][1].ToString() });
                contractors.Add(Convert.ToInt32(dt_distrs.Rows[i][0].ToString()), dt_distrs.Rows[i][1].ToString());
            }

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
        }

        private void Cansel_Click(object sender, RoutedEventArgs e) => Close();

        private void Add_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int prf_k = newUser_prf.Where(x => x.Value == combo_prf.Text.ToString()).FirstOrDefault().Key;
                string user_k = newUser.Where(x => x.Value == combo_new_user.Text.ToString()).FirstOrDefault().Key;
                int user_id_k = newUser_id.Where(x => x.Value == user_k).FirstOrDefault().Key;

                _ADDUSER(user_k, prf_k);
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
                MessageBox.Show("Пользователь добавлен в программу "+ prg_id + ", проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
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
        /// Список пользователей
        /// </summary>
        /// <param name="zavod">код завода</param>
        /// <returns></returns>
        public string Users(int zavod)
        {
            string q = @"SELECT
                          ui.user_id,
                          ui.user_name,
                          ui.id
                        FROM
                          nefco.dbo.user_inf ui
                          inner join nefco.dbo.auth_factory af on ui.id = af.user_id
                        WHERE
                            ui.user_name <> ''''
                          and af.factory_id = " + zavod + " ORDER BY ui.user_name";
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

        #region чекбокс Без ограничения по дистрибьюторам
        /// <summary>
        /// Убрана галочка "Без ограничения по дистрибьюторам"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_all_distr_Unchecked(object sender, RoutedEventArgs e) => List_distrs.IsEnabled = true;

        /// <summary>
        /// Поставлена галочка "Без ограничения по дистрибьюторам"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_all_distr_Checked(object sender, RoutedEventArgs e) => List_distrs.IsEnabled = false;
        #endregion

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

            /*foreach (string i in contractors.Values)
                if (i.ToUpper().Contains(search_contr))
                    for (int z = 0; z < List_distrs.Items.Count; z++)                 
                        if (((ListBoxItem)List_distrs.Items[z]).Content.ToString() == i)
                            List_distrs.ScrollIntoView(List_distrs.Items[z]);*/
        }

        #region запросы для заведения нового пользователя
        /// <summary>
        /// Строка запроса на добавление нового пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="prf_id">идентификатор профиля</param>
        /// <returns></returns>
        public string NewUser_prf(string login, int prf_id)
        {
            string q = @"INSERT INTO
                          nefco.dbo.auth_usr_prf
                          (usr_id, prf_id)
                        VALUES
                          ('" + login + "', " + prf_id + ")";
            return q;
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="prf_id">идентификатор профиля</param>
        public void _ADDUSER(string login, int prf_id)
        {
            try
            {
                SqlCommand command_ADDUSER = new SqlCommand(NewUser_prf(login, prf_id), connection);
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
                SqlCommand command_del_user_region = new SqlCommand("DELETE FROM nefco.dbo.auth_user_region WHERE user_id = " + user_id, connection);
                command_del_user_region.ExecuteNonQuery();

                if (list_region.SelectedItems.Count > 0)
                {
                    foreach (object item in list_region.SelectedItems)
                    {
                        int r = region.Where(x => x.Value == item.ToString()).FirstOrDefault().Key;
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
                        int d = contractors.Where(x => x.Value == item.ToString()).FirstOrDefault().Key;
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
