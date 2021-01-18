using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            label_hello.Text = "Hello" + '\n' + Environment.UserName;

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

            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(300), RepeatBehavior=new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 50, CenterY = 50 };
            this.btn_sql_searching.RenderTransform = rt;

            if (btn_sql_searching.Content.ToString() == "привет всем")
            {
                btn_sql_searching.Content = "Поиск по базе";
            }
            else
            {
                btn_sql_searching.Content = "привет всем";
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

        private void btn_sql_searching_MouseEnter(object sender, MouseEventArgs e)
        {
            /*for (int i = 0; i < 2; i++)
            {
                
            }*/
                
                
            
        }

        /*private void btn_sql_searching_MouseEnter(object sender, MouseEventArgs e)
        {
            
            
            DoubleAnimation btnAnimation1 = new DoubleAnimation();
            btnAnimation1.From = 0;
            btnAnimation1.To = 45;
            btnAnimation1.Duration = TimeSpan.FromSeconds(3);
            btn_sql_searching.BeginAnimation(RotateTransform.AngleProperty, btnAnimation1);
            //btn_sql_searching.BeginAnimation(Button.WidthProperty, btnAnimation1);
             /*if(btn_sql_searching.ActualWidth==0)
            {
                DoubleAnimation btnAnimation2 = new DoubleAnimation();
                btn_sql_searching.Content = "привет всем";
                btnAnimation2.From = 0;
                btnAnimation2.To = 325;
                btnAnimation2.Duration = TimeSpan.FromSeconds(3);
                btn_sql_searching.BeginAnimation(Button.WidthProperty, btnAnimation2);
            }
           
        }*/
    }
}
