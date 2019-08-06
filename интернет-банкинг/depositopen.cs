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
    public partial class depositopen : MetroFramework.Forms.MetroForm
    {
        public depositopen(int[] mas_id_card, int[] mas_id_deposit,int col_cards, int col_deposit, string id_client, string RUB, string EUR, string USD)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.mas_id_deposit = mas_id_deposit;
            this.col_cards = col_cards;
            this.col_deposit = col_deposit;
            this.id_client = id_client;
            this.RUB = RUB;
            this.EUR = EUR;
            this.USD = USD;
        }

        int[] mas_id_card = new int[10]; //id карт 
        int[] mas_id_deposit = new int[10]; //id депозитов
        int col_cards; //кол-во карт
        int col_deposit;
        int id_selected;//выбранная карта
        bool vybordeposit = false;
        bool vyborcard = false;
        int id_deposit;//выбранный депозит
        string srok_deposit;//срок депозита
        string snyatie;//ежемесячное снятие
        string id_client;//id клиента
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курс доллара
        string valytadeposit;
        string valytacard;
        string min_vznos;
        string max_vznos;
        int kod;

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            vyborcard=true;
            id_selected = comboBox2.SelectedIndex;
            panel1.Visible = true;
            panel1.Top = label10.Top + label10.Height + 15;
            button1.Top = panel1.Top + panel1.Height + 15;
            Height = button1.Top + button1.Height + 50;
            try
            {
                string str = "SELECT number_phone,PIN FROM bank_card WHERE id_card=" + mas_id_card[id_selected];
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(str, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader[0].ToString() == "")
                {
                    kod = Convert.ToInt32(reader[1]);
                    label11.Text = "PIN-код";
                    linkLabel1.Visible = false;

                }
                else
                {
                    label11.Text = "Код-подтверждения";
                    linkLabel1.Visible = true;
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void depositopen_Load(object sender, EventArgs e)
        {
            ActiveControl = button1;
            try
            {
                for (int i = 0; i < col_cards; i++)
                {
                    string str = "SELECT type_card,name,valyta,number_card,ostatok FROM bank_card,type,type_card,valyta WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")) AND type.id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND id_card =" + mas_id_card[i];
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(str, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string[] mas = new string[10];
                    mas = reader[3].ToString().Split(' ');
                    string ostatok = reader[4].ToString();
                    ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(ostatok) % 0.01);
                    comboBox2.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + mas[0] + "**** ****" + mas[3] + " остаток:" + ostatok + " " + reader[2].ToString());
                    reader.Close();
                    conn.Close();

                }

                string sql = "SELECT name_deposit FROM deposit ";
                for (int i = 1; i <= col_deposit; i++)
                {
                    if (i == 1) sql += "WHERE NOT id_deposit = " + mas_id_deposit[i];
                    if (i != 1) sql += " AND NOT id_deposit=" + mas_id_deposit[i];
                }
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    comboBox1.Items.Add(reader1[0].ToString());
                }
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                vybordeposit = true;
                string sql = "SELECT valyta,srok_deposit,procent_deposit,id_deposit,ejemesyachnoe_snyatie,min_vznos,max_vznos FROM deposit,valyta WHERE valyta.id_valyta=(SELECT id_valyta FROM deposit WHERE name_deposit='" + comboBox1.SelectedItem.ToString() + "') AND name_deposit='" + comboBox1.SelectedItem.ToString()+"';";
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                label6.Text = reader1[0].ToString();
                valytadeposit = reader1[0].ToString();
                label7.Text = reader1[1].ToString() + " дней";
                srok_deposit = reader1[1].ToString();
                label9.Text = reader1[2].ToString() + "%";
                id_deposit = Convert.ToInt32(reader1[3]);
                snyatie = reader1[4].ToString();
                min_vznos= reader1[5].ToString();
                max_vznos = reader1[6].ToString();
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vyborcard == false)
                {
                    MessageBox.Show("Выберите карту для пополнения!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (vybordeposit == false)
                    {
                        MessageBox.Show("Выберите депозит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Введите сумму депозита!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (kod.ToString() != textBox2.Text)
                            {
                                MessageBox.Show("Вы ввели неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                string summazapyataya = textBox1.Text;
                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(min_vznos) || Convert.ToDouble(summazapyataya) > Convert.ToDouble(max_vznos))
                                {
                                    MessageBox.Show("Сумма депозита должна быть в диапазоне от " + min_vznos + " до " + max_vznos + " " + valytadeposit + " !", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    string sql = "SELECT ostatok,valyta FROM bank_card,valyta WHERE id_card=" + mas_id_card[id_selected] + " AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[id_selected] + ")";
                                    MySqlConnection conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    MySqlCommand command1 = new MySqlCommand(sql, conn1);
                                    MySqlDataReader reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    string ostatok = reader1[0].ToString();
                                    valytacard = reader1[1].ToString();
                                    ostatok = ostatok.Replace('.', ',');
                                    reader1.Close();
                                    conn1.Close();

                                    if (valytacard == valytadeposit)
                                    {
                                        if (Convert.ToDouble(ostatok) >= Convert.ToDouble(summazapyataya))
                                        {
                                            ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(summazapyataya));
                                            ostatok = ostatok.Replace(',', '.');
                                            sql = "UPDATE bank_card SET ostatok='" + ostatok + "' WHERE id_card=" + mas_id_card[id_selected];
                                            conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            command1 = new MySqlCommand(sql, conn1);
                                            reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            reader1.Close();
                                            conn1.Close();

                                            sql = "SELECT id_deposit_dopolnitelnaya FROM deposit_dopolnitelnaya ORDER BY id_deposit_dopolnitelnaya DESC";
                                            conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            command1 = new MySqlCommand(sql, conn1);
                                            reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            string id_last_deposit = Convert.ToString(Convert.ToInt32(reader1[0]) + 1);
                                            reader1.Close();
                                            conn1.Close();

                                            string symma = summazapyataya.Replace(',', '.');
                                            string id_valytacard = "";
                                            if (valytacard == "BYN") id_valytacard = "1";
                                            if (valytacard == "RUB") id_valytacard = "2";
                                            if (valytacard == "EUR") id_valytacard = "3";
                                            if (valytacard == "USD") id_valytacard = "4";
                                            sql = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + symma + "','Открытие депозита " + comboBox1.SelectedItem.ToString() + "'," + id_valytacard + ")";
                                            conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            command1 = new MySqlCommand(sql, conn1);
                                            reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            reader1.Close();
                                            conn1.Close();

                                            string data_vidachi = DateTime.Now.ToString("yyyy-MM-dd");
                                            string data_okonchianiya = DateTime.Now.AddDays(Convert.ToDouble(srok_deposit)).ToString("yyyy-MM-dd");
                                            string snytie = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                                            if (snyatie == "YES")
                                            {
                                                sql = "INSERT INTO deposit_dopolnitelnaya(id_deposit_dopolnitelnaya, id_client, id_deposit, data_vidachi, data_okonchianiya, snytie, summa, data_raschet, nakopleniya) VALUES (" + id_last_deposit + "," + id_client + "," + id_deposit + ",'" + data_vidachi + "','" + data_okonchianiya + "','" + snytie + "','" + symma + "',NULL,'0')";
                                                conn1 = DBUtils.GetDBConnection();
                                                conn1.Open();
                                                command1 = new MySqlCommand(sql, conn1);
                                                reader1 = command1.ExecuteReader();
                                                reader1.Read();
                                                reader1.Close();
                                                conn1.Close();
                                                this.Close();
                                                MessageBox.Show("Открыт новый депозит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                            }
                                            else
                                            {
                                                sql = "INSERT INTO deposit_dopolnitelnaya(id_deposit_dopolnitelnaya, id_client, id_deposit, data_vidachi, data_okonchianiya, snytie, summa, data_raschet, nakopleniya) VALUES (" + id_last_deposit + "," + id_client + "," + id_deposit + ",'" + data_vidachi + "','" + data_okonchianiya + "',NULL,'" + symma + "',NULL,'0')";
                                                conn1 = DBUtils.GetDBConnection();
                                                conn1.Open();
                                                command1 = new MySqlCommand(sql, conn1);
                                                reader1 = command1.ExecuteReader();
                                                reader1.Read();
                                                reader1.Close();
                                                conn1.Close();
                                                this.Close();
                                                MessageBox.Show("Открыт новый депозит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else
                                    {
                                        if (valytadeposit == "BYN")
                                        {
                                            if (valytacard == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                            if (valytacard == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                            if (valytacard == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                        }
                                        else
                                        {
                                            if (valytadeposit == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                            if (valytadeposit == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                            if (valytadeposit == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                            if (valytacard == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                            if (valytacard == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                            if (valytacard == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                        }
                                        if (Convert.ToDouble(ostatok) >= Convert.ToDouble(summazapyataya))
                                        {
                                            DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                            if (dialog == DialogResult.Yes)
                                            {
                                                ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(summazapyataya));
                                                ostatok = ostatok.Replace(',', '.');
                                                sql = "UPDATE bank_card SET ostatok='" + ostatok + "' WHERE id_card=" + mas_id_card[id_selected];
                                                conn1 = DBUtils.GetDBConnection();
                                                conn1.Open();
                                                command1 = new MySqlCommand(sql, conn1);
                                                reader1 = command1.ExecuteReader();
                                                reader1.Read();
                                                reader1.Close();
                                                conn1.Close();

                                                sql = "SELECT id_deposit_dopolnitelnaya FROM deposit_dopolnitelnaya ORDER BY id_deposit_dopolnitelnaya DESC";
                                                conn1 = DBUtils.GetDBConnection();
                                                conn1.Open();
                                                command1 = new MySqlCommand(sql, conn1);
                                                reader1 = command1.ExecuteReader();
                                                reader1.Read();
                                                string id_last_deposit = Convert.ToString(Convert.ToInt32(reader1[0]) + 1);
                                                reader1.Close();
                                                conn1.Close();

                                                string symma = textBox1.Text.Replace(',', '.');
                                                string id_valytacard = "";
                                                if (valytacard == "BYN") id_valytacard = "1";
                                                if (valytacard == "RUB") id_valytacard = "2";
                                                if (valytacard == "EUR") id_valytacard = "3";
                                                if (valytacard == "USD") id_valytacard = "4";
                                                sql = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + symma + "','Открытие депозита " + comboBox1.SelectedItem.ToString() + "'," + id_valytacard + ")";
                                                conn1 = DBUtils.GetDBConnection();
                                                conn1.Open();
                                                command1 = new MySqlCommand(sql, conn1);
                                                reader1 = command1.ExecuteReader();
                                                reader1.Read();
                                                reader1.Close();
                                                conn1.Close();

                                                string data_vidachi = DateTime.Now.ToString("yyyy-MM-dd");
                                                string data_okonchianiya = DateTime.Now.AddDays(Convert.ToDouble(srok_deposit)).ToString("yyyy-MM-dd");
                                                string snytie = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                                                if (snyatie == "YES")
                                                {
                                                    sql = "INSERT INTO deposit_dopolnitelnaya(id_deposit_dopolnitelnaya, id_client, id_deposit, data_vidachi, data_okonchianiya, snytie, summa, data_raschet, nakopleniya) VALUES (" + id_last_deposit + "," + id_client + "," + id_deposit + ",'" + data_vidachi + "','" + data_okonchianiya + "','" + snytie + "','" + symma + "',NULL,'0')";
                                                    conn1 = DBUtils.GetDBConnection();
                                                    conn1.Open();
                                                    command1 = new MySqlCommand(sql, conn1);
                                                    reader1 = command1.ExecuteReader();
                                                    reader1.Read();
                                                    reader1.Close();
                                                    conn1.Close();
                                                    this.Close();
                                                    MessageBox.Show("Открыт новый депозит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                                }
                                                else
                                                {
                                                    sql = "INSERT INTO deposit_dopolnitelnaya(id_deposit_dopolnitelnaya, id_client, id_deposit, data_vidachi, data_okonchianiya, snytie, summa, data_raschet, nakopleniya) VALUES (" + id_last_deposit + "," + id_client + "," + id_deposit + ",'" + data_vidachi + "','" + data_okonchianiya + "',NULL,'" + symma + "',NULL,'0')";
                                                    conn1 = DBUtils.GetDBConnection();
                                                    conn1.Open();
                                                    command1 = new MySqlCommand(sql, conn1);
                                                    reader1 = command1.ExecuteReader();
                                                    reader1.Read();
                                                    reader1.Close();
                                                    conn1.Close();
                                                    this.Close();
                                                    MessageBox.Show("Открыт новый депозит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            deposit form = new deposit(mas_id_card, mas_id_deposit, col_cards, col_deposit, id_client, RUB, EUR, USD);
            form.Show();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && e.KeyChar!=',')
            {
                e.Handled = true;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Random random = new Random();
            kod = random.Next(1000, 99999);
            phone form = new phone();
            form.label3.Text += kod;
            form.Show();
        }
    }
}
