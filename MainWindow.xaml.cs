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

            label_hello.Content = "Привет тебе" + '\n' + Environment.UserName;

            #region анимация кодом
            /*DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 325;
            btnAnimation.Duration = TimeSpan.FromSeconds(3);
            btn_create_tt.BeginAnimation(Button.WidthProperty, btnAnimation);*/
            #endregion
        }

        #region Запуск соответствующих форм
        private void btn_sql_searching_Click(object sender, RoutedEventArgs e)
        {
            sql_searching sqlSearching = new sql_searching();
            sqlSearching.Show();
        }

        private void btn_create_tt_Click(object sender, RoutedEventArgs e)
        {
            Create_TT_form create_TT = new Create_TT_form();
            create_TT.Show();
        }

        private void btn_update_sales_Click(object sender, RoutedEventArgs e)
        {
            Update_sales_form update_Sales = new Update_sales_form();
            update_Sales.Show();
        }

        private void btn_work_sql_Click(object sender, RoutedEventArgs e)
        {
            WorkSQL_form workSQL = new WorkSQL_form();
            workSQL.Show();
        }
        #endregion

        #region Анимации кнопок
        private void btn_sql_searching_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_sql_searching.RenderTransform = rt;

            text_btn_search.Text = "*" + "Пользователь программ на делфи";
            text_btn_search.Text += '\n' + "*" + "Какие программы на делфи у данного пользователя";
            text_btn_search.Text += '\n' + "*" + "Пользователь NCSD";
            text_btn_search.Text += '\n' + "*" + "Информация у дистрибьюторе";
            text_btn_search.Text += '\n' + "*" + "Логины для дистрибьюторов";
            text_btn_search.Text += '\n' + "*" + "Продукция в последнем прайсе дистра";
            text_btn_search.Text += '\n' + "*" + "Список прайсов дистрибьютора";
            text_btn_search.Text += '\n' + "*" + "Продукция в прайс-листе";
            text_btn_search.Text += '\n' + "*" + "Поиск по базе сервис деска";
            text_btn_search.Text += '\n' + "*" + "Поиск роли для отчета на сайте";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_sql_searching_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_sql_searching.RenderTransform = rt;

            text_btn_search.Text = "Поиск по базе";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_update_sales_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_update_sales.RenderTransform = rt;

            text_btn_update_sales.Text = "*" + "Проверка продаж";
            text_btn_update_sales.Text += '\n' + "*" + "Обработка продаж";
            text_btn_update_sales.Text += '\n' + "*" + "Переобработка продаж";
            text_btn_update_sales.Text += '\n' + "*" + "Пометка продаж на удаление в базе UNITY";
            text_btn_update_sales.Text += '\n' + "*" + "Поиск дистрибьютора по наименованию";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_update_sales_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_update_sales.RenderTransform = rt;

            text_btn_update_sales.Text = "Обработка продаж";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_work_sql_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_work_sql.RenderTransform = rt;

            text_btn_work_sql.Text = "*" + "Новый дистрибьютор";
            text_btn_work_sql.Text += '\n' + "*" + "Добавление/изменение транспортной";
            text_btn_work_sql.Text += '\n' + "*" + "Новый пользователь программ на делфи";
            text_btn_work_sql.Text += '\n' + "*" + "Копирование ТТ с сектора на сектор";
            text_btn_work_sql.Text += '\n' + "*" + "Удаление/Восстановление заказа в ZAKAZ_HAT";
            text_btn_work_sql.Text += '\n' + "*" + "Новый атрибут для материалов";
            text_btn_work_sql.Text += '\n' + "*" + "Удаление остатков";
            text_btn_work_sql.Text += '\n' + "*" + "Новый класс прайс-листа";
            text_btn_work_sql.Text += '\n' + "*" + "Работа с рейсами";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_work_sql_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_work_sql.RenderTransform = rt;

            text_btn_work_sql.Text = "Работа с базой";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);

        }

        private void btn_create_tt_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_create_tt.RenderTransform = rt;

            text_btn_create_tt.Text = "Загрузка заполненного по шаблону файла, для создания клиентских карточек";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void btn_create_tt_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation() { From = 0, To = 360, Duration = TimeSpan.FromMilliseconds(100), RepeatBehavior = new RepeatBehavior(1) };
            RotateTransform rt = new RotateTransform() { CenterX = 165, CenterY = 143 };
            this.btn_create_tt.RenderTransform = rt;

            text_btn_create_tt.Text = "Заведение клиентских карточек";

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        } 
        #endregion
    }
}
