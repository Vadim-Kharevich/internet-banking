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
    public partial class selectcard : MetroFramework.Forms.MetroForm
    {
        public selectcard(int[] mas_id_card, int col_cards, int id_deposit,string id_client,string RUB,string EUR,string USD)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.col_cards = col_cards;
            this.id_deposit = id_deposit;
            this.id_client = id_client;
            this.RUB = RUB;
            this.EUR = EUR;
            this.USD = USD;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int col_cards; //кол-во карт
        int id_selected;//выбранная карта
        bool change = false;
        int id_deposit;//выбранный депозит
        string id_client;//id клиента
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курм доллара
        string valytamessage;//валюта для вывода сообщения
        int kod;

        void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if(label1.Text== "Выберите карту на которую будут возвращены средства" || label1.Text == "Выберите карту для начисления") //возврат средств при досрочном закрытии
            {
                if (change == false)
                {
                    MessageBox.Show("Выберите карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        string valytadeposit;
                        string valytacard;
                        string summadeposit;
                        string str = "SELECT deposit.id_valyta,bank_card.id_valyta,deposit_dopolnitelnaya.summa,deposit_dopolnitelnaya.nakopleniya,deposit_dopolnitelnaya.close FROM deposit_dopolnitelnaya,deposit,bank_card WHERE deposit.id_deposit=(SELECT deposit_dopolnitelnaya.id_deposit FROM deposit_dopolnitelnaya WHERE deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit + ") AND id_card=" + mas_id_card[id_selected] + " AND deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit;
                        MySqlConnection conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        MySqlCommand command1 = new MySqlCommand(str, conn1);
                        MySqlDataReader reader1 = command1.ExecuteReader();
                        reader1.Read();
                        valytadeposit = reader1[0].ToString();
                        valytacard = reader1[1].ToString();
                        if (reader1[4].ToString() == "YES")
                        {
                            summadeposit = (Convert.ToDouble(reader1[2].ToString().Replace('.',',')) + Convert.ToDouble(reader1[3].ToString().Replace('.', ','))).ToString();
                        }
                        else
                            summadeposit = reader1[2].ToString().Replace('.',',');
                        reader1.Close();
                        conn1.Close();

                        if (valytacard == valytadeposit) 
                        {
                            summadeposit = summadeposit.Replace(',', '.');
                            str = "UPDATE bank_card SET ostatok=ostatok+'" + summadeposit + "' WHERE id_card=" + mas_id_card[id_selected];
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(str, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                        }
                        else
                        {
                            DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog == DialogResult.Yes)
                            {
                                if (valytadeposit == "1")
                                {
                                    if (valytacard == "2") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(USD));
                                }
                                else
                                {
                                    if (valytadeposit == "2") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / 100 * Convert.ToDouble(RUB));
                                    if (valytadeposit == "3") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) * Convert.ToDouble(EUR));
                                    if (valytadeposit == "4") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) * Convert.ToDouble(USD));
                                    if (valytacard == "2") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summadeposit = Convert.ToString(Convert.ToDouble(summadeposit) / Convert.ToDouble(USD));
                                }
                            }

                            summadeposit = summadeposit.Replace(',', '.');
                            str = "UPDATE bank_card SET ostatok=ostatok+'" + summadeposit + "' WHERE id_card=" + mas_id_card[id_selected];
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(str, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                        }
                        str = "DELETE FROM deposit_dopolnitelnaya WHERE id_client=" + id_client + " AND id_deposit=" + id_deposit;
                        conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        command1 = new MySqlCommand(str, conn1);
                        reader1 = command1.ExecuteReader();
                        reader1.Read();
                        reader1.Close();
                        conn1.Close();
                        this.Close();
                        MessageBox.Show("Депозит успешно закрыт!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                    
            }
            if (label1.Text == "Выберите карту для пополнения депозита" || label1.Text == "Выберите карту для начисления средств") //выбор карты при пополнении депозита
            {
                if (change == false)
                {
                    MessageBox.Show("Выберите карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        flag = true;
                        textBox1.Visible = true;
                        comboBox1.Visible = false;
                        if (label1.Text == "Выберите карту для пополнения депозита") label1.Text = "Введите сумму пополнения";
                        if (label1.Text == "Выберите карту для начисления средств") label1.Text = "Введите сумму начисления";
                        if (label1.Text == "Выберите карту для пополнения депозита") button1.Text = "Пополнить";
                        if (label1.Text == "Выберите карту для начисления средств") button1.Text = "Начислить";

                        string str = "SELECT valyta FROM valyta WHERE id_valyta=(SELECT id_valyta FROM deposit WHERE id_deposit=" + id_deposit+")";
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(str, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        label2.Text = reader[0].ToString();
                        valytamessage = label2.Text;
                        reader.Close();
                        conn.Close();

                        if (label1.Text == "Введите сумму пополнения")
                        {
                            str = "SELECT number_phone,PIN FROM bank_card WHERE id_card=" + mas_id_card[id_selected];
                            conn = DBUtils.GetDBConnection();
                            conn.Open();
                            command = new MySqlCommand(str, conn);
                            reader = command.ExecuteReader();
                            reader.Read();
                            if (reader[0].ToString() == "")
                            {
                                kod = Convert.ToInt32(reader[1]);
                                label4.Text = "PIN-код";
                                linkLabel1.Visible = false;
                                panel1.Visible = true;
                            }
                            else
                            {
                                label4.Text = "Код подтверждения";
                                linkLabel1.Visible = true;
                                panel1.Visible = true;
                            }
                            reader.Close();
                            conn.Close();
                        }

                        label2.Visible = true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

           
            if (label1.Text == "Введите сумму начисления" && flag == false) //ежемесячное снятие с депозита 
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Введите сумму начисления!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        string valytadeposit;
                        string valytacard;
                        string ostatoktochka;
                        string str = "SELECT deposit.id_valyta,bank_card.id_valyta,bank_card.ostatok FROM deposit,bank_card WHERE deposit.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                        MySqlConnection conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        MySqlCommand command1 = new MySqlCommand(str, conn1);
                        MySqlDataReader reader1 = command1.ExecuteReader();
                        reader1.Read();
                        valytadeposit = reader1[0].ToString();
                        valytacard = reader1[1].ToString();
                        ostatoktochka = reader1[2].ToString();
                        reader1.Close();
                        conn1.Close();
                        string summazapyataya = textBox1.Text;
                        string[] mas = new string[10];
                        mas=label3.Text.Split(' ');
                        string summatocka = summazapyataya.Replace(',', '.');
                        string summadeposit = summatocka;
                        string ostatokzapyataya = ostatoktochka.Replace('.', ',');
                        if (Convert.ToDouble(summazapyataya) <= Convert.ToDouble(mas[4]))
                        {
                            if (valytacard == valytadeposit)
                            {
                                str = "UPDATE bank_card,deposit_dopolnitelnaya SET bank_card.ostatok=bank_card.ostatok+'" + summatocka + "',deposit_dopolnitelnaya.nakopleniya=deposit_dopolnitelnaya.nakopleniya-'" + summatocka + "' WHERE deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                                conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                command1 = new MySqlCommand(str, conn1);
                                reader1 = command1.ExecuteReader();
                                reader1.Read();
                                reader1.Close();
                                conn1.Close();
                                this.Close();
                                MessageBox.Show("Выполнено пополнение карты на сумму " + summatocka + " " + valytamessage, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                            {
                                if (valytadeposit == "1")
                                {
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }
                                else
                                {
                                    if (valytadeposit == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                    if (valytadeposit == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                    if (valytadeposit == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }
                                DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dialog == DialogResult.Yes)
                                {
                                    summatocka = summazapyataya.Replace(',', '.'); ;
                                    str = "UPDATE bank_card,deposit_dopolnitelnaya SET bank_card.ostatok=bank_card.ostatok+'" + summatocka + "',deposit_dopolnitelnaya.nakopleniya=deposit_dopolnitelnaya.nakopleniya-'" + summadeposit + "' WHERE deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();
                                    this.Close();
                                    MessageBox.Show("Выполнено пополнение карты на сумму " + summadeposit + " " + valytamessage, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Максимальная сумма вывода "+mas[4], "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (label1.Text == "Введите сумму пополнения" && flag == false) //пополнение депозита 
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Введите сумму пополнения!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (kod.ToString() != textBox2.Text)
                    {
                        MessageBox.Show("Вы ввели неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
                        {
                            string valytadeposit;
                            string valytacard;
                            string ostatoktochka;
                            string str = "SELECT deposit.id_valyta,bank_card.id_valyta,bank_card.ostatok,deposit.name_deposit FROM deposit,bank_card WHERE deposit.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(str, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            valytadeposit = reader1[0].ToString();
                            valytacard = reader1[1].ToString();
                            ostatoktochka = reader1[2].ToString();
                            string name_deposit = reader1[3].ToString();
                            reader1.Close();
                            conn1.Close();
                            string summazapyataya = textBox1.Text;
                            string summatocka = summazapyataya.Replace(',', '.');
                            string summadeposit = summatocka;

                            string ostatokzapyataya = ostatoktochka.Replace('.', ',');
                            if (valytacard == valytadeposit)
                            {
                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    str = "UPDATE bank_card,deposit_dopolnitelnaya SET bank_card.ostatok=bank_card.ostatok-'" + summatocka + "',deposit_dopolnitelnaya.summa=deposit_dopolnitelnaya.summa+'" + summatocka + "' WHERE deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Пополнение депозита " + name_deposit + "'," + valytacard + ")";
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    this.Close();
                                    MessageBox.Show("Выполнено пополнение депозита на сумму " + summatocka + " " + valytamessage, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    button2.Visible = true;
                                }
                            }
                            else
                            {
                                if (valytadeposit == "1")
                                {
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }
                                else
                                {
                                    if (valytadeposit == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                    if (valytadeposit == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                    if (valytadeposit == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }

                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialog == DialogResult.Yes)
                                    {
                                        summazapyataya = Convert.ToString(Math.Round(Convert.ToDouble(summazapyataya), 2));
                                        summatocka = summazapyataya.Replace(',', '.'); ;
                                        str = "UPDATE bank_card,deposit_dopolnitelnaya SET bank_card.ostatok=bank_card.ostatok-'" + summatocka + "',deposit_dopolnitelnaya.summa=deposit_dopolnitelnaya.summa+'" + summadeposit + "' WHERE deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + id_deposit + " AND id_card=" + mas_id_card[id_selected];
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Пополнение депозита " + name_deposit + "'," + valytacard + ")";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        this.Close();
                                        MessageBox.Show("Выполнено пополнение депозита на сумму " + summadeposit + " " + valytamessage, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    button2.Visible = true;
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                }
            }

            if (label1.Text == "Выберите карту для расчета") //закрытие кредита
            {
                if (change == false)
                {
                    MessageBox.Show("Выберите карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (kod.ToString() != textBox2.Text)
                    {
                        MessageBox.Show("Вы ввели неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
                        {
                            int id_credit = id_deposit;
                            string valytacredit;
                            string valytacard;
                            string ostatoktochka;
                            string str = "SELECT credit.id_valyta,bank_card.id_valyta,bank_card.ostatok,credit.name_credit FROM credit,bank_card WHERE credit.id_credit=" + id_credit + " AND id_card=" + mas_id_card[id_selected];
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(str, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            valytacredit = reader1[0].ToString();
                            valytacard = reader1[1].ToString();
                            ostatoktochka = reader1[2].ToString();
                            string name_credit = reader1[3].ToString();
                            reader1.Close();
                            conn1.Close();

                            string[] mas = new string[10];
                            mas = label3.Text.Split(' ');
                            string summazapyataya = mas[2];
                            string summatocka = summazapyataya.Replace(',', '.');
                            string ostatokzapyataya = ostatoktochka.Replace('.', ',');
                            if (valytacard == valytacredit)
                            {
                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    str = "DELETE FROM credit_dopolnitelnaya WHERE id_credit=" + id_credit + " AND id_client=" + id_client;
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Закрытие кредита " + name_credit + "'," + valytacard + ")";
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    this.Close();
                                    MessageBox.Show("Кредит успешно закрыт!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                if (valytacredit == "1")
                                {
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }
                                else
                                {
                                    if (valytacredit == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                    if (valytacredit == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                    if (valytacredit == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }

                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialog == DialogResult.Yes)
                                    {

                                        summatocka = summazapyataya.Replace(',', '.'); ;
                                        str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        str = "DELETE FROM credit_dopolnitelnaya WHERE id_credit=" + id_credit + " AND id_client=" + id_client;
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Закрытие кредита " + name_credit + "'," + valytacard + ")";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        this.Close();
                                        MessageBox.Show("Кредит успешно закрыт!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            if (label1.Text == "Выберите карту для оплаты") //оплата кредита
            {
                if (change == false)
                {
                    MessageBox.Show("Выберите карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (kod.ToString() != textBox2.Text)
                    {
                        MessageBox.Show("Вы ввели неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
                        {
                            int id_credit = id_deposit;
                            string valytacredit;
                            string valytacard;
                            string ostatoktochka;
                            string str = "SELECT credit.id_valyta,bank_card.id_valyta,bank_card.ostatok,credit.name_credit FROM credit,bank_card WHERE credit.id_credit=" + id_credit + " AND id_card=" + mas_id_card[id_selected];
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(str, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            valytacredit = reader1[0].ToString();
                            valytacard = reader1[1].ToString();
                            ostatoktochka = reader1[2].ToString();
                            string name_credit = reader1[3].ToString();
                            reader1.Close();
                            conn1.Close();

                            string[] mas = new string[10];
                            mas = label3.Text.Split(' ');
                            string summazapyataya = mas[2];
                            string summatocka = summazapyataya.Replace(',', '.');
                            string summazapyataya1 = summazapyataya;
                            string ostatokzapyataya = ostatoktochka.Replace('.', ',');
                            if (valytacard == valytacredit)
                            {
                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    string sql1 = "SELECT summa,summa_month,summa_procent,vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client + " AND id_credit=" + id_credit;
                                    MySqlConnection connection = DBUtils.GetDBConnection();
                                    connection.Open();
                                    MySqlCommand comm = new MySqlCommand(sql1, connection);
                                    MySqlDataReader read = comm.ExecuteReader();
                                    read.Read();
                                    double col_month = Convert.ToDouble(read[3].ToString().Replace('.', ',')) / (Convert.ToDouble(read[1].ToString().Replace('.', ',')) + Convert.ToDouble(read[2].ToString().Replace('.', ',')));
                                    string symma = (Convert.ToDouble(read[0].ToString().Replace('.', ',')) - (Convert.ToDouble(read[1].ToString().Replace('.', ',')) * Convert.ToInt32(col_month))).ToString().Replace(',', '.');

                                    str = "UPDATE credit_dopolnitelnaya SET summa='" + symma + "',vystavleno='0' WHERE id_credit=" + id_credit + " AND id_client=" + id_client;
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Оплата кредита " + name_credit + "'," + valytacard + ")";
                                    conn1 = DBUtils.GetDBConnection();
                                    conn1.Open();
                                    command1 = new MySqlCommand(str, conn1);
                                    reader1 = command1.ExecuteReader();
                                    reader1.Read();
                                    reader1.Close();
                                    conn1.Close();

                                    read.Close();
                                    connection.Close();
                                    this.Close();
                                    MessageBox.Show("Выполнена оплата кредита на сумму " + summazapyataya + " " + mas[3] + "!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                if (valytacredit == "1")
                                {
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }
                                else
                                {
                                    if (valytacredit == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                    if (valytacredit == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                    if (valytacredit == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                    if (valytacard == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                }

                                if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                {
                                    DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialog == DialogResult.Yes)
                                    {

                                        summatocka = summazapyataya.Replace(',', '.'); ;
                                        str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        string sql1 = "SELECT summa,summa_month,summa_procent,vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client + " AND id_credit=" + id_credit;
                                        MySqlConnection connection = DBUtils.GetDBConnection();
                                        connection.Open();
                                        MySqlCommand comm = new MySqlCommand(sql1, connection);
                                        MySqlDataReader read = comm.ExecuteReader();
                                        read.Read();
                                        double col_month = Convert.ToDouble(read[3].ToString().Replace('.', ',')) / (Convert.ToDouble(read[1].ToString().Replace('.', ',')) + Convert.ToDouble(read[2].ToString().Replace('.', ',')));
                                        string symma = (Convert.ToDouble(read[0].ToString().Replace('.', ',')) - (Convert.ToDouble(read[1].ToString().Replace('.', ',')) * Convert.ToInt32(col_month))).ToString().Replace(',', '.');

                                        str = "UPDATE credit_dopolnitelnaya SET summa='" + symma + "',vystavleno='0' WHERE id_credit=" + id_credit + " AND id_client=" + id_client;
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        str = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Оплата кредита " + name_credit + "'," + valytacard + ")";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        read.Close();
                                        connection.Close();
                                        this.Close();
                                        MessageBox.Show("Выполнена оплата кредита на сумму " + summazapyataya1 + " " + mas[3] + "!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

        private void selectcard_Load(object sender, EventArgs e)
        {
            ActiveControl = button1;
            for (int i = 0; i < col_cards; i++)
            {
                try
                {
                    string str = "SELECT type_card,name,valyta,number_card,ostatok FROM bank_card,type,type_card,valyta WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")) AND type.id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND id_card =" + mas_id_card[i];
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(str, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string ostatok = reader[4].ToString();
                    ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(ostatok) % 0.01);
                    string[] mas = new string[10];
                    mas = reader[3].ToString().Split(' ');
                    comboBox1.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + mas[0] + "**** ****" + mas[3] + " остаток:" + ostatok + " " + reader[2].ToString());
                    reader.Close();
                    conn.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }

        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs ex)
        {
            change = true;
            var a = (ComboBox)sender;
            if (a != null)
            {
                id_selected = a.SelectedIndex;
            }
            if(label1.Text == "Выберите карту для расчета"|| label1.Text == "Выберите карту для оплаты")
            {
                panel1.Visible = true;
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
                        label4.Text = "PIN-код";
                        linkLabel1.Visible = false;
                    }
                    else
                    {
                        label4.Text = "Код-подтверждения";
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
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) // ввод только цифр,точки и клавиши BackSpace
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && e.KeyChar != ',') 
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) //клавиша назад
        {
            label1.Text = "Выберите карту для пополнения депозита";
            comboBox1.Visible = true;
            textBox1.Visible = false;
            label2.Visible = false;
            button2.Visible = false;
            button1.Text = "Далее";
            panel1.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";
            button1.Left = button2.Left + button2.Width - 50;
        }
        bool panel = true;
        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (panel == true)
            {
                button1.Top = panel1.Top + panel1.Height + 15;
                button2.Top = panel1.Top + panel1.Height + 15;
                Height = button1.Top + button1.Height + 80;
                panel = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Random random = new Random();
            kod = random.Next(1000, 99999);
            phone form = new phone();
            form.label3.Text += kod;
            form.Show();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button2_VisibleChanged(object sender, EventArgs e)
        {
            button1.Left = button2.Left + button2.Width + 50;
        }
    }
}
