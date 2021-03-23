using System.Windows;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для Word_search_form.xaml
    /// </summary>
    public partial class Word_search_form : Window
    {
        public Word_search_form()
        {
            InitializeComponent();
        }

        private void btn_text_ok_Click(object sender, RoutedEventArgs e)
        {
            sql_searching sq = this.Owner as sql_searching;
            sq.list_search_word.Items.Add(text_search.Text);

            Close();
        }
    }
}
