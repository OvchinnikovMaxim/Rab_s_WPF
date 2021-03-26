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
        /// <returns></returns>
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
        /// <returns></returns>
        public string Display_zakaz(int number)
        {
            string query = "SELECT * FROM [nefco].[dbo].[zakaz_hat] WHERE id = " + number + ";";

            return query;
        }
    }
}
