using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace rab_stol.forms_for_workSQL.autorization_forms
{
    /// <summary>
    /// Логика взаимодействия для New_tag_form.xaml
    /// </summary>
    public partial class New_tag_form : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private readonly int prg_id;

        public New_tag_form(TextBox text_server, Label label_status, int programm_id)
        {
            InitializeComponent();
            prg_id = programm_id;
            connection = sc.Connection(text_server, label_status);
        }

        private void Name_new_tag_TextChanged(object sender, TextChangedEventArgs e)
        {
            Add_tag.IsEnabled = !string.IsNullOrEmpty(Name_new_tag.Text.ToString());
        }

        private void Add_tag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand command_new_tag = new SqlCommand("INSERT INTO nefco.dbo.auth_prgm_tags (prgm_id, tag_name) VALUES (" + prg_id + ", '" + Name_new_tag.Text + "')", connection);
                command_new_tag.ExecuteNonQuery();
            }
            catch (Exception z)
            {
                MessageBox.Show(z.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                string str = new SqlCommand("SELECT prgm_name FROM nefco.dbo.auth_prgm WHERE prgm_id=" + prg_id, connection).ExecuteScalar().ToString();
                MessageBox.Show("Тэг добавлен в программу " + str + ", проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
