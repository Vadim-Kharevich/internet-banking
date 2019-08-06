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
    public partial class cardopen : MetroFramework.Forms.MetroForm
    {
        public cardopen(int[] mas_id_card, int col_cards, string id_client)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.col_cards = col_cards;
            this.id_client = id_client;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int col_cards; //кол-во карт
        bool vyborsystem = false;
        bool vyborvalyta = false;
        bool vyborsrok = false;
        string id_client;//id клиента
        string srok;
        string type_card;
        string valyta;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vyborsystem == false)
                {
                    MessageBox.Show("Выберите платежную систему!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (vyborvalyta == false)
                    {
                        MessageBox.Show("Выберите валюту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (vyborsrok == false)
                        {
                            MessageBox.Show("Выберите срок действия карты!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string[] mas = new string[5];
                            mas = type_card.Split(' ');
                            string type="";
                            string name = "";
                            for(int i = 1;i<mas.Length; i++)
                            {
                                if (i != mas.Length-1)
                                    name += mas[i] + " ";
                                else
                                    name += mas[i];
                            }
                            if (mas[0] == "Visa")
                                type = "1";
                            if (mas[0] == "MasterCard")
                                type = "2";
                            if (mas[0] == "Maestro")
                                type = "3";
                            string sql = "SELECT FIO,type.id_type FROM bank_card,type WHERE id_client='" + id_client + "' AND type.type='"+type+"' AND type.name='"+name+"'";
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            string FIO = reader1[0].ToString();
                            type = reader1[1].ToString();
                            reader1.Close();
                            conn1.Close();

                            sql = "SELECT id_card FROM bank_card ORDER BY id_card DESC";
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(sql, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            string id_card = (Convert.ToInt32(reader1[0])+1).ToString();
                            reader1.Close();
                            conn1.Close();

                            string srok_deystviya = DateTime.Now.AddYears(Convert.ToInt32(srok)).ToString("yyyy-MM-dd");
                            
                            string randomizer()
                            {
                                string number_card = "";
                                Random random = new Random();
                                int number1 = random.Next(1000,9999);
                                int number2 = random.Next(1000, 9999);
                                int number3 = random.Next(1000, 9999);
                                int number4 = random.Next(1000, 9999);
                                number_card = number1.ToString() + " " + number2.ToString() + " " + number3.ToString() + " " + number4.ToString();

                                sql = "SELECT number_card FROM bank_card WHERE number_card='"+number_card+"'";
                                conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                command1 = new MySqlCommand(sql, conn1);
                                reader1 = command1.ExecuteReader();
                                if (reader1.Read())
                                {
                                    reader1.Close();
                                    conn1.Close();
                                    randomizer();
                                }
                                else
                                {
                                    reader1.Close();
                                    conn1.Close();
                                }
                                return number_card;
                            }
                            Random random1 = new Random();
                            int PIN = random1.Next(1000, 9999);

                            sql = "INSERT INTO bank_card(id_card, id_client, id_type, id_valyta, FIO, number_card, ostatok, srok_deystviya, PIN) VALUES ('"+id_card+"','"+id_client+"','"+type+"','"+valyta+"','"+FIO+"','"+randomizer()+"','0','"+srok_deystviya+"','"+PIN+"')";
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(sql, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                            

                            this.Close();
                            MessageBox.Show("Получена новая карта "+type_card+"!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            vyborsystem = true;
            type_card = comboBox1.SelectedItem.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            vyborsrok = true;
            srok = comboBox3.SelectedItem.ToString();
            srok = srok[0].ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            vyborvalyta = true;
            valyta = comboBox2.SelectedIndex.ToString();
            valyta = (Convert.ToInt32(valyta)+1).ToString();
        }

        private void cardopen_Load(object sender, EventArgs e)
        {
            ActiveControl = button1;
            try
            {
                string sql = "SELECT type,name FROM type ";
                for (int i = 0; i < col_cards; i++)
                {
                    if (i == 0) sql += "WHERE NOT id_type = (SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i]+")";
                    if (i != 0) sql += " AND NOT id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")";
                }
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    if (reader1[0].ToString() == "1")
                        comboBox1.Items.Add("Visa " + reader1[1].ToString());
                    if (reader1[0].ToString() == "2")
                        comboBox1.Items.Add("MasterCard " + reader1[1].ToString());
                    if (reader1[0].ToString() == "3")
                        comboBox1.Items.Add("Maestro " + reader1[1].ToString());
                }
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Close();
            card form = new card(mas_id_card,col_cards,id_client);
            form.Show();
            form.TopMost = true;
        }
    }
}
