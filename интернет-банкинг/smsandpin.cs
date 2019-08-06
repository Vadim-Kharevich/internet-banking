using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace интернет_банкинг
{
    public partial class smsandpin : MetroFramework.Forms.MetroForm
    {
        public smsandpin(string operation,int id_card,string id_client)
        {
            InitializeComponent();
            this.operation = operation;
            this.id_card = id_card;
            this.id_client = id_client;
        }
        string operation;
        int id_card;
        string id_client;
        string PIN;
        string number_phone;

        private void button1_Click(object sender, EventArgs e)
        {
            if (operation == "sms")
            {
                number_phone = textBox1.Text;
                if (Regex.IsMatch(number_phone, @"\+375[0-9]{9}"))
                {
                    DialogResult dialog = MessageBox.Show("Изменить номер телефона SMS-оповещения?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            string sql = "UPDATE bank_card SET number_phone=" + number_phone + " WHERE id_client=" + id_client + " AND id_card=" + id_card;
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                            Close();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    MessageBox.Show("Введите номер телефона в формате +375XXXXXXXXX!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            else
            {
                PIN = textBox1.Text;
                if (Regex.IsMatch(PIN, @"[0-9]{4}"))
                {
                    DialogResult dialog = MessageBox.Show("Изменить PIN-код?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        try
                        {
                            string sql = "UPDATE bank_card SET PIN=" + PIN + " WHERE id_client=" + id_client + " AND id_card=" + id_card;
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                            Close();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    MessageBox.Show("PIN-код должен состоять из 4-х цифр!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && e.KeyChar!='+')
            {
                e.Handled = true;
            }
        }

        private void smsandpin_Load(object sender, EventArgs e)
        {
            ActiveControl = button1;
            try
            {
                string sql = "SELECT number_phone,PIN FROM bank_card WHERE id_client="+id_client+" AND id_card="+id_card;
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                number_phone = reader1[0].ToString();
                PIN = reader1[1].ToString();
                reader1.Close();
                conn1.Close();

                if (operation == "sms")
                {
                    if (number_phone == "")
                    {
                        label2.Text = "Введите номер";
                        button1.Text = "Сохранить";
                    }
                    else
                    {
                        label4.Text = number_phone;
                        label3.Visible = true;
                        label4.Visible = true;
                    }
                }
                else
                {
                    label4.Text = PIN;
                    label3.Text = "Старый PIN";
                    label2.Text = "Новый PIN";
                    label1.Text = "Изменение PIN-кода";
                    textBox1.Text = "XXXX";
                    label3.Visible = true;
                    label4.Visible = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (operation == "sms")
            {
                textBox1.Text = "+375";
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else
            {
                textBox1.Text = "";
            }
        }
    }
}
