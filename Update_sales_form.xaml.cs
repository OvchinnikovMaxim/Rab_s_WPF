using SQLquerys;
using System;
using System.ComponentModel;
using System.Data;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol
{
    /// <summary>
    /// Логика взаимодействия для Update_sales_form.xaml
    /// </summary>
    public partial class Update_sales_form : Window
    {
        SqlConnection connection;

        readonly Query q = new Query();

        readonly Serv_conn sc = new Serv_conn();

        DateTime date1 = new DateTime(0, 0);

        //System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        System.Timers.Timer timer = new System.Timers.Timer();

        BackgroundWorker bw1 = new BackgroundWorker();
        private readonly BackgroundWorker backgroundWorker2 = new BackgroundWorker();


        public Update_sales_form()
        {
            InitializeComponent();

            date_begin.SelectedDate = DateTime.Now;
            date_end.SelectedDate = DateTime.Now;

            timer.Interval = 1000;
            //timer.Tick += new EventHandler(timerTick);
            //timer.Interval = new TimeSpan(0, 0, 1);
            timer.Elapsed += timerTick;
            // worker.DoWork += worker_DoWork;
            //bw1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker2.DoWork += backgroundWorker2_DoWork;
            bw1.DoWork += backgroundWorker1_DoWork;
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

        private void text_distrID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void btn_search_distr_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            SqlDataAdapter adapter;

            try
            {
                string query = @"DECLARE @distr VARCHAR(50) = '%" + distr_name.Text +
                        "%' ;" +
                        "SELECT d.contractor_id, d.*, ccd.fio, ccd.password, ccd.login, ccd.isMT, ccd.is_mt_sales, ccd.is_mt_ost " +
                        "FROM nefco.dbo.distr d LEFT JOIN nefco.dbo.client_card_distrpass ccd on d.distr_id=ccd.distr_id " +
                        "WHERE d.distr LIKE @distr";

                adapter = new SqlDataAdapter(query, connection);

                dt = new DataTable();
                adapter.Fill(dt);
                dataGR_NEFCO.ItemsSource = dt.DefaultView;

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

        private void btn_check_sales_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt_nefco, dt_unity;
            SqlDataAdapter adapter_nefco, adapter_unity;

            try
            {
                adapter_nefco = new SqlDataAdapter(q.select_sales_Rub_NEFCO(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate), connection);
                adapter_nefco.SelectCommand.CommandTimeout = 3600;

                dt_nefco = new DataTable();
                adapter_nefco.Fill(dt_nefco);
                dataGR_NEFCO.ItemsSource = dt_nefco.DefaultView;

                adapter_unity = new SqlDataAdapter(q.select_sales_Rub_UNITY(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate), connection);
                adapter_unity.SelectCommand.CommandTimeout = 3600;
                dt_unity = new DataTable();
                adapter_unity.Fill(dt_unity);
                dataGR_UNITY.ItemsSource = dt_unity.DefaultView;

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private  void btn_obrabotka_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                    date1 = new DateTime(0, 0);
                check_blocking.Visibility = Visibility.Hidden;
                label_time.Content = "00:00";
                
                //timer.Enabled = true;
                timer.Start();
                    //timer.IsEnabled = true;
                    //label_time.Content = "00:01";

                
                backgroundWorker2.RunWorkerAsync();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }



        private void btn_pereobr_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("1) Накладные в базе UNITY, пометятся как перепроведенные." + '\n' +
                                                "2) Детали и шапки накладных в базе NEFCO, пометятся на удаление." + '\n' +
                                             "3) Запуск обработки накладных из UNITY в NEFCO", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    date1 = new DateTime(0, 0);
                    check_blocking.Visibility = Visibility.Hidden;
                    label_time.Content = "00:00";
                    timer.Start();
                    //timer.IsEnabled = true;

                    bw1.RunWorkerAsync();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не был указан код дистрибьютора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        #region checkbox
        private void check_update_Checked(object sender, RoutedEventArgs e)
        {
            btn_update_UNITY.IsEnabled = true;
        }

        private void check_update_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_update_UNITY.IsEnabled = false;
        } 
        #endregion

        private void btn_update_UNITY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand updDET_unity = new SqlCommand(q.updateDETAILS_unity(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate), connection);
                updDET_unity.CommandTimeout = 3600;
                updDET_unity.ExecuteNonQuery();

                SqlCommand updDOC_unity = new SqlCommand(q.updateDOCS_unity(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate), connection);
                updDOC_unity.CommandTimeout = 3600;
                updDOC_unity.ExecuteNonQuery();

                MessageBox.Show("Накладные в базе unity помечены на удаление", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Не был указан код дистрибьютора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// метод, для объединения вызываемых функций для преобработки продаж
        /// </summary>
        /// <param name="contractor">Код дистрибьютора</param>
        /// <param name="begin">Дата начала периода</param>
        /// <param name="end">Дата конца периода</param>
        /// <param name="connection">подключение к серверу</param>
        void reload(int contractor, DateTime begin, DateTime end, SqlConnection connection)
        {
            SqlCommand actual = new SqlCommand(q.actualDOCS_unity(contractor, begin, end), connection);
            actual.CommandTimeout = 3600;
            actual.ExecuteNonQuery();

            SqlCommand updDET_nefco = new SqlCommand(q.updateDETAILS_nefco(contractor, begin, end), connection);
            updDET_nefco.CommandTimeout = 3600;
            updDET_nefco.ExecuteNonQuery();

            SqlCommand updDOC_nefco = new SqlCommand(q.updateDOCS_nefco(contractor, begin, end), connection);
            updDOC_nefco.CommandTimeout = 3600;
            updDOC_nefco.ExecuteNonQuery();

            SqlCommand exec_transfer_docs = new SqlCommand(q.TRANSFER_MT_DOCS_UNITY(contractor, begin, end), connection);
            exec_transfer_docs.CommandTimeout = 3600;
            exec_transfer_docs.ExecuteNonQuery();
        }

        private void timerTick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                date1 = date1.AddSeconds(1);
                //label_time.Content = "00:00";
                label_time.Content = date1.ToString("mm:ss");
            
            if (date1.Minute > 1)
            {
                check_blocking.Visibility = Visibility.Visible;
            }
            else
            {
                check_blocking.Visibility = Visibility.Hidden;
            }
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                try
                {
                    reload(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate, connection);

                    //timer.IsEnabled = false;

                    MessageBox.Show("Переобработка продаж завершена, проверьте продажи", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException ex)
                {
                    //timer.IsEnabled = false;
                    MessageBox.Show(ex.Message.ToString(), "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    //timer.IsEnabled = false;
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));
        }


        public  void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            /*btn_obrabotka.*/Dispatcher.BeginInvoke(new Action(delegate ()
            {
            SqlCommand exec_transfer_docs = new SqlCommand(q.TRANSFER_MT_DOCS_UNITY(Convert.ToInt32(text_distrID.Text), (DateTime)date_begin.SelectedDate, (DateTime)date_end.SelectedDate), connection);
            
                
                        exec_transfer_docs.CommandTimeout = 3600;  
                exec_transfer_docs.ExecuteNonQuery();
               // timer.Enabled = false;
                
                 timer.Stop();
                

                MessageBox.Show("Обработка продаж завершена, проверьте продажи", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }));
            
        }
        
    }
}
