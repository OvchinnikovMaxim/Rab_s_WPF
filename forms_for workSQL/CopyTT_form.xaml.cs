using SQLquerys;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rab_stol.forms_for_workSQL
{
    /// <summary>
    /// Логика взаимодействия для CopyTT_form.xaml
    /// </summary>
    public partial class CopyTT_form : Window
    {
        SqlConnection connection;

        Query q = new Query();

        Serv_conn sc = new Serv_conn();

        public CopyTT_form()
        {
            InitializeComponent();
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

        #region только числа
        private void distrID_out_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void distrID_in_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void sectorID_out_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void sectorID_in_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void sector_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }

        private void contractor_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            sc.TextChisla(e);
        }
        #endregion

        private void btn_copyTT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sectorFrom = Convert.ToInt32(sectorID_out.Text);
                int sectorTo = Convert.ToInt32(sectorID_in.Text);
                int contractorFrom = Convert.ToInt32(distrID_out.Text);
                int contractorTo = Convert.ToInt32(distrID_in.Text);

                SqlCommand copyTT = new SqlCommand(q.copyTT_distr_sector(sectorFrom, sectorTo, contractorFrom, contractorTo), connection);
                copyTT.ExecuteNonQuery();

                MessageBox.Show("Копирование завершено", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btn_copyTT_new_distr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string t1, t2, t3, query;

                t1 = @"/* Входные параметры */
DECLARE @contractor_id  int             = " + Convert.ToInt32(contractor_id.Text) + @"; --Новый дистр
DECLARE @sector_id      int             = " + Convert.ToInt32(sector_id.Text) + @"; --Новый сектор
DECLARE @clients varchar(max)           = '" + clients.Text + @"'

DECLARE @distr_id varchar(max) = (select distr_id from nefco.dbo.co_contractor_attr_customer where contractor_id = @contractor_id);
            DECLARE @ALL_IS_OK int = 0;

            If(OBJECT_ID('tempdb..#clients') IS NOT NULL) Drop Table #clients;

BEGIN TRANSACTION Trans_clients_2_contractor
BEGIN TRY
    /* Запоминаем клиентов которых надо копернуть */
    Select *, row_number() over(order by client_id) as rnm

    INTO #clients
	from nefco.dbo.client_card c

    JOIN(select items from analitic.dbo.Split(@clients, ',')) cls ON cls.items = c.client_id

    DECLARE
        @rows       int = (select count(*) from #clients),
		@max_d1     int = (select max(id1) from nefco.dbo.client_card where contractor_id = @contractor_id),
		@start_row  int = 1;

            WHILE @start_row <= @rows

    BEGIN
        INSERT INTO nefco.dbo.client_card
            ([INN], [client_name], [client_adress], [urname], [uradress], [parent_id]
            , [type_id], [category_id], [dostup_id], [oplata_id], [oplata_prolong], [distr_id], [id1]
            , [city_id], [raion_id], [emp_id], [date_create], [visible], [fupd], [tnet_id], [area_id]
            , [resp_id], [sector_id], [factory_id], [postindex], [cspace], [visittime], [ordertype]
            , [comment], [visitweek], [visitday], [new_card], [visible_2122010], [route_pos], [face_sms]
            , [face_gms], [face_tm], [face_butter], [net_type_id], [similar_id], [city_kladr_id], [child_sales]
            , [work_time_from], [work_time_to], [visit_time_from], [visit_time_to], [rest_time_from]
            , [rest_time_to], [work_allday], [contact_id], [street_kladr_id], [house_num], [opt_contractor]
            , [square_area], [group_id], [contractor_id], [shelf])

        select
            [INN], [client_name], [client_adress], [urname], [uradress], [parent_id]
			, [type_id], [category_id], [dostup_id], [oplata_id], [oplata_prolong], @distr_id, @max_d1 + rnm
			, [city_id], [raion_id], [emp_id], [date_create], [visible], [fupd], [tnet_id], [area_id]
			, [resp_id], @sector_id, [factory_id], [postindex], [cspace], [visittime], [ordertype]
			, [comment], [visitweek], [visitday], [new_card], [visible_2122010], [route_pos], [face_sms]
			, [face_gms], [face_tm], [face_butter], [net_type_id], [similar_id], [city_kladr_id], [child_sales]
			, [work_time_from], [work_time_to], [visit_time_from], [visit_time_to], [rest_time_from]
			, [rest_time_to], [work_allday], [contact_id], [street_kladr_id], [house_num], [opt_contractor]
			, [square_area], [group_id], @contractor_id, [shelf]
            from #clients where rnm = @start_row;

		set @start_row = @start_row + 1;
            END" + '\n';

                t2 = @"/* Скидываем видимость клиентов которых копирнули */
	    UPDATE c set visible = 1 
		from nefco.dbo.client_card c 	
		JOIN (select items from analitic.dbo.Split(@clients,',')) cls ON cls.items = c.client_id" + '\n';

                t3 = @"SET @ALL_IS_OK = 1;
END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION Trans_clients_2_contractor
    print 'Error!'
END CATCH

IF @ALL_IS_OK = 1 COMMIT TRANSACTION Trans_clients_2_contractor";

                query = visible_copies_tt.IsChecked == true ? t1 + t2 + t3 : t1 + t3;

            
                SqlCommand copyTT_new_distr = new SqlCommand(query, connection);
                copyTT_new_distr.ExecuteNonQuery();

                MessageBox.Show("Копирование завершено", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void clients_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || e.Text == ","))
                e.Handled = true;
        }

        
    }
}
