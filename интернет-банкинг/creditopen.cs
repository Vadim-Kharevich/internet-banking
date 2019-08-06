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
    public partial class creditopen : MetroFramework.Forms.MetroForm
    {
        public creditopen(int[] mas_id_card, int[] mas_id_credit, int col_cards, int col_credit, string id_client, string RUB, string EUR, string USD)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.mas_id_credit = mas_id_credit;
            this.col_cards = col_cards;
            this.col_credit = col_credit;
            this.id_client = id_client;
            this.RUB = RUB;
            this.EUR = EUR;
            this.USD = USD;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int[] mas_id_credit = new int[10]; //id депозитов
        int col_cards; //кол-во карт
        int col_credit;
        int id_selected;//выбранная карта
        bool vyborcredit = false;
        bool vyborcard = false;
        int id_credit;//выбранный депозит
        string srok_credit;//срок депозита
        string id_client;//id клиента
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курс доллара
        string valytacredit;
        string valytacard;
        string min_vznos;
        string max_vznos;
        string procent_credit;

        private void creditopen_Load(object sender, EventArgs e)
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

                string sql = "SELECT name_credit FROM credit ";
                for (int i = 1; i <= col_credit; i++)
                {
                    if (i == 1) sql += "WHERE NOT id_credit = " + mas_id_credit[i];
                    if (i != 1) sql += " AND NOT id_credit=" + mas_id_credit[i];
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

        private void label5_Click(object sender, EventArgs e)
        {
            credit form = new credit(mas_id_card, mas_id_credit, col_cards, col_credit, id_client, RUB, EUR, USD);
            form.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                vyborcredit = true;
                string sql = "SELECT valyta,srok_credit,procent_credit,id_credit,min_summa,max_summa FROM credit,valyta WHERE valyta.id_valyta=(SELECT id_valyta FROM credit WHERE name_credit='" + comboBox1.SelectedItem.ToString() + "') AND name_credit='" + comboBox1.SelectedItem.ToString() + "';";
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                label6.Text = reader1[0].ToString();
                valytacredit = reader1[0].ToString();
                label7.Text = reader1[1].ToString() + " дней";
                srok_credit = reader1[1].ToString();
                label9.Text = reader1[2].ToString() + "%";
                procent_credit = reader1[2].ToString();
                id_credit = Convert.ToInt32(reader1[3]);
                min_vznos = reader1[4].ToString();
                max_vznos = reader1[5].ToString();
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vyborcard == false)
                {
                    MessageBox.Show("Выберите карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (vyborcredit == false)
                    {
                        MessageBox.Show("Выберите кредит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Введите сумму кредита!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string summazapyataya = textBox1.Text;
                            if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(min_vznos) || Convert.ToDouble(summazapyataya) > Convert.ToDouble(max_vznos))
                            {
                                MessageBox.Show("Сумма кредита должна быть в диапазоне от " + min_vznos + " до " + max_vznos + " " + valytacredit + " !", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                                sql = "SELECT id_credit_dopolnitelnaya FROM credit_dopolnitelnaya ORDER BY id_credit_dopolnitelnaya DESC";
                                conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                command1 = new MySqlCommand(sql, conn1);
                                reader1 = command1.ExecuteReader();
                                reader1.Read();
                                string id_last_credit = Convert.ToString(Convert.ToInt32(reader1[0]) + 1);
                                reader1.Close();
                                conn1.Close();

                                if (valytacard == valytacredit)
                                {
                                    ostatok = Convert.ToString(Convert.ToDouble(ostatok) + Convert.ToDouble(summazapyataya));
                                    ostatok = ostatok.Replace(',', '.');
                                    sql = "UPDATE bank_card SET ostatok='" + ostatok + "' WHERE id_card=" + mas_id_card[id_selected];
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(sql, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    string symma_month = (Convert.ToDouble(summazapyataya) / (Convert.ToDouble(srok_credit) / 30)).ToString().Replace(',','.');
                                    string symma_procent= (Convert.ToDouble(summazapyataya) * Convert.ToDouble(procent_credit)* 30/36500).ToString().Replace(',', '.');
                                    string vystavleno = (Convert.ToDouble(symma_month.Replace('.', ',')) + Convert.ToDouble(symma_procent.Replace('.', ','))).ToString().Replace(',', '.');
                                    string symma = summazapyataya.Replace(',', '.');
                                    string data_vidachi = DateTime.Now.ToString("yyyy-MM-dd");
                                    string data_okonchianiya = DateTime.Now.AddDays(Convert.ToDouble(srok_credit)).ToString("yyyy-MM-dd");
                                    string raschet = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                                    sql = "INSERT INTO credit_dopolnitelnaya(id_credit_dopolnitelnaya, id_client, id_credit, data_vidachi, data_raschet, data_okonchianiya, summa, summa_month, summa_procent,vystavleno) VALUES (" + id_last_credit + "," + id_client + "," + id_credit + ",'" + data_vidachi + "','" + raschet + "','" + data_okonchianiya + "','" + symma + "','" + symma_month + "','"+symma_procent+"','"+vystavleno+"')";
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(sql, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();
                                    this.Close();
                                    MessageBox.Show("Открыт новый кредит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                                else
                                {
                                    if (valytacredit == "BYN")
                                    {
                                        if (valytacard == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                        if (valytacard == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                        if (valytacard == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                    }
                                    else
                                    {
                                        if (valytacredit == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                        if (valytacredit == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                        if (valytacredit == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                        if (valytacard == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                        if (valytacard == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                        if (valytacard == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                    }
                                    DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialog == DialogResult.Yes)
                                    {
                                        ostatok = Convert.ToString(Convert.ToDouble(ostatok) + Convert.ToDouble(summazapyataya));
                                        ostatok = ostatok.Replace(',', '.');
                                        sql = "UPDATE bank_card SET ostatok='" + ostatok + "' WHERE id_card=" + mas_id_card[id_selected];
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(sql, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();


                                        string symma = textBox1.Text.Replace(',', '.');
                                        string symma_month = (Convert.ToDouble(textBox1.Text) / (Convert.ToDouble(srok_credit) / 30)).ToString().Replace(',','.');
                                        string symma_procent = (Convert.ToDouble(textBox1.Text) * Convert.ToDouble(procent_credit) * 30 / 36500).ToString().Replace(',', '.');
                                        string vystavleno = (Convert.ToDouble(symma_month.Replace('.', ',')) + Convert.ToDouble(symma_procent.Replace('.', ','))).ToString().Replace(',', '.');
                                        string data_vidachi = DateTime.Now.ToString("yyyy-MM-dd");
                                        string data_okonchianiya = DateTime.Now.AddDays(Convert.ToDouble(srok_credit)).ToString("yyyy-MM-dd");
                                        string raschet = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                                        sql = "INSERT INTO credit_dopolnitelnaya(id_credit_dopolnitelnaya, id_client, id_credit, data_vidachi, data_raschet, data_okonchianiya, summa, summa_month, summa_procent,vystavleno) VALUES (" + id_last_credit + "," + id_client + "," + id_credit + ",'" + data_vidachi + "','" + raschet + "','" + data_okonchianiya + "','" + symma + "','" + symma_month + "','" + symma_procent + "','" + vystavleno + "')";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(sql, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();
                                        this.Close();
                                        MessageBox.Show("Открыт новый кредит!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            vyborcard = true;
            id_selected = comboBox2.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (vyborcredit == true)
                {
                    string symma_procent = (Convert.ToDouble(textBox1.Text) * Convert.ToDouble(procent_credit) * 30 / 36500).ToString();
                    label11.Text = (Convert.ToDouble(symma_procent)+(Convert.ToDouble(textBox1.Text) / (Convert.ToDouble(srok_credit) / 30))).ToString();
                    label11.Text = (Convert.ToDouble(label11.Text) - Convert.ToDouble(label11.Text) % 0.01).ToString();
                }
            }
            catch { }
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
