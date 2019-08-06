using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace интернет_банкинг
{
    public partial class oplatakodyslygi : MetroFramework.Forms.MetroForm
    {
        public oplatakodyslygi(int[] mas_id_card, int col_cards, string id_client, string RUB, string EUR, string USD)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.col_cards = col_cards;
            this.id_client = id_client;
            this.RUB = RUB;
            this.EUR = EUR;
            this.USD = USD;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int col_cards; //кол-во карт
        string id_client;//id клиента
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курм доллара

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    string sql = "SELECT id_plateg,naznachenie FROM plategi,naznachenie WHERE kod_uslygi='"+textBox1.Text+ "' AND naznachenie.id_naznachenie=(SELECT id_naznachenie FROM plategi WHERE kod_uslygi='" + textBox1.Text + "')";
                    MySqlConnection conn1 = DBUtils.GetDBConnection();
                    conn1.Open();
                    MySqlCommand command1 = new MySqlCommand(sql, conn1);
                    MySqlDataReader reader1 = command1.ExecuteReader();
                    if (reader1.Read())
                    {
                        plategi form = new plategi(mas_id_card, col_cards, id_client, reader1[0].ToString(), reader1[1].ToString(), RUB, EUR, USD, textBox1.Text);
                        form.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Платеж с таким номером не найден!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    reader1.Close();
                    conn1.Close();

                    
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите код услуги!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
