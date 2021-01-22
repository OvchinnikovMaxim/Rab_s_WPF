using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
//для анимации
using System.Windows.Media.Animation;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            label_hello.Content = "Hello" + '\n' + Environment.UserName;

            #region анимация кодом
            /*DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 325;
            btnAnimation.Duration = TimeSpan.FromSeconds(3);
            btn_create_tt.BeginAnimation(Button.WidthProperty, btnAnimation);*/
            #endregion
        }

        private void btn_sql_searching_Click(object sender, RoutedEventArgs e)
        {
            
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior=new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_sql_searching.RenderTransform = rt;

            if (text_btn_search.Text == "Поиск по базе")
            {
                text_btn_search.Text = "*" + "Пользователь программ на делфи";
                text_btn_search.Text += '\n' + "*" + "Какие программы на делфи у данного пользователя";
                text_btn_search.Text += '\n' + "*" + "Пользователь NCSD";
                text_btn_search.Text += '\n' + "*" + "Информация у дистрибьюторе";
                text_btn_search.Text += '\n' + "*" + "Логины для дистрибьюторов";
                text_btn_search.Text += '\n' + "*" + "Продукция в последнем прайсе дистра";
                text_btn_search.Text += '\n' + "*" + "Список прайсов дистрибьютора";
                text_btn_search.Text += '\n' + "*" + "Продукция в прайс-листе";
                text_btn_search.Text += '\n' + "*" + "Поиск по базе сервис деска";
            }
            else
            {
                text_btn_search.Text = "Поиск по базе";
            }
            
            rt.BeginAnimation(RotateTransform.AngleProperty, da);

            //sql_searching sqlSearching = new sql_searching();
            //sqlSearching.Show();
        }

        private void btn_sql_searching_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sql_searching sqlSearching = new sql_searching();
            sqlSearching.Show();
        }

        private void btn_create_tt_Click(object sender, RoutedEventArgs e)
        {
            Create_TT_form create_TT = new Create_TT_form();
            create_TT.Show();
        }

    }
}
