using SQLquerys;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для New_attr_form.xaml
    /// </summary>
    public partial class New_attr_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public New_attr_form()
        {
            InitializeComponent();

            combo_attr_type.SelectedIndex = 0;
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

        private void btn_new_attr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zav;
                int attr = 0;

                zav = combo_zavod.SelectedIndex == 0 ? 1 : 2;

                switch (combo_attr_type.SelectedIndex)
                {
                    case 0:
                        attr = 21; //Категория
                        break;
                    case 1:
                        attr = 22; //Бренд
                        break;
                    case 2:
                        attr = 23; //Серия
                        break;
                    case 3:
                        attr = 24; //Назначение
                        break;
                    case 4:
                        attr = 25; //Аромат/направление
                        break;
                    case 5:
                        attr = 26; //Бренд конкурента
                        break;
                    case 6:
                        attr = 27; //Тип упаковки
                        break;
                    case 7:
                        attr = 28; //Серия конкурента
                        break;
                    default:
                        MessageBox.Show("Не выбран атрибут", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }

                string name = attr_name.Text;
                name = name.Replace("'", "`");

                SqlCommand new_atr_mat = new SqlCommand(q.new_material_attr(name, attr, zav), connection);
                new_atr_mat.ExecuteNonQuery();

                MessageBox.Show("Атрибут добавлен", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
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
