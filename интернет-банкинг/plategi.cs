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
    public partial class plategi : MetroFramework.Forms.MetroForm
    {
        public plategi(int[] mas_id_card, int col_cards, string id_client, string id_plateg,string naznachenie, string RUB, string EUR, string USD,string nomer_uslygi)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.col_cards = col_cards;
            this.id_client = id_client;
            this.id_plateg = id_plateg;
            this.naznachenie = naznachenie;
            this.RUB = RUB;
            this.EUR = EUR;
            this.USD = USD;
            this.nomer_uslygi = nomer_uslygi;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int col_cards; //кол-во карт
        string id_client;//id клиента
        string id_plateg;//id платежа
        string naznachenie; //назначение
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курм доллара
        string FIO;
        string adres;
        int id_selected;
        string valytacard;
        string ostatok;
        string id_plateg_dopolnitelnaya;
        int kod;
        string number_phone;
        string number_card;
        string scet;
        string str_scet;
        string nomer_uslygi;
        string doc_content = "";
        string doc_name;

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (button1.Text == "Оплатить")
            {
                if (textBox3.Text != ""&& textBox3.Text != "0")
                {
                    if (textBox2.Text != "")
                    {
                        if (textBox2.Text == kod.ToString())
                        {
                            try
                            {
                                string summazapyataya = textBox3.Text;
                                string summatocka = summazapyataya.Replace(',', '.');
                                string summazapyataya1 = summazapyataya;
                                string ostatokzapyataya = ostatok.Replace('.', ',');
                                bool flag = false;
                                if (valytacard == "BYN")
                                {
                                    if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                    {
                                        string str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                        MySqlConnection conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        MySqlCommand command1 = new MySqlCommand(str, conn1);
                                        MySqlDataReader reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        string sql1 = "SELECT vystavleno FROM plategi_dopolnitelnaya WHERE id_plateg_dopolnitelnaya=" + id_plateg_dopolnitelnaya;
                                        MySqlConnection connection = DBUtils.GetDBConnection();
                                        connection.Open();
                                        MySqlCommand comm = new MySqlCommand(sql1, connection);
                                        MySqlDataReader read = comm.ExecuteReader();
                                        read.Read();
                                        string vystavleno = (Convert.ToDouble(read[0].ToString().Replace('.', ',')) - Convert.ToDouble(summazapyataya)).ToString().Replace(',', '.');
                                        if (Convert.ToDouble(vystavleno.Replace('.', ',')) < 0) vystavleno = "0";
                                        read.Close();
                                        connection.Close();

                                        str = "UPDATE plategi_dopolnitelnaya SET vystavleno='" + vystavleno + "' WHERE id_plateg_dopolnitelnaya=" + id_plateg_dopolnitelnaya;
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        str = "INSERT INTO history(id_client, id_plateg, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + id_plateg + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Оплата "+naznachenie+" "+scet+"',1)";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(str, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();
                                        flag = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    if (valytacard == "RUB") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                    if (valytacard == "EUR") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                    if (valytacard == "USD") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));

                                    if (Convert.ToDouble(summazapyataya) < Convert.ToDouble(ostatokzapyataya))
                                    {
                                        DialogResult dialog = MessageBox.Show("Для продолжения необходимо произвести обмен валют.\r\nОбмен будет произведен по следующему курсу:\r\n100 Российских рублей =" + RUB + " Белорусских рублей\r\nДоллар США =" + USD + " Белорусских рублей\r\nЕвро =" + EUR + " Белорусских рублей", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (dialog == DialogResult.Yes)
                                        {
                                            summatocka = summazapyataya.Replace(',', '.');
                                            string str = "UPDATE bank_card SET ostatok=ostatok-'" + summatocka + "' WHERE id_card=" + mas_id_card[id_selected];
                                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            MySqlCommand command1 = new MySqlCommand(str, conn1);
                                            MySqlDataReader reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            reader1.Close();
                                            conn1.Close();

                                            string sql1 = "SELECT vystavleno FROM plategi_dopolnitelnaya WHERE id_plateg_dopolnitelnaya=" + id_plateg_dopolnitelnaya;
                                            MySqlConnection connection = DBUtils.GetDBConnection();
                                            connection.Open();
                                            MySqlCommand comm = new MySqlCommand(sql1, connection);
                                            MySqlDataReader read = comm.ExecuteReader();
                                            read.Read();
                                            string vystavleno = (Convert.ToDouble(read[0].ToString().Replace('.', ',')) - Convert.ToDouble(summazapyataya1)).ToString().Replace(',', '.');
                                            if (Convert.ToDouble(vystavleno.Replace('.', ',')) < 0) vystavleno = "0";
                                            read.Close();
                                            connection.Close();

                                            str = "UPDATE plategi_dopolnitelnaya SET vystavleno='" + vystavleno + "' WHERE id_plateg_dopolnitelnaya=" + id_plateg_dopolnitelnaya;
                                            conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            command1 = new MySqlCommand(str, conn1);
                                            reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            reader1.Close();
                                            conn1.Close();

                                            summatocka = summazapyataya1.Replace(',', '.');
                                            str = "INSERT INTO history(id_client, id_plateg, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + id_plateg + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + summatocka + "','Оплата " + naznachenie + " " + scet + "',1)";
                                            conn1 = DBUtils.GetDBConnection();
                                            conn1.Open();
                                            command1 = new MySqlCommand(str, conn1);
                                            reader1 = command1.ExecuteReader();
                                            reader1.Read();
                                            reader1.Close();
                                            conn1.Close();
                                            flag = true;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                if (flag == true)
                                {
                                    Random random = new Random();
                                    doc_content += "Карт-чек №" + random.Next(100000000, 999999999) + "\r\n";
                                    doc_content += DateTime.Now + "\r\nБанковская карточка: " + number_card + "\r\nНомер плательщика: " + random.Next(100000000, 999999999);
                                    doc_content += "\r\nПолучатель платежа: \"Унитарное \r\nпредприятие \"Компания\"\"\r\n\r\n";
                                    doc_content += naznachenie + "\r\n" + str_scet + ": " + scet + "\r\n";
                                    if (FIO != "")
                                    {
                                        doc_content += "ФИО: " + FIO + "\r\n";
                                    }
                                    if (adres != "")
                                    {
                                        doc_content += "Адрес: " + adres + "\r\n";
                                    }
                                    if (id_plateg == "280" || id_plateg == "281" || id_plateg == "282")
                                    {
                                        doc_content += "Фамилия: " + textBox4.Text + "\r\n";
                                        doc_content += "Имя: " + textBox5.Text + "\r\n";
                                        doc_content += "Отчество: " + textBox6.Text + "\r\n";
                                    }
                                    doc_content += "Сумма: " + summazapyataya1 +" BYN\r\n\r\nНомер услуги: " + nomer_uslygi + "\r\n№ операции: " + random.Next(100000000, 999999999)+"\r\n";

                                    string sqlhistory = "SELECT id_history FROM history ORDER BY id_history DESC";
                                    MySqlConnection connhistory = DBUtils.GetDBConnection();
                                    connhistory.Open();
                                    MySqlCommand commandhistory = new MySqlCommand(sqlhistory, connhistory);
                                    MySqlDataReader readerhistory = commandhistory.ExecuteReader();
                                    readerhistory.Read();
                                    doc_name = readerhistory[0].ToString();
                                    readerhistory.Close();
                                    connhistory.Close();

                                    Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();
                                    Microsoft.Office.Interop.Word.Document doc = app.Documents.Add();
                                    doc.Paragraphs[1].Range.Text = doc_content;
                                    Clipboard.SetImage(pictureBox1.Image);
                                    doc.Paragraphs[doc.Paragraphs.Count].Range.Paste();
                                    app.Visible = false;
                                    doc.SaveAs2(@"D:\Колледж\4 КУРС\ПРАКТИКА\интернет-банкинг\история\" + doc_name + ".docx");
                                    doc.Close();
                                    app.Quit();

                                    panel2.Visible = false;
                                    panel3.Visible = true;
                                    label6.Text = doc_content;
                                    pictureBox1.Top = label6.Top + label6.Height + 5;
                                    groupBox1.Height = pictureBox1.Height + label6.Height + 40;
                                    groupBox1.Width = label6.Width + 15;
                                    Height = groupBox1.Height+200;
                                    panel3.Height = Height - 70;
                                    Width = Width - 100;
                                }
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите код подтверждения!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Введите сумму платежа!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (button1.Text == "Далее")
            {
                str_scet = label1.Text;
                string sql = "SELECT FIO,adres,vystavleno,id_plateg_dopolnitelnaya FROM plategi_dopolnitelnaya WHERE id_plateg=" + id_plateg + " AND litsevoy_scet=" + textBox1.Text;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Visible = false;
                    scet = textBox1.Text;
                    FIO = reader[0].ToString();
                    if (FIO != "")
                    {
                        label2.Text ="ФИО: "+ FIO;
                        label2.Visible = true;
                    }
                    else
                    {
                        label2.Text = "";
                        label3.Top = label2.Top;
                        label1.Top = label3.Top + label3.Height + 15;
                        textBox3.Top= label3.Top + label3.Height + 15;
                        panel1.Top = label1.Top + label1.Height + 30;
                        button1.Top = panel1.Top + panel1.Height + 30;
                    }
                    adres = reader[1].ToString();
                    if (adres != "")
                    {
                        label3.Text += adres;
                        label3.Visible = true;
                    }
                    else
                    {
                        label3.Text = "";
                        label1.Top = label3.Top;
                        textBox3.Top = label3.Top;
                        panel1.Top = label1.Top + label1.Height + 30;
                        button1.Top = panel1.Top + panel1.Height + 30;
                    }
                    textBox3.Text = reader[2].ToString();
                    label1.Text = "Сумма";
                    button1.Text = "Оплатить";
                    panel1.Visible = true;
                    if (number_phone == "")
                    {
                        label4.Text = "PIN-код";
                        linkLabel1.Visible = false;
                    }
                    id_plateg_dopolnitelnaya = reader[3].ToString();
                    textBox1.Visible = false;
                    textBox3.Visible = true;

                    if (id_plateg == "280" || id_plateg == "281" || id_plateg == "282")
                    {
                        panel4.Visible = true;
                        panel4.Top = label1.Top;
                        label1.Top = panel4.Top + panel4.Height + 15;
                        textBox3.Top = panel4.Top + panel4.Height + 15;
                        panel1.Top = label1.Top + label1.Height + 70;
                        button1.Top = panel1.Top + panel1.Height ;
                    }
                    Height = button1.Top + button1.Height + 150;
                }
                else
                {
                    MessageBox.Show("Вы ввели несуществующий номер!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                reader.Close();
                conn.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Random random = new Random();
            kod=random.Next(1000,99999);
            phone form = new phone();
            form.label3.Text += kod;
            form.Show();
        }

        private void plategi_Load(object sender, EventArgs e)
        {
            ActiveControl = textBox1;
            Text = "Оплата "+naznachenie;
            if (naznachenie == "Дополнительное занятие" || naznachenie == "Сдача экзамена" || naznachenie == "Топливо" || naznachenie == "Автомобиль для экзамена" || naznachenie == "Обучение" || naznachenie == "Общежитие" || naznachenie == "Byfly,ZALA,Максифон,Умный дом" || naznachenie == "Единый Таможенный тариф ЕАЭС")
            {
                label1.Text = "Номер договора";
            }
            if (naznachenie == "Питание" || naznachenie == "Учебники" || naznachenie == "Кружки")
            {
                label1.Text = "Номер учащегося";
            }
            if (naznachenie == "Телефон" || naznachenie == "По № телефона")
            {
                label1.Text = "Номер телефона";
                label2.Text = "Введите 9 цифр номера телефона\r\nв формате: 29ххххххх, 33ххххххх,\r\n25ххххххх, 44ххххххх";
                label2.Visible = true;
            }
            if (naznachenie == "velcom - voka" || naznachenie == "velcom - Домашний интернет и ТВ" || naznachenie == "Интернет" || naznachenie == "Абонентская плата" || naznachenie == "Водоснабжение" || naznachenie == "Газоснабжение" || naznachenie == "Коммунальные платежи" || naznachenie == "Электроэнергия")
            {
                label1.Text = "Лицевой счет";
            }
            if (naznachenie == "Компьютерные услуги" || naznachenie == "Практический экзамен на авто" || naznachenie == "Практический экзамен на мото" || naznachenie == "Теоретический экзамен" || naznachenie == "Замена номера автомобиля" || naznachenie == "Выдача транзита" || naznachenie == "Замена номера мото/прицепа" || naznachenie == "Изготовление номеров" || naznachenie == "Регистрация автомобиля" || naznachenie == "Регистрация мотоцикла/прицепа")
            {
                label1.Text = "Идентификационный \r\nномер";
            }
            if (naznachenie == "Госпошлина" || naznachenie == "Разрешение")
            {
                label1.Text = "Серия и номер\r\nтехпаспорта ТС";
                label2.Text = "Введите серию и номер \r\nсвидетельства о гос.регистрации\r\nтранспортного средства\r\n(например: АБВ012345)";
                label2.Visible = true;
            }
            if (naznachenie == "Пополнение счета")
            {
                label1.Text = "Номер игрового счета";
            }
            if (label2.Visible == false)
            {
                label1.Top = label2.Top;
                textBox1.Top= label2.Top;
                button1.Top = label1.Top + label1.Height + 60;
            }
            Height = button1.Top+ button1.Height + 150;
            for (int i = 0; i < col_cards; i++)
            {
                try
                {
                    string str = "SELECT type_card,name,valyta,number_card,ostatok,raschetnaya,number_phone,PIN FROM bank_card,type,type_card,valyta WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")) AND type.id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND id_card =" + mas_id_card[i];
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(str, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string ostatok1 = reader[4].ToString();
                    ostatok1 = Convert.ToString(Convert.ToDouble(ostatok1) - Convert.ToDouble(ostatok1) % 0.01);
                    string[] mas = new string[10];
                    mas = reader[3].ToString().Split(' ');
                    comboBox1.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + mas[0] + "**** ****" + mas[3] + " остаток:" + ostatok1 + " " + reader[2].ToString());
                    if (reader[5].ToString()== "YES")
                    {
                        comboBox1.SelectedIndex = i;
                        id_selected = i;
                        valytacard = reader[2].ToString();
                        ostatok = reader[4].ToString();
                        number_card = mas[0] + "**** ****" + mas[3];
                        number_phone = reader[6].ToString();
                        kod = Convert.ToInt32(reader[7]);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = (ComboBox)sender;
            if (a != null)
            {
                id_selected = a.SelectedIndex;
            }
            try
            {
                string str = "SELECT valyta,number_card,ostatok,number_phone,PIN FROM bank_card,valyta WHERE id_card=" + mas_id_card[id_selected]+ " AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[id_selected] + ")";
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(str, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                string[] mas = new string[10];
                mas = reader[1].ToString().Split(' ');
                valytacard = reader[0].ToString();
                ostatok = reader[2].ToString();
                number_card = mas[0] + "**** ****" + mas[3];
                number_phone = reader[3].ToString();
                if (number_phone == "")
                {
                    label4.Text = "PIN-код";
                    linkLabel1.Visible = false;
                }
                else
                {
                    label4.Text = "Код подтверждения";
                    linkLabel1.Visible = true;
                }
                kod = Convert.ToInt32(reader[4]);
                reader.Close();
                conn.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
       {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Microsoft Word document(*.docx)|*.docx";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                try
                {
                    string[] mas = new string[5];
                    mas = saveFileDialog1.FileName.Split('.');
                    if (mas[1] == "docx" || mas[1] == "doc")
                    {
                        Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document doc = app.Documents.Add();
                        doc.Paragraphs[1].Range.Text = doc_content;
                        Clipboard.SetImage(pictureBox1.Image);
                        doc.Paragraphs[doc.Paragraphs.Count].Range.Paste();
                        app.Visible = false;
                        doc.SaveAs2(saveFileDialog1.FileName);
                        doc.Close();
                        app.Quit();
                        MessageBox.Show("Файл сохранен!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                        MessageBox.Show("Недопустимый тип файла!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();
            app.Visible = false;
           

            if (pDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(@"D:\Колледж\4 КУРС\ПРАКТИКА\интернет-банкинг\история\" + doc_name + ".docx");
                    app.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                    app.ActiveDocument.PrintOut();
                    doc.Close(SaveChanges: false);
                    doc = null;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
