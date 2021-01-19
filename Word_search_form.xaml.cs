using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
