using rab_stol.forms_for_workSQL;
using System.Windows;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для WorkSQL_form.xaml
    /// </summary>
    public partial class WorkSQL_form : Window
    {
        public WorkSQL_form()
        {
            InitializeComponent();
        }

        #region вызов соответствующей формы
        private void btn_new_distr_Click(object sender, RoutedEventArgs e)
        {
            New_distr_form distr_Form = new New_distr_form();
            distr_Form.Show();
        }

        private void btn_new_tek_Click(object sender, RoutedEventArgs e)
        {
            New_tek_form tek_Form = new New_tek_form();
            tek_Form.Show();
        }

        private void btn_new_user_inf_Click(object sender, RoutedEventArgs e)
        {
            New_user_inf_form user_Inf_Form = new New_user_inf_form();
            user_Inf_Form.Show();
        }

        private void btn_copyTT_Click(object sender, RoutedEventArgs e)
        {
            CopyTT_form copyTT = new CopyTT_form();
            copyTT.Show();
        }

        private void btn_delZAKAZ_in_ZAKAZHAT_Click(object sender, RoutedEventArgs e)
        {
            DelZAKAZ_in_ZAKAZHAT_form delZAKAZ_In = new DelZAKAZ_in_ZAKAZHAT_form();
            delZAKAZ_In.Show();
        }

        private void btn_new_attr_Click(object sender, RoutedEventArgs e)
        {
            New_attr_form attr_Form = new New_attr_form();
            attr_Form.Show();
        }

        private void btn_class_price_Click(object sender, RoutedEventArgs e)
        {
            New_class_price_form class_Price_Form = new New_class_price_form();
            class_Price_Form.Show();
        }        

        private void btn_del_ost_Click(object sender, RoutedEventArgs e)
        {
            Del_ost_form del_Ost = new Del_ost_form();
            del_Ost.Show();
        }
        
        private void btn_tc_trip_Click(object sender, RoutedEventArgs e)
        {
            Work_trip trip = new Work_trip();
            trip.Show();
        }
        #endregion
    }
}
