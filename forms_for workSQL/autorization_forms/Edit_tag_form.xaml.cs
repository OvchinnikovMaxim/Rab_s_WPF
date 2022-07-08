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
    /// Логика взаимодействия для Edit_tag_form.xaml
    /// </summary>
    public partial class Edit_tag_form : Window
    {
        private readonly SqlConnection connection;

        private readonly Serv_conn sc = new Serv_conn();

        private readonly int tag_id;
        private readonly string tag_name;
        public Edit_tag_form(TextBox text_server, Label label_status, int tag, string tag_n)
        {
            InitializeComponent();
            tag_id = tag;
            tag_name = tag_n;
            connection = sc.Connection(text_server, label_status);
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Name_edit_tag_TextChanged(object sender, TextChangedEventArgs e)
        {
            Edit_tag.IsEnabled = !string.IsNullOrEmpty(Name_edit_tag.Text.ToString());
        }

        private void Edit_tag_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                SqlCommand command_edit_tag = new SqlCommand("UPDATE nefco.dbo.auth_prgm_tags SET tag_name = '" + Name_edit_tag.Text + "' WHERE tag =" + tag_id, connection);
                command_edit_tag.ExecuteNonQuery();

            }
            catch (Exception z)
            {
                MessageBox.Show(z.Message.ToString(), "Результат", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MessageBox.Show("Тэг изменен, проверьте", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name_edit_tag.Text = tag_name;
        }
    }
}
