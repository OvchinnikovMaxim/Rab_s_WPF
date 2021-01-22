using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Media;
using SQLquerys;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rab_stol
{
    class Serv_conn
    {
        

        public SqlConnection Connection(TextBox server, Label status)
        {
            SqlConnectionStringBuilder conSTR = new SqlConnectionStringBuilder();
            conSTR.DataSource = server.Text;
            conSTR.IntegratedSecurity = true;

            SqlConnection conn = new SqlConnection(conSTR.ToString());
            try
            {
                conn.Open();
                status.Content = "Подключено";
                status.Foreground = Brushes.Green;
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return conn;
        }

        public void TextChanged(SqlConnection conn, TextBox server, Label status)
        {
            SqlConnectionStringBuilder conSTR = new SqlConnectionStringBuilder();
            conSTR.DataSource = server.Text;
            conSTR.IntegratedSecurity = true;

            conn = new SqlConnection(conSTR.ToString());

            try
            {
                
                    conn.Close();
                status.Content = "Отключено";
                status.Foreground = Brushes.Red;
                
                
                

            }
            catch (Exception)
            {

            }
        }

        public void TextServer(TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || e.Text == "."))
            {
                e.Handled = true;
            }
        }

        public void TextChisla(TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
