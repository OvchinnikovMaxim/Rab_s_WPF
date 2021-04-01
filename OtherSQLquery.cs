using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rab_stol
{
    class OtherSQLquery
    {
        /// <summary>
        /// Восстановление заказа в ZAKAZ_HAT с записью в историю
        /// </summary>
        /// <param name="number">Код заказа</param>
        /// <param name="comment">Комментарий для лога</param>
        /// <returns>строку запроса</returns>
        public string Recovery_zakaz(int number, string comment)
        {
            string query =
            @"DECLARE @order_id INT;
            SET @order_id = " + number + @";

            SET IDENTITY_INSERT[nefco].[dbo].[zakaz_hat] ON

            INSERT INTO[nefco].[dbo].[zakaz_hat]
            ([id]
            ,[num]
            ,[date_imp]
            ,[date_create]
            ,[date_transp]
            ,[date]
            ,[st]
            ,[ver_id]
            ,[zaivka_id]
            ,[transp_id]
            ,[saleman_id]
            ,[distr_name]
            ,[adress_d]
            ,[skid]
            ,[pay_id]
            ,[sign]
            ,[stand_zaivka]
            ,[is_garmach]
            ,[time_dos]
            ,[itog_otchet]
            ,[is_masspoddon]
            ,[is_opened1111]
            ,[is_notfinish]
            ,[import_number]
            ,[logistics_1_prc]
            ,[nulled_passed]
            ,[outage_passed]
            ,[action_report]
            ,[cc_signature]
            ,[description]
            ,[dscnt_3_mnplt]
            ,[dscnt_5_mnplt]
            ,[consignee_id]
            ,[dscnt_mnplt]
            ,[price_id]
            ,[date_otgr]
            ,[exp_kgk]
            ,[err_id]
            ,[dscnt_maket]
            ,[is_mnplt_1prc]
            ,[date_init]
            ,[poddon_id]
            ,[warehouse_id]
            ,[buyer_num]
            ,[buyer_date]
            ,[time_transp]
            ,[cost_ecod_state_id]
            ,[dscnt_disposal]
            ,[disposal_on]
            ,[bonus_free]
            ,[sum_from_1c]
            ,[address_id]
            ,[agreement_new]
            ,[is_logistics]
            ,[ecod_confirmation]
            ,[type]
            ,[delivery_type_ecod]
            ,[transp_types_ecod]
            ,[delivery_instruction_ecod]
            ,[location_name_ecod]
            ,[locat_str_number_ecod]
            ,[location_city_ecod]
            ,[final_buyer_ecod]
            ,[order_ext]
	        )
	        SELECT
            [id]
            ,[num]
            ,[date_imp]
            ,[date_create]
            ,[date_transp]
            ,[date]
            ,[st]
            ,[ver_id]
            ,[zaivka_id]
            ,[transp_id]
            ,[saleman_id]
            ,[distr_name]
            ,[adress_d]
            ,[skid]
            ,[pay_id]
            ,[sign]
            ,[stand_zaivka]
            ,[is_garmach]
            ,[time_dos]
            ,[itog_otchet]
            ,[is_masspoddon]
            ,[is_opened1111]
            ,[is_notfinish]
            ,[import_number]
            ,[logistics_1_prc]
            ,[nulled_passed]
            ,[outage_passed]
            ,[action_report]
            ,[cc_signature]
            ,[description]
            ,[dscnt_3_mnplt]
            ,[dscnt_5_mnplt]
            ,[consignee_id]
            ,[dscnt_mnplt]
            ,[price_id]
            ,[date_otgr]
            ,[exp_kgk]
            ,[err_id]
            ,[dscnt_maket]
            ,[is_mnplt_1prc]
            ,[date_init]
            ,[poddon_id]
            ,[warehouse_id]
            ,[buyer_num]
            ,[buyer_date]
            ,[time_transp]
            ,[cost_ecod_state_id]
            ,[dscnt_disposal]
            ,[disposal_on]
            ,[bonus_free]
            ,[sum_from_1c]
            ,[address_id]
            ,[agreement_new]
            ,[is_logistics]
            ,[ecod_confirmation]
            ,[type]
            ,[delivery_type_ecod]
            ,[transp_types_ecod]
            ,[delivery_instruction_ecod]
            ,[location_name_ecod]
            ,[locat_str_number_ecod]
            ,[location_city_ecod]
            ,[final_buyer_ecod]
            ,[order_ext]
            FROM[nefco].[dbo].[zakaz_hat_delete]
            WHERE id = @order_id;

            SET IDENTITY_INSERT[nefco].[dbo].[zakaz_hat] OFF

            INSERT INTO[nefco].[dbo].[order_control_logs] VALUES('admin', @order_id, '', 1, GETDATE(), '" + comment + "');";

            return query;
        }

        /// <summary>
        /// Отображение восстановленного заказа
        /// </summary>
        /// <param name="number">Код заказа</param>
        /// <returns>строку запроса</returns>
        public string Display_zakaz(int number)
        {
            string query = "SELECT * FROM [nefco].[dbo].[zakaz_hat] WHERE id = " + number + ";";

            return query;
        }

        /// <summary>
        /// Редактирование транспортной/ЖД
        /// </summary>
        /// <param name="TEKid">код транспортной/ЖД</param>
        /// <param name="name">Наименование</param>
        /// <param name="dogovor">номер договора</param>
        /// <param name="address">Юридический адрес</param>
        /// <param name="r_s">Расчетный счет</param>
        /// <param name="bank">Наименование банка</param>
        /// <param name="k_s">Корреспондетский счет</param>
        /// <param name="bik">БИК</param>
        /// <param name="inn">ИНН</param>
        /// <param name="kpp">КПП</param>
        /// <param name="date_n">Дата договора</param>
        /// <param name="userID">Код сотрудника изменивщего ТЭК</param>
        /// <returns>строку запроса</returns>
        public string Edit_tek(int TEKid,string name, string dogovor, string address, string r_s, string bank, string k_s, string bik, string inn, string kpp, DateTime date_n, int userID)
        {
            string query = @"
DECLARE @name varchar(500) = '" + name + "';" +
"DECLARE @contract_number varchar(50) = '" + dogovor + "'; " +
"DECLARE @address varchar(250) = '" + address + "'; " +
"DECLARE @settlement_account varchar(50) = '" + r_s + "'; " +
"DECLARE @bank_name varchar(256) = '" + bank + "';" +
"DECLARE @loro_account varchar(50) = '" + k_s + "'; " +
"DECLARE @bik varchar(50) = '" + bik + "'; " +
"DECLARE @inn varchar(50) = '" + inn + "'; " +
"DECLARE @kpp varchar(50) = '" + kpp + "'; " +
"DECLARE @contract_date datetime = '" + date_n.ToShortDateString() + "'; " +
"DECLARE @id INT = " + TEKid + ";" +
@"
-- Изменение контрагента
  UPDATE dbo.co_contractor
  SET 
    [name] = @name
  WHERE
    id = @id;
 -- Изменение атрибутов
 UPDATE
		dbo.co_contractor_attr_transp
	  SET
		contract_number = @contract_number, 
		address = @address,
		settlement_account = @settlement_account, 
		bank_name = @bank_name,
		loro_account = @loro_account, 
		bik = @bik,
		inn = @inn, 
		kpp = @kpp,
        contract_date = @contract_date
	  WHERE
		contractor_id = @id;
-- Логирование
  DECLARE @log_id INT;
  DECLARE @user_id INT = " + userID + ";" +
                          @"DECLARE @prgm_id INT = 6;              
						  DECLARE @tag_id INT = 121;
  EXEC dbo.co_log_insert @user_id, 6, @tag_id, @id, @log_id OUTPUT;

  DECLARE @log_detail_id INT;
  EXEC dbo.co_log_detail_insert @log_id, 'наименование', @name, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'Номер договора', @contract_number, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'Адрес', @address, @log_detail_id OUTPUT; 
  EXEC dbo.co_log_detail_insert @log_id, 'Расчетный счет', @settlement_account, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'имя банка', @bank_name, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'кор. счет', @loro_account, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'бик', @bik, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'инн', @inn, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'кпп', @kpp, @log_detail_id OUTPUT;
  EXEC dbo.co_log_detail_insert @log_id, 'дата договора', @contract_date, @log_detail_id OUTPUT;";

            return query;
        }

        /// <summary>
        /// Запись в лог, что зарегистрирована отправка рейса в 1С
        /// </summary>
        /// <param name="trip_id">код рейса</param>
        /// <returns>строку запроса</returns>
        public string Trip_log_in_1c(int trip_id)
        {
            string query = "INSERT INTO [service].[dbo].[tc_trip_log](log_app_id, tc_trip_id, date, user_id, action, status_id) " +
                "VALUES(4, " + trip_id + ", CURRENT_TIMESTAMP, 1, 'Зарегистрирована отправка в 1С', 6)";

            return query;
        }

        /// <summary>
        /// История рейса
        /// </summary>
        /// <param name="trip_id">код рейса</param>
        /// <returns>строку запроса</returns>
        public string Trip_history(int trip_id)
        {
            string query = @"
SELECT ttl.tc_trip_id as 'ID рейса'
, isnull(cc.name, ' ' ) 'ТЭК'
, ttl.date 'Дата действия'
, isnull(ttl.action, '') 'Действие'
, ttls.name 'Тип действия'
, isnull(cp.name, ' ') 'Имя пользователя'
, isnull(cp.surname, ' ') 'Фамилия пользователя'
FROM Service.dbo.tc_trip_log ttl
left join Service.dbo.tc_trip_log_status ttls on ttls.id = ttl.status_id
left join co_user cu on cu.id = ttl.user_id
left join co_person cp on cp.user_id = cu.id
left join co_contractor cc on cc.id = ttl.transp_company
WHERE tc_trip_id = " + trip_id +
"ORDER BY date DESC";

            return query;
        }
    }
}
