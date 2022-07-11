using rab_stol.forms_for_workSQL.autorization_forms;
using SQLquerys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для Autorization_form.xaml
    /// </summary>
    public partial class Autorization_form : Window
    {
        private SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        private DataTable dt_users;
        private DataTable dt_prf;
        private DataTable dt_tags;

        private SqlDataAdapter adapter_users;
        private SqlDataAdapter adapter_prf;
        private SqlDataAdapter adapter_tags;


        public Autorization_form()
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

        /// <summary>
        /// Соединение пользователя администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_connect_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand user = new SqlCommand("SELECT id FROM nefco.dbo.user_inf WHERE user_id = '" + login_user.Text + "' and pass = '" + pass_user.Text + "'", connection);
                int user_id = Convert.ToInt32(user.ExecuteScalar().ToString());

                SqlCommand auth_register_user_session = new SqlCommand("exec nefco.dbo.prc_auth_register_user_session " + user_id, connection);
                auth_register_user_session.ExecuteNonQuery();

                SqlCommand auth_user_session_param_factory_clear = new SqlCommand("exec nefco.dbo.prc_auth_user_session_param_factory_clear " + user_id, connection);
                auth_user_session_param_factory_clear.ExecuteNonQuery();

                SqlCommand auth_user_session_param_factory_add = new SqlCommand("exec nefco.dbo.prc_auth_user_session_param_factory_add " + user_id + ", 1", connection);
                auth_user_session_param_factory_add.ExecuteNonQuery();

                login_user.IsEnabled = false;
                pass_user.IsEnabled = false;
                btn_connect_user.IsEnabled = false;
                status_user.Foreground = Brushes.Green;
                status_user.Content = "Соединено";

                check.IsEnabled = true;
                combo_prog.IsEnabled = true;
                combo_zavod.IsEnabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("Ссылка на объект не указывает на экземпляр объекта."))
                {
                    MessageBox.Show("Неверный логин или пароль","Ошибка",MessageBoxButton.OK ,MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// Отключение соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            login_user.IsEnabled = true;
            pass_user.IsEnabled = true;
            btn_connect_user.IsEnabled = true;
            status_user.Foreground = Brushes.Red;
            status_user.Content = "Нет соединения";
            usr_prf_tg.IsEnabled = false;
            check.IsEnabled = false;
            combo_prog.IsEnabled = false;
            combo_zavod.IsEnabled = false;
        }

        /// <summary>
        /// Проверка списков пользователей, профилей, тагов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            /* ЗАВОД
             * индекс = 0 - НК = 1
             * индекс = 1 - КЖК = 2
             * -----------
             * ПРОГРАММА
             * индекс = 0 - Редактор справочников = 6
             * индекс = 1 - Персонал = 45
             * индекс = 2 - Администрирование = 7
             */

            int programm_id = 0, zavod;

            usr_prf_tg.IsEnabled = true;

            zavod = combo_zavod.SelectedIndex == 0 ? 1 : 2;

            switch (combo_prog.SelectedIndex)
            {
                case 0:
                    programm_id = 6;
                    break;
                case 1:
                    programm_id = 45;
                    break;
                case 2:
                    programm_id = 7;
                    break;
                default:
                    usr_prf_tg.IsEnabled = false;
                    MessageBox.Show("Программа не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            // Отображение пользователей
            adapter_users = new SqlDataAdapter(Users(programm_id, zavod), connection);
            dt_users = new DataTable();
            adapter_users.Fill(dt_users);
            data_users.ItemsSource = dt_users.DefaultView;

            // Отображение профилей
            adapter_prf = new SqlDataAdapter(Profiles(programm_id), connection);
            dt_prf = new DataTable();
            adapter_prf.Fill(dt_prf);
            data_prf.ItemsSource = dt_prf.DefaultView;

            // Отображение тагов
            adapter_tags = new SqlDataAdapter(Tags(programm_id), connection);
            dt_tags = new DataTable();
            adapter_tags.Fill(dt_tags);
            data_tags.ItemsSource = dt_tags.DefaultView;
        }

        #region Списки пользователей, профилей, тагов
        /// <summary>
        /// Текст запроса списка пользователей
        /// </summary>
        /// <param name="programm_id">код программы</param>
        /// <param name="zavod">код завода</param>
        /// <returns>Текст запроса</returns>
        public string Users(int programm_id, int zavod)
        {
            string q = @"SELECT aprg.prgm_name,
                          aup.usr_id,
                          ui.id,
                          ui.pass,
                          aup.prf_id,
                          ui.user_name,
                          ap.prf_name
                        FROM
                          nefco.dbo.auth_usr_prf aup,
                          nefco.dbo.auth_prf ap,
                          nefco.dbo.user_inf ui,
                          nefco.dbo.auth_factory af,
                          nefco.dbo.auth_prgm aprg  
                        WHERE
                          aup.usr_id = ui.user_id AND
                          aup.prf_id = ap.prf_id AND
                          ap.prgm_id=aprg.prgm_id and
                          ap.prgm_id = " + programm_id +
                          @" and af.user_id = ui.id
                          and af.factory_id = " + zavod +
                          @" and ui.date_end is null
                        ORDER BY
                          ui.user_name";
            return q;
        }

        /// <summary>
        /// Текст запроса списка профилей
        /// </summary>
        /// <param name="programm_id">код программы</param>
        /// <returns>Текст запроса</returns>
        public string Profiles(int programm_id)
        {
            string q = @"SELECT
                          prf_id,
                          prf_name
                        FROM
                          nefco.dbo.auth_prf
                        WHERE
                          prgm_id = " + programm_id;
            return q;
        }

        /// <summary>
        /// Текст запроса списка тагов
        /// </summary>
        /// <param name="programm_id">код программы</param>
        /// <returns>Текст запроса</returns>
        public string Tags(int programm_id)
        {
            string q = @"SELECT
                          tag,
                          tag_name
                        FROM
                          nefco.dbo.auth_prgm_tags
                        WHERE
                          prgm_id = " + programm_id;
            return q;
        }
        #endregion

        /// <summary>
        /// Только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        #region Работа с пользователями
        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_user_Click(object sender, RoutedEventArgs e)
        {
            int zavod = combo_zavod.SelectedIndex == 0 ? 1 : 2;
            int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

            /* передача данных в форму добавления нового пользователя
             * идентификатор завода
             * адрес сервера
             * статус подключения
             * идентификатор программы
             */
            ////////////////////////////////
            /* ЗАВОД
             * индекс = 0 - НК = 1
             * индекс = 1 - КЖК = 2             
             */

            New_user new_User = new New_user(zavod, text_server, label_status, programm_id);
            new_User.Show();
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_user_Click(object sender, RoutedEventArgs e)
        {
            int zavod = combo_zavod.SelectedIndex == 0 ? 1 : 2;
            int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

            /* передача данных в форму добавления нового пользователя
             * идентификатор завода
             * адрес сервера
             * статус подключения
             * идентификатор программы
             * идентификатор пользователя
             * ФИО пользователя
             * профиль пользователя
             * список выбранных регионов
             * список выбранных дистрибьюторов
             */
            ////////////////////////////////
            /* ЗАВОД
             * индекс = 0 - НК = 1
             * индекс = 1 - КЖК = 2
             */
            try
            {
                #region Выбранные данные
                // ФИО выбранного пользователя
                SqlCommand curr_user_name = new SqlCommand("SELECT user_name FROM nefco.dbo.user_inf WHERE id = " + Convert.ToInt32(Id_user.Text), connection);

                // Профиль выбранного пользователя
                SqlCommand curr_prf_id = new SqlCommand("SELECT prf_id FROM nefco.dbo.auth_prf WHERE prgm_id=" + programm_id + " and prf_id in (SELECT prf_id FROM nefco.dbo.auth_usr_prf WHERE usr_id= (select  user_id from nefco.dbo.user_inf where id =" + Convert.ToInt32(Id_user.Text) + "))", connection);

                // Выбранные регионы
                List<int> list_curr_region = new List<int>();

                SqlDataAdapter curr_user_region = new SqlDataAdapter("SELECT region_id FROM nefco.dbo.auth_user_region WHERE user_id=" + Convert.ToInt32(Id_user.Text), connection);
                DataTable dt_curr_user_region = new DataTable();
                curr_user_region.Fill(dt_curr_user_region);

                for (int i = 0; i < dt_curr_user_region.Rows.Count; i++)
                {
                    list_curr_region.Add(Convert.ToInt32(dt_curr_user_region.Rows[i][0]));
                }

                // Выбранные дистрибьюторы
                List<int> list_curr_distr = new List<int>();

                SqlDataAdapter curr_user_distr = new SqlDataAdapter("SELECT contractor_id FROM nefco.dbo.auth_user_contractor WHERE user_id=" + Convert.ToInt32(Id_user.Text), connection);
                DataTable dt_curr_user_distr = new DataTable();
                curr_user_distr.Fill(dt_curr_user_distr);
                for (int i = 0; i < dt_curr_user_distr.Rows.Count; i++)
                {
                    list_curr_distr.Add(Convert.ToInt32(dt_curr_user_distr.Rows[i][0]));
                } 
                #endregion

                Edit_user edit_User = new Edit_user(zavod, text_server, label_status, programm_id, Convert.ToInt32(Id_user.Text), curr_user_name.ExecuteScalar().ToString(), Convert.ToInt32(curr_prf_id.ExecuteScalar()), list_curr_region, list_curr_distr);
                edit_User.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

                SqlCommand user_name = new SqlCommand("SELECT user_name FROM nefco.dbo.user_inf WHERE id = " + Convert.ToInt32(Id_user.Text), connection);
                string prog = combo_prog.SelectedItem.ToString();

                MessageBoxResult boxResult = MessageBox.Show("Удалить пользователя '" + user_name.ExecuteScalar() + "' из '" + prog.Substring(prog.IndexOf(':') + 2) + "'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (boxResult == MessageBoxResult.Yes)
                {
                    string del_user = "DELETE FROM nefco.dbo.auth_usr_prf WHERE usr_id = (select user_id from nefco.dbo.user_inf where id = " + Convert.ToInt32(Id_user.Text) + ") AND prf_id = (select prf_id from nefco.dbo.auth_prf where prgm_id = " + programm_id + " and prf_id in (select prf_id FROM nefco.dbo.auth_usr_prf WHERE usr_id = (select user_id from nefco.dbo.user_inf where id = " + Convert.ToInt32(Id_user.Text) + ")))";
                    SqlCommand sql_del_user = new SqlCommand(del_user, connection);
                    sql_del_user.ExecuteNonQuery();

                    MessageBox.Show("Пользователь удален. Нажмите \"Проверить\"", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        #endregion

        #region Работа с профилями
        /// <summary>
        /// Добавление профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_prf_Click(object sender, RoutedEventArgs e)
        {
            int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

            New_prf_form new_Prf_Form = new New_prf_form(text_server, label_status, programm_id);
            new_Prf_Form.Show();
        }

        /// <summary>
        /// Редактирование профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_prf_Click(object sender, RoutedEventArgs e)
        {
            int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

            SqlCommand curr_prf_name = new SqlCommand("SELECT prf_name FROM nefco.dbo.auth_prf WHERE prgm_id=" + programm_id + " AND prf_id=" + Convert.ToInt32(Id_prf.Text), connection);

            // Выбранные тэги
            List<int> list_curr_tags = new List<int>();

            SqlDataAdapter curr_prf_tags = new SqlDataAdapter("SELECT tag FROM nefco.dbo.auth_prf_tags WHERE prf_id=" + Convert.ToInt32(Id_prf.Text), connection);
            DataTable dt_curr_prf_tags = new DataTable();
            curr_prf_tags.Fill(dt_curr_prf_tags);
            for (int i = 0; i < dt_curr_prf_tags.Rows.Count; i++)
            {
                list_curr_tags.Add(Convert.ToInt32(dt_curr_prf_tags.Rows[i][0]));
            }

            Edit_prf_form edit_Prf_Form = new Edit_prf_form(text_server, label_status, programm_id, curr_prf_name.ExecuteScalar().ToString(), list_curr_tags, Convert.ToInt32(Id_prf.Text));
            edit_Prf_Form.Show();
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_prf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand prf_name = new SqlCommand("SELECT prf_name FROM nefco.dbo.auth_prf WHERE prf_id = " + Convert.ToInt32(Id_prf.Text), connection);
                prf_name.ExecuteScalar();

                SqlCommand prf_user = new SqlCommand("SELECT usr_id FROM nefco.dbo.auth_usr_prf WHERE prf_id = " + Convert.ToInt32(Id_prf.Text), connection);

                var s = prf_user.ExecuteScalar();

                if (s != null)
                {
                    MessageBoxResult boxResult1 = MessageBox.Show("Имеются пользователи привязанные к данному профилю. Разорвать привязку?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (boxResult1 == MessageBoxResult.Yes)
                    {
                        string del_prf_user = "DELETE FROM nefco.dbo.auth_usr_prf WHERE prf_id = " + Convert.ToInt32(Id_prf.Text);
                        SqlCommand sql_del_prf_user = new SqlCommand(del_prf_user, connection);
                        sql_del_prf_user.ExecuteNonQuery();

                        string del_prf = "DELETE FROM nefco.dbo.auth_prf WHERE prf_id = " + Convert.ToInt32(Id_prf.Text);
                        SqlCommand sql_del_prf = new SqlCommand(del_prf, connection);
                        sql_del_prf.ExecuteNonQuery();

                        string del_prf_tags = "DELETE FROM nefco.dbo.auth_prf_tags WHERE prf_id = " + Convert.ToInt32(Id_prf.Text);
                        SqlCommand sql_del_prf_tags = new SqlCommand(del_prf_tags, connection);
                        sql_del_prf_tags.ExecuteNonQuery();

                        MessageBox.Show("Профиль удален. Нажмите \"Проверить\"", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    string del_prf = "DELETE FROM nefco.dbo.auth_prf WHERE prf_id = " + Convert.ToInt32(Id_prf.Text);
                    SqlCommand sql_del_prf = new SqlCommand(del_prf, connection);
                    sql_del_prf.ExecuteNonQuery();

                    string del_prf_tags = "DELETE FROM nefco.dbo.auth_prf_tags WHERE prf_id = " + Convert.ToInt32(Id_prf.Text);
                    SqlCommand sql_del_prf_tags = new SqlCommand(del_prf_tags, connection);
                    sql_del_prf_tags.ExecuteNonQuery();

                    MessageBox.Show("Профиль удален. Нажмите \"Проверить\"", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        #endregion

        #region Работа с тэгами
        /// <summary>
        /// Новый тэг для программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_tag_Click(object sender, RoutedEventArgs e)
        {
            int programm_id = PROGRAMM_ID(combo_prog.SelectedIndex);

            New_tag_form new_Tag_Form = new New_tag_form(text_server, label_status, programm_id);
            new_Tag_Form.Show();
        }

        /// <summary>
        /// Редактирование тэга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_tag_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand curr_tag_name = new SqlCommand("SELECT tag_name FROM nefco.dbo.auth_prgm_tags WHERE tag=" + Convert.ToInt32(Id_tags.Text), connection);

            Edit_tag_form edit_Tag_Form = new Edit_tag_form(text_server, label_status, Convert.ToInt32(Id_tags.Text), curr_tag_name.ExecuteScalar().ToString());
            edit_Tag_Form.Show();
        }

        /// <summary>
        /// Удаление тэга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_tag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand comm_tag_name = new SqlCommand("SELECT tag_name FROM nefco.dbo.auth_prgm_tags WHERE tag =" + Convert.ToInt32(Id_tags.Text), connection);

                MessageBoxResult boxResult1 = MessageBox.Show("Удалить тэг '" + comm_tag_name.ExecuteScalar().ToString() + "'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (boxResult1 == MessageBoxResult.Yes)
                {
                    SqlCommand com_del_tag = new SqlCommand("DELETE FROM nefco.dbo.auth_prgm_tags WHERE tag = " + Convert.ToInt32(Id_tags.Text), connection);
                    com_del_tag.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MessageBox.Show("Тэг удален. Нажмите \"Проверить\"", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        /// <summary>
        /// Выбор программы
        /// </summary>
        /// <param name="prog">выбранная программа</param>
        /// <returns>идентификатор программы: Редактор справочников = 6, Персонал = 45, Администрирование = 7</returns>
        public int PROGRAMM_ID(int prog)
        {
            /*ПРОГРАММА
             * индекс = 0 - Редактор справочников = 6
             * индекс = 1 - Персонал = 45
             * индекс = 2 - Администрирование = 7
             */

            int programm_id;
            switch (prog)
            {
                case 0:
                    programm_id = 6;
                    break;
                case 1:
                    programm_id = 45;
                    break;
                case 2:
                    programm_id = 7;
                    break;
                default:
                    programm_id = Convert.ToInt32(null);
                    break;
            }
            return programm_id;
        }
    }
}
