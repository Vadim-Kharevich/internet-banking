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
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using MetroFramework.Forms;
using System.Runtime.InteropServices;

namespace интернет_банкинг
{
    public partial class user : MetroForm
    {
        const int WM_NCLBUTTONDOWN = 0x00A1;//запрет на перетаскивание формы
        const int WM_NCHITTEST = 0x0084;
        const int HTCAPTION = 2;
        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd,
        int Msg, IntPtr wParam, IntPtr lParam);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCLBUTTONDOWN)
            {
                int result = SendMessage(m.HWnd, WM_NCHITTEST,
                IntPtr.Zero, m.LParam);
                if (result == HTCAPTION)
                    return;
            }
            base.WndProc(ref m);
        }

        public user(string id_client,Form form)
        {
            InitializeComponent();
            main_form = form;
            this.id_client = id_client;
            int width = Screen.PrimaryScreen.Bounds.Width;
            panel1.Left = width - panel1.Width - 50;
            panel9.Left = width - panel9.Width - 50;
            panel2.Left = (width / 2) - (panel2.Width / 2) - 200;
            panel2.Top = 200;
            panel3.Left = (width / 2) - (panel3.Width / 2) - 200;
            panel3.Top = 200;
            panel4.Left = (width / 2) - (panel4.Width / 2) - 100;
            panel4.Top = 200;
            panel5.Left = (width / 2) - (panel5.Width / 2) - 100;
            panel5.Top = 200;
            panel6.Left = (width / 2) - (panel6.Width / 2) - 100;
            panel6.Top = 200;
            panel7.Left = (width / 2) - (panel7.Width / 2) - 150;
            panel7.Top = 200;
            panel8.Width = width / 3 * 2;
            panel8.Left = (width / 2) - (panel8.Width / 2);
            int rastoyanie = 50;
            int widthbutton = (panel8.Width - rastoyanie * 6) / 5;
            button1.Width = widthbutton;
            button2.Width = widthbutton;
            button6.Width = widthbutton;
            button3.Width = widthbutton;
            button4.Width = widthbutton;
            button1.Left = rastoyanie;
            button2.Left = button1.Left+ button1.Width + rastoyanie;
            button6.Left = button2.Left + button2.Width + rastoyanie;
            button3.Left = button6.Left + button6.Width + rastoyanie;
            button4.Left = button3.Left + button3.Width + rastoyanie;

            try
            {
                string sql = "SELECT FIO FROM bank_card WHERE id_client=" + id_client;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);                     // получение ФИО клиента
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                label1.Text += reader[0].ToString();
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            label2.Left = width - label2.Width - 150;
            label1.Left = label2.Left - label1.Width - 70;
        }

        Form main_form;
        string id_client; //id клиента
        int col_cards; //кол-во карт
        int col_deposit;//кол-во депозитов
        int col_credit;//кол-во кредитов
        int[] mas_id_card = new int[10];
        int[] mas_id_deposit = new int[20];
        int[] mas_id_credit = new int[20];
        int[] mas_id_plateg = new int[20];
        int id_raschetnaya; //расчетная карта
        string RUB="3,1"; //курс рубля
        string USD="2,1";//курс доллара
        string EUR="2,5";//курс евро

        private void label2_Click(object sender, EventArgs e)//выход
        {
            this.Hide();
            main_form.Show();
        }

        private void user_Load(object sender, EventArgs e) 
        {
            linkLabel2.Text = "Руководство \rпользователя. \rКарты";
            if(training()) timer1.Enabled = true;
            toolTip1.SetToolTip(button1, "Позволяет вывести на экран ваши банковские карты");
            toolTip2.SetToolTip(button2, "Позволяет вывести на экран список платежей");
            toolTip3.SetToolTip(button6, "Позволяет вывести на экран окно для совершения перевода");
            toolTip4.SetToolTip(button3, "Позволяет вывести на экран список ваших депозитов");
            toolTip5.SetToolTip(button4, "Позволяет вывести на экран список ваших кредитов");
            toolTip6.SetToolTip(panel1, "Возможность просматривать текущие курсы валют");
            toolTip7.SetToolTip(button5, "Позволяет вывести на экран историю платежей");
            toolTip8.SetToolTip(button8, "Позволяет вывести на экран окно для изменения пароля");
            toolTip9.SetToolTip(label2, "Позволяет выйти в главное меню");
            toolTip10.SetToolTip(label7, "Позволяет открыть окно для подбора подходящего депозита");
            toolTip11.SetToolTip(label8, "Позволяет открыть окно для открытия депозита");
            toolTip12.SetToolTip(label17, "Позволяет открыть окно для подбора подходящего кредита");
            toolTip13.SetToolTip(label16, "Позволяет открыть окно для открытия депозита");
            toolTip14.SetToolTip(label21, "Позволяет открыть окно для подбора подходящей банковской карты");
            toolTip15.SetToolTip(label20, "Позволяет открыть окно для создания новой карты");
            toolTip16.SetToolTip(groupBox5, "Служит для выбора необходимого платежа");
            toolTip20.SetToolTip(panel7, "Необходимо для отправки перевода на другую банковскую карту");
            toolTip21.SetToolTip(panel2, "Здесь выводятся ваши депозиты");
            toolTip21.SetToolTip(panel11, "Здесь выводятся ваши депозиты");
            toolTip23.SetToolTip(panel3, "Здесь выводятся ваши кредиты");
            toolTip23.SetToolTip(panel12, "Здесь выводятся ваши кредиты");
            toolTip22.SetToolTip(linkLabel1, "При активированной услуге sms-оповещение вам придет код подтверждения необходимый для оплаты");
            string rub = getcurs("RUB");        //вывод курса валют
            string usd = getcurs("USD");
            string eur = getcurs("EUR");
            if (rub != "" && usd != "" && eur != "")
            {
                RUB = rub.Replace('.',',');
                EUR = eur.Replace('.', ',');
                USD = usd.Replace('.', ',');
                label10.Text += RUB;   
                label11.Text += USD;
                label12.Text += EUR;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
            }
            else
            {
                label13.Visible = true;
                label14.Visible = true;
            }
            vivodcard();
            raschet_deposit();
            raschetcredit();
        }

        void raschet_deposit()//расчет и запись прибыли от депозита,его закрытие
        {
            try
            {
                string sql = "SELECT id_deposit_dopolnitelnaya,data_vidachi,data_raschet,nakopleniya,snytie,summa,data_okonchianiya FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read())
                {
                    int countmonth = 0;
                    bool close = false;
                    DateTime data_vidachi = new DateTime();
                    DateTime date1 = new DateTime();
                    DateTime data_okonchaniya = new DateTime();
                    data_okonchaniya = DateTime.Parse(reader1[6].ToString());
                    if (data_okonchaniya.Date <= DateTime.Now.Date)
                    {
                        if (reader1[3].ToString() == "0")
                        {
                            data_vidachi = DateTime.Parse(reader1[1].ToString());
                            while (data_vidachi < data_okonchaniya)
                            {
                                data_vidachi = data_vidachi.AddDays(30);
                                countmonth++;
                            }
                        }
                        else
                        {
                            data_vidachi = DateTime.Parse(reader1[2].ToString());
                            while (data_vidachi < data_okonchaniya)
                            {
                                data_vidachi = data_vidachi.AddDays(30);
                                countmonth++;
                            }
                        }
                        close = true;
                    }
                    else
                    {
                        if (reader1[3].ToString() == "0")
                        {
                            data_vidachi = DateTime.Parse(reader1[1].ToString());
                            date1 = data_vidachi.AddDays(30);
                            if (date1 < DateTime.Now)
                            {
                                while (date1 < DateTime.Now)
                                {
                                    date1 = date1.AddDays(30);
                                    data_vidachi = data_vidachi.AddDays(30);
                                    countmonth++;
                                }
                            }
                        }
                        else
                        {
                            data_vidachi = DateTime.Parse(reader1[2].ToString());
                            date1 = data_vidachi.AddDays(30);
                            if (date1 < DateTime.Now)
                            {
                                while (date1 < DateTime.Now)
                                {
                                    date1 = date1.AddDays(30);
                                    data_vidachi = data_vidachi.AddDays(30);
                                    countmonth++;
                                }
                            }
                        }
                    }

                    string sql2 = "SELECT procent_deposit FROM deposit WHERE id_deposit=(SELECT id_deposit FROM deposit_dopolnitelnaya WHERE id_deposit_dopolnitelnaya=" + reader1[0] + ")";
                    MySqlConnection conn2 = DBUtils.GetDBConnection();
                    conn2.Open();
                    MySqlCommand command2 = new MySqlCommand(sql2, conn2);
                    MySqlDataReader reader2 = command2.ExecuteReader();
                    reader2.Read();
                    string nakopleniya = Convert.ToString((Convert.ToDouble(reader1[5]) * Convert.ToDouble(reader2[0]) * 30 / 36500) * countmonth);
                    nakopleniya = nakopleniya.Replace(',', '.');
                    reader2.Close();
                    conn2.Close();
                    if (countmonth != 0)
                    {
                        if (reader1[4].ToString() == "" && close == false)
                        {
                            string sql1 = "UPDATE deposit_dopolnitelnaya SET data_raschet='" + data_vidachi.ToString("yyyy-MM-dd") + "', nakopleniya=nakopleniya+'" + nakopleniya + "' WHERE id_deposit_dopolnitelnaya=" + reader1[0].ToString();
                            conn2 = DBUtils.GetDBConnection();
                            conn2.Open();
                            command2 = new MySqlCommand(sql1, conn2);
                            reader2 = command2.ExecuteReader();
                            reader2.Read();
                            reader2.Close();
                            conn2.Close();
                        }
                        if (reader1[4].ToString() != "" && close == false)
                        {
                            if (date1 > data_okonchaniya) date1 = data_okonchaniya;
                            string sql1 = "UPDATE deposit_dopolnitelnaya SET data_raschet='" + data_vidachi.ToString("yyyy-MM-dd") + "', nakopleniya=nakopleniya+'" + nakopleniya + "',snytie='" + date1.ToString("yyyy-MM-dd") + "' WHERE id_deposit_dopolnitelnaya=" + reader1[0].ToString();
                            conn2 = DBUtils.GetDBConnection();
                            conn2.Open();
                            command2 = new MySqlCommand(sql1, conn2);
                            reader2 = command2.ExecuteReader();
                            reader2.Read();
                            reader2.Close();
                            conn2.Close();
                        }
                        if (close == true)
                        {
                            string sql1 = "UPDATE deposit_dopolnitelnaya SET data_raschet='" + data_vidachi.ToString("yyyy-MM-dd") + "', nakopleniya+='" + nakopleniya + "',close='YES' WHERE id_deposit_dopolnitelnaya=" + reader1[0].ToString();
                            conn2 = DBUtils.GetDBConnection();
                            conn2.Open();
                            command2 = new MySqlCommand(sql1, conn2);
                            reader2 = command2.ExecuteReader();
                            reader2.Read();
                            reader2.Close();
                            conn2.Close();
                        }
                    }
                }
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void raschetcredit()
        {
            try
            {
                string sql = "SELECT id_credit_dopolnitelnaya,data_vidachi,data_raschet,data_okonchianiya,summa_month,summa_procent,vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(sql, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();

                while (reader1.Read())
                {
                    int countmonth = 0;
                   // bool close = false;
                   // DateTime data_vidachi = new DateTime();
                   // DateTime data_raschetplus30 = new DateTime();
                    DateTime data_raschet = new DateTime();
                    DateTime data_okonchaniya = new DateTime();
                    data_okonchaniya = DateTime.Parse(reader1[3].ToString());
                    data_raschet = DateTime.Parse(reader1[2].ToString());
                    if (data_raschet.Date <= DateTime.Now.Date)
                    {
                        
                        while (data_raschet < DateTime.Now)
                        {
                            if (data_raschet <= data_okonchaniya)
                            {
                                data_raschet = data_raschet.AddDays(30);
                                countmonth++;
                            }
                            
                        }

                    }
                    if (countmonth != 0)
                    {
                        double vystavleno = Convert.ToDouble(reader1[6].ToString().Replace('.', ',')) + (Convert.ToDouble(reader1[5].ToString().Replace('.', ',')) + Convert.ToDouble(reader1[4].ToString().Replace('.', ','))) * countmonth;
                        string str = "UPDATE credit_dopolnitelnaya SET data_raschet='"+data_raschet.ToString("yyyy-MM-dd")+ "',vystavleno='" + vystavleno.ToString().Replace(',','.')+ "' WHERE id_credit_dopolnitelnaya=" + reader1[0];
                        MySqlConnection conn2 = DBUtils.GetDBConnection();
                        conn2.Open();
                        MySqlCommand command2 = new MySqlCommand(str, conn2);
                        MySqlDataReader reader2 = command2.ExecuteReader();
                        reader2.Read();
                        reader2.Close();
                        conn2.Close();
                    }
                }
                reader1.Close();
                conn1.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void vivodcard()  // вывод карт клиента
        {
            panel10.Controls.Clear();
            col_cards = 0;
            
            try
            {
                string sql = "SELECT id_card FROM bank_card WHERE id_client=" + id_client;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);                     // получение ФИО клиента и его карт
                MySqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    col_cards++;
                    mas_id_card[i] = Convert.ToInt32(reader[0]);
                    i++;
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            PictureBox[] picturebox = new PictureBox[10]; 
            Label[] lbinformation = new Label[10];
            Label[] lbostatok = new Label[10];
            Label[] lbraschetnaya = new Label[10];
            ComboBox[] combobox = new ComboBox[10];
            int coord = 50;
            int coordlabel = 60;
            for (int i = 0; i < col_cards; i++)
            {
                int coordleft = 20;
                combobox[i] = new ComboBox();
                picturebox[i] = new PictureBox();
                lbinformation[i] = new Label();
                lbostatok[i] = new Label();
                lbraschetnaya[i] = new Label();

                combobox[i].Width = 17;
                picturebox[i].Width = 160;
                lbostatok[i].Width = 100;
                lbinformation[i].Width = 250;
                lbraschetnaya[i].Width = 200;

                Font font1 = new Font("Tahoma", 9);
                Font font2 = new Font("Tahoma", 10);
                lbostatok[i].Font = font1;
                lbinformation[i].Font = font2;
                lbraschetnaya[i].Font = font1;

                picturebox[i].Height = 97;
                lbinformation[i].Height = 97;

                lbinformation[i].Top = coordlabel;
                lbostatok[i].Top = coordlabel;
                picturebox[i].Top = coord;
                combobox[i].Top = coordlabel;
                lbraschetnaya[i].Top = coordlabel;

                picturebox[i].Left = coordleft;
                coordleft += picturebox[i].Width + 20;
                lbinformation[i].Left = coordleft;
                coordleft += lbinformation[i].Width + 20;
                lbostatok[i].Left = coordleft;
                coordleft += lbostatok[i].Width + 20;
                combobox[i].Left = coordleft;
                coordleft += combobox[i].Width + 20;
                lbraschetnaya[i].Left = coordleft;
                coordleft += lbraschetnaya[i].Width + 20;

                lbraschetnaya[i].Text = "Карта является расчетной";
                lbraschetnaya[i].Visible = false;
                combobox[i].Items.Add("сделать расчетной");
                combobox[i].Items.Add("настроить sms-оповещение");
                combobox[i].Items.Add("изменить PIN");
                combobox[i].Items.Add("заблокировать");
                combobox[i].DropDownWidth = 150;
                combobox[i].Name = i + "box";
                combobox[i].SelectedIndexChanged += comboBox_SelectedIndexChanged;

                panel10.Controls.Add(picturebox[i]);
                panel10.Controls.Add(lbinformation[i]);
                panel10.Controls.Add(lbostatok[i]);
                panel10.Controls.Add(combobox[i]);
                panel10.Controls.Add(lbraschetnaya[i]);

                toolTip17.SetToolTip(combobox[i], "Применяется для просмотра действий доступных по данной карте");
                toolTip18.SetToolTip(lbostatok[i], "Применяется для просмотра остатка и валюты карты");
                toolTip19.SetToolTip(lbinformation[i], "Применяется для просмотра информации по данной карте");

                coord += 130;
                coordlabel += 130;
                panel10.Width = coordleft;
            }
            if(coordlabel<(Height-250))
                panel10.Height = coordlabel;
            else
                panel10.Height = Height-300;
            panel4.Height = panel10.Height + 50;
            panel4.Width = panel10.Width + 50;

            void comboBox_SelectedIndexChanged(object sender1, EventArgs ex)
            {
                var a = (ComboBox)sender1;
                if (a != null)
                {
                    int b = Convert.ToInt32(a.Name[0]) - 48;
                    string selectedState = a.SelectedItem.ToString();
                    if (selectedState == "сделать расчетной")
                    {
                        raschetnaya(mas_id_card[b]);
                        lbraschetnaya[b].Visible = true;
                    }
                    if (selectedState == "настроить sms-оповещение")
                    {
                        smsandpin form = new smsandpin("sms",mas_id_card[b],id_client);
                        form.Show();
                    }
                    if (selectedState == "изменить PIN")
                    {
                        smsandpin form = new smsandpin("pin", mas_id_card[b], id_client);
                        form.Show();
                    }
                    if (selectedState == "заблокировать")
                    {
                        if (col_cards > 1)
                        {
                            DialogResult dialog = MessageBox.Show("Вы действительно хотите заблокировать карту?\r\nРазблокировать карту будет невозможно", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog == DialogResult.Yes)
                            {
                                try
                                {
                                    string sql = "DELETE FROM history WHERE id_card = " + mas_id_card[b];
                                    MySqlConnection conn = DBUtils.GetDBConnection();
                                    conn.Open();
                                    MySqlCommand command = new MySqlCommand(sql, conn);
                                    MySqlDataReader reader = command.ExecuteReader();
                                    reader.Read();
                                    reader.Close();
                                    conn.Close();

                                    sql = "DELETE FROM bank_card WHERE id_card = " + mas_id_card[b];
                                    conn = DBUtils.GetDBConnection();
                                    conn.Open();
                                    command = new MySqlCommand(sql, conn);
                                    reader = command.ExecuteReader();
                                    reader.Read();
                                    reader.Close();
                                    conn.Close();
                                }
                                catch (Exception exc)
                                {
                                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            vivodcard();
                        }
                        else
                        {
                            MessageBox.Show("Вы не можете заблокировать эту карту!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

            }

            void raschetnaya(int id_card)
            {
                if (id_card != id_raschetnaya)
                {
                    try
                    {
                        string sql = "UPDATE bank_card SET raschetnaya = 'YES' WHERE id_card = " + id_card;
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                        conn.Close();

                        sql = "UPDATE bank_card SET raschetnaya = NULL WHERE id_card = " + id_raschetnaya;
                        conn = DBUtils.GetDBConnection();
                        conn.Open();
                        command = new MySqlCommand(sql, conn);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                        conn.Close();

                        id_raschetnaya = id_card;
                        for (int i = 0; i < col_cards; i++)
                        {
                            lbraschetnaya[i].Visible = false;
                        }
                        MessageBox.Show("Расчетная карта изменена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная карта уже является расчетной!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            try
            {
                string sql = "SELECT id_type,number_card,srok_deystviya,ostatok,id_valyta,raschetnaya,id_card FROM bank_card JOIN client ON bank_card.id_client=client.id_client AND client.id_client=" + id_client;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                for (int i = 0; i < col_cards; i++)
                {
                    reader.Read();
                    string sql1 = "SELECT type_card,name,image from type_card,type WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=" + reader[0].ToString() + ") AND type.id_type=" + reader[0].ToString();
                    MySqlConnection conn1 = DBUtils.GetDBConnection();
                    conn1.Open();
                    MySqlCommand command1 = new MySqlCommand(sql1, conn1);
                    MySqlDataReader reader1 = command1.ExecuteReader();
                    reader1.Read();
                    lbinformation[i].Text = reader1[0].ToString() + " ";
                    lbinformation[i].Text += reader1[1].ToString() + "\r\n";
                    picturebox[i].Image = Image.FromFile(reader1[2].ToString());
                    reader1.Close();
                    conn1.Close();

                    string[] mas = new string[10];
                    mas = reader[1].ToString().Split(' ');
                    lbinformation[i].Text += "Номер: " + mas[0] + " **** **** " + mas[3] + "\r\n";
                    DateTime date = new DateTime();
                    date = DateTime.Parse(reader[2].ToString());
                    lbinformation[i].Text += "Действительна до: " + date.ToShortDateString();

                    string ostatok = reader[3].ToString().Replace('.',',');
                    ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(ostatok) % 0.01);
                    lbostatok[i].Text = ostatok + " ";

                    sql1 = "SELECT valyta from valyta WHERE id_valyta=" + reader[4].ToString();
                    conn1 = DBUtils.GetDBConnection();
                    conn1.Open();
                    command1 = new MySqlCommand(sql1, conn1);
                    reader1 = command1.ExecuteReader();
                    reader1.Read();
                    lbostatok[i].Text += reader1[0].ToString();
                    reader1.Close();
                    conn1.Close();

                    if (reader[5].ToString() == "YES")
                    {
                        id_raschetnaya = Convert.ToInt32(reader[6]);
                        lbraschetnaya[i].Visible = true; ;
                    }
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void user_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)//кнопка "депозиты"
        {
            linkLabel2.Text = "Руководство \rпользователя. \rДепозиты";
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            string sql = "SELECT COUNT(*) FROM deposit_dopolnitelnaya WHERE id_client="+id_client;
            string sql1 = "SELECT * FROM deposit JOIN deposit_dopolnitelnaya ON deposit.id_deposit=deposit_dopolnitelnaya.id_deposit AND deposit_dopolnitelnaya.id_client="+id_client;
            string sql2 = "SELECT snytie,nakopleniya,close FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
            vivoddeposit(sql, sql1, sql2);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            deposit form = new deposit(mas_id_card, mas_id_deposit, col_cards, col_deposit, id_client, RUB, EUR, USD);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                string sql = "SELECT COUNT(*) FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
                string sql1 = "SELECT * FROM deposit JOIN deposit_dopolnitelnaya ON deposit.id_deposit=deposit_dopolnitelnaya.id_deposit AND deposit_dopolnitelnaya.id_client=" + id_client;
                string sql2 = "SELECT snytie,nakopleniya,close FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
                vivoddeposit(sql, sql1, sql2);
            }
            form.FormClosed += Selected;
        }

        void vivoddeposit(string sql, string sql1, string sql2) //вывод депозита
        {
            try
            {
                panel11.Controls.Clear();
                panel11.Visible = true;
                label6.Visible = false;
                col_deposit = 0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                col_deposit = Convert.ToInt32(reader[0]);
                reader.Close();
                conn.Close();

                if (col_deposit != 0)
                {
                    PictureBox[] picturebox = new PictureBox[20];
                    Label[] name = new Label[20];
                    Label[] srok = new Label[20];
                    Label[] okonchyanie = new Label[20];
                    Label[] snyatie = new Label[20];
                    Label[] procent = new Label[20];
                    Label[] summa = new Label[20];
                    ComboBox[] combobox = new ComboBox[20];

                    int coord = 60;
                    int coordtop = 85;

                    name[0] = new Label();
                    srok[0] = new Label();
                    okonchyanie[0] = new Label();
                    snyatie[0] = new Label();
                    procent[0] = new Label();
                    summa[0] = new Label();
                    panel11.Controls.Add(name[0]);
                    panel11.Controls.Add(srok[0]);
                    panel11.Controls.Add(okonchyanie[0]);
                    panel11.Controls.Add(snyatie[0]);
                    panel11.Controls.Add(procent[0]);
                    panel11.Controls.Add(summa[0]);

                    MySqlConnection connection = DBUtils.GetDBConnection();
                    connection.Open();
                    MySqlCommand comm = new MySqlCommand(sql2, connection);
                    MySqlDataReader read = comm.ExecuteReader();
                    
                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 1; i <= col_deposit; i++)
                    {
                        read.Read();
                        reader.Read();
                        mas_id_deposit[i] = Convert.ToInt32(reader[0]);
                        int coordleft = 10;
                        picturebox[i] = new PictureBox();
                        name[i] = new Label();
                        srok[i] = new Label();
                        okonchyanie[i] = new Label();
                        snyatie[i] = new Label();
                        procent[i] = new Label();
                        summa[i] = new Label();
                        combobox[i] = new ComboBox();

                        picturebox[i].Width = 120;
                        name[i].Width = 200;
                        srok[i].Width = 70;
                        okonchyanie[i].Width = 100;
                        snyatie[i].Width = 100;
                        procent[i].Width = 70;
                        summa[i].Width = 100;
                        combobox[i].Width = 100;

                        picturebox[i].Height = 80;
                        name[i].Height = 50;
                        combobox[i].Height = 30;

                        picturebox[i].Top = coord;
                        name[i].Top = coordtop;
                        srok[i].Top = coordtop;
                        okonchyanie[i].Top = coordtop;
                        snyatie[i].Top = coordtop;
                        procent[i].Top = coordtop;
                        summa[i].Top = coordtop;
                        combobox[i].Top = coordtop;

                        picturebox[i].Left = coordleft;
                        coordleft += picturebox[i].Width + 20;
                        name[0].Left = coordleft;
                        name[i].Left = coordleft;
                        coordleft += name[i].Width + 20;
                        srok[0].Left = coordleft;
                        srok[i].Left = coordleft;
                        coordleft += srok[i].Width + 20;
                        okonchyanie[0].Left = coordleft;
                        okonchyanie[i].Left = coordleft;
                        coordleft += okonchyanie[i].Width + 20;
                        snyatie[0].Left = coordleft;
                        snyatie[i].Left = coordleft;
                        coordleft += snyatie[i].Width + 20;
                        procent[0].Left = coordleft;
                        procent[i].Left = coordleft;
                        coordleft += procent[i].Width + 20;
                        summa[0].Left = coordleft;
                        summa[i].Left = coordleft;
                        coordleft += summa[i].Width + 20;
                        combobox[i].Left = coordleft;
                        coordleft += combobox[i].Width + 20;
                        panel11.Width = coordleft+15;

                        combobox[i].Name = i + "btn";
                        combobox[i].SelectedIndexChanged += comboBox_SelectedIndexChanged;
                        combobox[i].Visible = false;
                        combobox[i].Text = "Действия";
                        if (reader[7].ToString()=="YES" && read[2].ToString() != "YES")
                        {
                            combobox[i].Items.Add("Снять денежные средства");
                            combobox[i].Visible = true;
                        }
                        if (reader[8].ToString() == "YES" && read[2].ToString() != "YES")
                        {
                            combobox[i].Items.Add("Пополнить депозит");
                            combobox[i].Visible = true;
                        }
                        if (reader[11].ToString() == "YES" && read[2].ToString() != "YES")
                        {
                            combobox[i].Items.Add("Ежемесячное снятие");
                            combobox[i].Visible = true;
                        }
                        if (read[2].ToString() == "YES")
                        {
                            combobox[i].Items.Add("Закрыть депозит");
                            combobox[i].Visible = true;
                        }
                        combobox[i].DropDownWidth = 150;
                        combobox[i].KeyPress += comboBox_KeyPress;
                        name[i].TextAlign = ContentAlignment.TopCenter;
                        srok[i].TextAlign = ContentAlignment.TopCenter;
                        okonchyanie[i].TextAlign = ContentAlignment.TopCenter;
                        snyatie[i].TextAlign = ContentAlignment.TopCenter;
                        procent[i].TextAlign = ContentAlignment.TopCenter;
                        summa[i].TextAlign = ContentAlignment.TopCenter;
                        picturebox[i].BackgroundImageLayout = ImageLayout.Center;

                        panel11.Controls.Add(picturebox[i]);
                        panel11.Controls.Add(name[i]);
                        panel11.Controls.Add(srok[i]);
                        panel11.Controls.Add(okonchyanie[i]);
                        panel11.Controls.Add(snyatie[i]);
                        panel11.Controls.Add(procent[i]);
                        panel11.Controls.Add(summa[i]);
                        panel11.Controls.Add(combobox[i]);
                        coord += picturebox[i].Height + 20;
                        coordtop += picturebox[i].Height + 20;

                        
                        name[i].Text = reader[2].ToString();
                        picturebox[i].BackgroundImage = Image.FromFile(reader[10].ToString());
                        if (read[2].ToString() != "YES")
                        {
                            srok[i].Text = reader[3].ToString() + " дней";
                            string str = "SELECT data_okonchianiya,snytie,summa FROM deposit_dopolnitelnaya JOIN deposit ON deposit.id_deposit=deposit_dopolnitelnaya.id_deposit AND deposit_dopolnitelnaya.id_client=" + id_client + " AND deposit_dopolnitelnaya.id_deposit=" + reader[0].ToString();
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(str, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();

                            DateTime date = new DateTime();
                            date = DateTime.Parse(reader1[0].ToString());
                            okonchyanie[i].Text = date.ToShortDateString();
                            if (reader1[1].ToString() == "")
                            {
                                snyatie[i].Text = "-";
                            }
                            else
                            {
                                date = DateTime.Parse(reader1[1].ToString());
                                snyatie[i].Text = date.ToShortDateString();
                            }
                            summa[i].Text = Math.Round(Convert.ToDouble(reader1[2]), 2).ToString();
                            reader1.Close();
                            conn1.Close();

                            str = "SELECT valyta FROM valyta WHERE id_valyta=" + reader[1].ToString();
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(str, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            procent[i].Text = reader[4].ToString() + "%";
                            summa[i].Text += " " + reader1[0].ToString();
                           
                            reader1.Close();
                            conn1.Close();
                        }
                        else
                        {
                            srok[i].Text = "Срок действия депозита закончен.\r\nВы можете получить накопленные средства в подменю \"Действия\"";
                            srok[i].Width = 200;
                            srok[i].Height = 50;
                        }
                        if (coordtop < (Height - 250))
                        {
                            panel11.Height = coordtop;
                        }
                        else
                            panel11.Height = Height - 300;
                    }
                    reader.Close();
                    conn.Close();

                    name[0].Width = 200;
                    srok[0].Width = 70;
                    okonchyanie[0].Width = 100;
                    snyatie[0].Width = 100;
                    procent[0].Width = 70;
                    summa[0].Width = 100;

                    name[0].Text = "Название";
                    okonchyanie[0].Text = "Дата окончания";
                    procent[0].Text = "Процентная ставка";
                    srok[0].Text = "Срок";
                    summa[0].Text = "Сумма";
                    snyatie[0].Text = "Дата следующего снятия";

                    name[0].Top = 10;
                    okonchyanie[0].Top = 10;
                    procent[0].Top = 10;
                    srok[0].Top = 10;
                    summa[0].Top = 10;
                    snyatie[0].Top = 10;

                    name[0].Height = 40;
                    okonchyanie[0].Height = 40;
                    procent[0].Height = 40;
                    srok[0].Height = 40;
                    summa[0].Height = 40;
                    snyatie[0].Height = 40;

                    name[0].TextAlign = ContentAlignment.MiddleCenter;
                    okonchyanie[0].TextAlign = ContentAlignment.MiddleCenter;
                    procent[0].TextAlign = ContentAlignment.MiddleCenter;
                    srok[0].TextAlign = ContentAlignment.MiddleCenter;
                    summa[0].TextAlign = ContentAlignment.MiddleCenter;
                    snyatie[0].TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Width = panel11.Width + 20;
                    panel2.Height = panel11.Height + 70;
                }
                else
                {
                    label6.Visible = true;
                    label6.BringToFront();
                    panel11.Visible = false;
                    panel11.Width = 1000;
                    panel11.Height = 100;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        void vivodcredit(string sql, string sql1,string sql2) //вывод кредита
        {
            try
            {
                panel12.Controls.Clear();
                label15.Visible = false;
                col_credit = 0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                col_credit = Convert.ToInt32(reader[0]);
                reader.Close();
                conn.Close();

                if (col_credit != 0)
                {
                    PictureBox[] picturebox = new PictureBox[20];
                    Label[] name = new Label[20];
                    Label[] srok = new Label[20];
                    Label[] okonchyanie = new Label[20];
                    Label[] vyplata = new Label[20];
                    Label[] procent = new Label[20];
                    Label[] summa = new Label[20];
                    ComboBox[] combobox = new ComboBox[20];

                    int coord = 60;
                    int coordtop = 85;

                    name[0] = new Label();
                    srok[0] = new Label();
                    okonchyanie[0] = new Label();
                    vyplata[0] = new Label();
                    procent[0] = new Label();
                    summa[0] = new Label();
                    panel12.Controls.Add(name[0]);
                    panel12.Controls.Add(srok[0]);
                    panel12.Controls.Add(okonchyanie[0]);
                    panel12.Controls.Add(vyplata[0]);
                    panel12.Controls.Add(procent[0]);
                    panel12.Controls.Add(summa[0]);

                    MySqlConnection connection = DBUtils.GetDBConnection();
                    connection.Open();
                    MySqlCommand comm = new MySqlCommand(sql2, connection);
                    MySqlDataReader read = comm.ExecuteReader();

                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 1; i <= col_credit; i++)
                    {
                        read.Read();
                        reader.Read();
                        mas_id_credit[i] = Convert.ToInt32(reader[0]);
                        int coordleft = 20;
                        picturebox[i] = new PictureBox();
                        name[i] = new Label();
                        srok[i] = new Label();
                        okonchyanie[i] = new Label();
                        vyplata[i] = new Label();
                        procent[i] = new Label();
                        summa[i] = new Label();
                        combobox[i] = new ComboBox();

                        picturebox[i].Width = 120;
                        name[i].Width = 170;
                        srok[i].Width = 70;
                        okonchyanie[i].Width = 100;
                        vyplata[i].Width = 100;
                        procent[i].Width = 70;
                        summa[i].Width = 100;
                        combobox[i].Width = 100;

                        picturebox[i].Height = 80;
                        name[i].Height = 50;
                        combobox[i].Height = 30;

                        picturebox[i].Top = coord;
                        name[i].Top = coordtop;
                        srok[i].Top = coordtop;
                        okonchyanie[i].Top = coordtop;
                        vyplata[i].Top = coordtop;
                        procent[i].Top = coordtop;
                        summa[i].Top = coordtop;
                        combobox[i].Top = coordtop;

                        picturebox[i].Left = coordleft;
                        coordleft += picturebox[i].Width + 20;
                        name[0].Left = coordleft;
                        name[i].Left = coordleft;
                        coordleft += name[i].Width + 20;
                        srok[0].Left = coordleft;
                        srok[i].Left = coordleft;
                        coordleft += srok[i].Width + 20;
                        okonchyanie[0].Left = coordleft;
                        okonchyanie[i].Left = coordleft;
                        coordleft += okonchyanie[i].Width + 20;
                        vyplata[0].Left = coordleft;
                        vyplata[i].Left = coordleft;
                        coordleft += vyplata[i].Width + 20;
                        procent[0].Left = coordleft;
                        procent[i].Left = coordleft;
                        coordleft += procent[i].Width + 20;
                        summa[0].Left = coordleft;
                        summa[i].Left = coordleft;
                        coordleft += summa[i].Width + 20;
                        combobox[i].Left = coordleft;
                        coordleft += combobox[i].Width + 20;
                        panel12.Width = coordleft+20;

                        combobox[i].Name = i + "btn";
                        combobox[i].SelectedIndexChanged += comboBox_SelectedIndexChanged;
                        combobox[i].Visible = false;
                        combobox[i].Text = "Действия";
                        combobox[i].Items.Add("Погасить кредит");
                        if (read[0].ToString() != "0")
                        {
                            combobox[i].Items.Add("Оплата кредита");
                        }
                        combobox[i].Visible = true;
                        combobox[i].DropDownWidth = 150;
                        combobox[i].KeyPress += comboBox_KeyPress;
                        name[i].TextAlign = ContentAlignment.TopCenter;
                        srok[i].TextAlign = ContentAlignment.TopCenter;
                        okonchyanie[i].TextAlign = ContentAlignment.TopCenter;
                        vyplata[i].TextAlign = ContentAlignment.TopCenter;
                        procent[i].TextAlign = ContentAlignment.TopCenter;
                        summa[i].TextAlign = ContentAlignment.TopCenter;
                        picturebox[i].BackgroundImageLayout = ImageLayout.Center;

                        panel12.Controls.Add(picturebox[i]);
                        panel12.Controls.Add(name[i]);
                        panel12.Controls.Add(srok[i]);
                        panel12.Controls.Add(okonchyanie[i]);
                        panel12.Controls.Add(vyplata[i]);
                        panel12.Controls.Add(procent[i]);
                        panel12.Controls.Add(summa[i]);
                        panel12.Controls.Add(combobox[i]);
                        coord += picturebox[i].Height + 20;
                        coordtop += picturebox[i].Height + 20;


                        name[i].Text = reader[2].ToString();
                        picturebox[i].BackgroundImage = Image.FromFile(reader[8].ToString());
                        srok[i].Text = reader[3].ToString() + " дней";
                        string str = "SELECT data_okonchianiya,data_raschet,summa FROM credit_dopolnitelnaya JOIN credit ON credit.id_credit=credit_dopolnitelnaya.id_credit AND credit_dopolnitelnaya.id_client=" + id_client + " AND credit_dopolnitelnaya.id_credit=" + reader[0].ToString();
                        MySqlConnection conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        MySqlCommand command1 = new MySqlCommand(str, conn1);
                        MySqlDataReader reader1 = command1.ExecuteReader();
                        reader1.Read();

                        DateTime date = new DateTime();
                        date = DateTime.Parse(reader1[0].ToString());
                        okonchyanie[i].Text = date.ToShortDateString();
                        date = DateTime.Parse(reader1[1].ToString());
                        vyplata[i].Text = date.ToShortDateString();

                        summa[i].Text = Math.Round(Convert.ToDouble(reader1[2]), 2).ToString();
                        reader1.Close();
                        conn1.Close();

                        str = "SELECT valyta FROM valyta WHERE id_valyta=" + reader[1].ToString();
                        conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        command1 = new MySqlCommand(str, conn1);
                        reader1 = command1.ExecuteReader();
                        reader1.Read();
                        procent[i].Text = reader[4].ToString() + "%";
                        summa[i].Text += " " + reader1[0].ToString();

                        reader1.Close();
                        conn1.Close();
                       
                    }
                    read.Close();
                    connection.Close();
                    reader.Close();
                    conn.Close();

                    if (coord < (Height - 250))
                    {
                        panel12.Height = coord;
                        panel3.Height = panel12.Height + 80;
                    }
                    else
                    {
                        panel3.Height = Height - 250;
                        panel12.Height = panel3.Height - 50;
                    }
                        
                    panel3.Width = panel12.Width + 30;
                    

                    name[0].Width = 170;
                    srok[0].Width = 70;
                    okonchyanie[0].Width = 100;
                    vyplata[0].Width = 100;
                    procent[0].Width = 70;
                    summa[0].Width = 100;

                    name[0].Text = "Название";
                    okonchyanie[0].Text = "Дата окончания";
                    procent[0].Text = "Процентная ставка";
                    srok[0].Text = "Срок";
                    summa[0].Text = "Непогашенная сумма";
                    vyplata[0].Text = "Дата следующего взноса";

                    name[0].Top = 10;
                    okonchyanie[0].Top = 10;
                    procent[0].Top = 10;
                    srok[0].Top = 10;
                    summa[0].Top = 10;
                    vyplata[0].Top = 10;

                    name[0].Height = 40;
                    okonchyanie[0].Height = 40;
                    procent[0].Height = 40;
                    srok[0].Height = 40;
                    summa[0].Height = 40;
                    vyplata[0].Height = 40;

                    name[0].TextAlign = ContentAlignment.MiddleCenter;
                    okonchyanie[0].TextAlign = ContentAlignment.MiddleCenter;
                    procent[0].TextAlign = ContentAlignment.MiddleCenter;
                    srok[0].TextAlign = ContentAlignment.MiddleCenter;
                    summa[0].TextAlign = ContentAlignment.MiddleCenter;
                    vyplata[0].TextAlign = ContentAlignment.MiddleCenter;
                }
                else
                {
                    label15.Visible = true;
                    label15.BringToFront();
                    panel12.Visible = false;
                    panel12.Width = 1000;
                    panel12.Height = 100;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        void comboBox_SelectedIndexChanged(object sender1, EventArgs ex)
        {
            var a = (ComboBox)sender1;
            if (a != null)
            {
                int b = Convert.ToInt32(a.Name[0]) - 48;
                string selected = a.SelectedItem.ToString();
                if (selected == "Снять денежные средства") //досрочное закрытие депозита
                {
                    DialogResult result = MessageBox.Show("Вы действительно хотите закрыть депозит? При досрочном закрытии депозита сбережения обнуляются.", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        selectcard form = new selectcard(mas_id_card, col_cards,mas_id_deposit[b],id_client,RUB,EUR,USD);
                        form.Show();
                        form.FormClosed += UpdateDeposit;
                    }
                }
                if (selected == "Пополнить депозит")//пополнение депозита
                {
                    selectcard form = new selectcard(mas_id_card, col_cards, mas_id_deposit[b], id_client, RUB, EUR, USD);
                    form.Show();
                    form.label1.Text = "Выберите карту для пополнения депозита";
                    form.button1.Text = "Далее";
                    form.FormClosed += UpdateDeposit;
                }
                if(selected=="Закрыть депозит")
                {
                    selectcard form = new selectcard(mas_id_card, col_cards, mas_id_deposit[b], id_client, RUB, EUR, USD);
                    form.Show();
                    form.label1.Text = "Выберите карту для начисления";
                    form.comboBox1.Top += 35;
                    form.label3.Visible = true;
                    try
                    {
                        string str = "SELECT nakopleniya,valyta FROM deposit_dopolnitelnaya,valyta WHERE id_client=" + id_client + " AND id_deposit=" + mas_id_deposit[b] + " AND id_valyta=(SELECT id_valyta FROM deposit WHERE id_deposit=" + mas_id_deposit[b] + ")";
                        MySqlConnection connection = DBUtils.GetDBConnection();
                        connection.Open();
                        MySqlCommand comm = new MySqlCommand(str, connection);
                        MySqlDataReader read = comm.ExecuteReader();
                        read.Read();
                        form.label3.Text += " " + read[0] +" "+ read[1];
                        read.Close();
                        connection.Close();
                        form.FormClosed += UpdateDeposit;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (selected == "Ежемесячное снятие")//ежемесячное снятие
                {
                    try
                    {
                        string str = "SELECT nakopleniya,valyta FROM deposit_dopolnitelnaya,valyta WHERE id_client=" + id_client + " AND id_deposit=" + mas_id_deposit[b] + " AND id_valyta=(SELECT id_valyta FROM deposit WHERE id_deposit=" + mas_id_deposit[b] + ")";
                        MySqlConnection connection = DBUtils.GetDBConnection();
                        connection.Open();
                        MySqlCommand comm = new MySqlCommand(str, connection);
                        MySqlDataReader read = comm.ExecuteReader();
                        read.Read();
                        if(Convert.ToInt32(read[0])>=1)
                        {
                            string nakopleniya = read[0].ToString().Replace('.', ',');
                            nakopleniya = Convert.ToString(Convert.ToDouble(nakopleniya) - Convert.ToDouble(nakopleniya) % 0.01);
                            selectcard form = new selectcard(mas_id_card, col_cards, mas_id_deposit[b], id_client, RUB, EUR, USD);
                            form.Show();
                            form.label1.Text = "Выберите карту для начисления средств";
                            form.label3.Visible = true;
                            form.comboBox1.Top += 30;
                            form.textBox1.Top += 30;
                            form.label2.Top += 30;
                            form.label3.Text += " " + nakopleniya + " " + read[1];
                            form.FormClosed += UpdateDeposit;
                        }
                        else
                        {
                            MessageBox.Show("Нет накопленных средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        read.Close();
                        connection.Close();
                        
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if(selected=="Погасить кредит")//закрытие кредита
                {
                    try
                    {
                        string str = "SELECT summa,summa_month,vystavleno,valyta FROM credit_dopolnitelnaya,valyta WHERE id_client=" + id_client + " AND id_credit=" + mas_id_credit[b] + " AND id_valyta=(SELECT id_valyta FROM credit WHERE id_credit=" + mas_id_credit[b] + ")";
                        MySqlConnection connection = DBUtils.GetDBConnection();
                        connection.Open();
                        MySqlCommand comm = new MySqlCommand(str, connection);
                        MySqlDataReader read = comm.ExecuteReader();
                        read.Read();
                        string summa = read[0].ToString().Replace('.', ',');
                        if (Convert.ToInt32(read[2]) >= 1)
                        {
                            summa = Convert.ToString(Convert.ToDouble(summa) - Convert.ToDouble(read[1].ToString().Replace('.', ','))+ Convert.ToDouble(read[2].ToString().Replace('.', ',')));
                            summa = Convert.ToString(Convert.ToDouble(summa) - Convert.ToDouble(summa) % 0.01);
                        }
                        else
                        {
                            summa = Convert.ToString(Convert.ToDouble(summa) - Convert.ToDouble(summa) % 0.01);
                        }
                        selectcard form = new selectcard(mas_id_card, col_cards, mas_id_credit[b], id_client, RUB, EUR, USD);
                        form.Show();
                        form.label1.Text = "Выберите карту для расчета";
                        form.label3.Visible = true;
                        form.label3.Text = "Выставленная сумма";
                        form.comboBox1.Top += 30;
                        form.textBox1.Top += 30;
                        form.label2.Top += 30;
                        form.label3.Text += " " + summa + " " + read[3];
                        form.FormClosed += UpdateCredit;

                        read.Close();
                        connection.Close();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (selected == "Оплата кредита")
                {
                    try
                    {
                        string str = "SELECT vystavleno,valyta FROM credit_dopolnitelnaya,valyta WHERE id_client=" + id_client + " AND id_credit=" + mas_id_credit[b] + " AND id_valyta=(SELECT id_valyta FROM credit WHERE id_credit=" + mas_id_credit[b] + ")";
                        MySqlConnection connection = DBUtils.GetDBConnection();
                        connection.Open();
                        MySqlCommand comm = new MySqlCommand(str, connection);
                        MySqlDataReader read = comm.ExecuteReader();
                        read.Read();
                        string summa = read[0].ToString().Replace('.', ',');
                        summa = Convert.ToString(Convert.ToDouble(summa) - Convert.ToDouble(summa) % 0.01);
                        selectcard form = new selectcard(mas_id_card, col_cards, mas_id_credit[b], id_client, RUB, EUR, USD);
                        form.Show();
                        form.label1.Text = "Выберите карту для оплаты";
                        form.label3.Visible = true;
                        form.label3.Text = "Выставленная сумма";
                        form.comboBox1.Top += 30;
                        form.textBox1.Top += 30;
                        form.label2.Top += 30;
                        form.label3.Text += " " + summa + " " + read[1];
                        form.FormClosed += UpdateCredit;

                        read.Close();
                        connection.Close();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        void UpdateDeposit(object sender2, EventArgs e2)
        {
            string sql = "SELECT COUNT(*) FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
            string sql1 = "SELECT * FROM deposit JOIN deposit_dopolnitelnaya ON deposit.id_deposit=deposit_dopolnitelnaya.id_deposit AND deposit_dopolnitelnaya.id_client=" + id_client;
            string sql2 = "SELECT snytie,nakopleniya,close FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
            vivoddeposit(sql, sql1, sql2);
        }

        void UpdateCredit(object sender2, EventArgs e2)
        {
            string sql = "SELECT COUNT(*) FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
            string sql1 = "SELECT * FROM credit JOIN credit_dopolnitelnaya ON credit.id_credit=credit_dopolnitelnaya.id_credit AND credit_dopolnitelnaya.id_client=" + id_client;
            string sql2 = "SELECT vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
            vivodcredit(sql, sql1, sql2);
        }

        private void button1_Click(object sender, EventArgs e)//кнопка "карты"
        {
            linkLabel2.Text = "Руководство \rпользователя. \rКарты";
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            vivodcard();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            depositopen form = new depositopen(mas_id_card,mas_id_deposit,col_cards,col_deposit,id_client,RUB,EUR,USD);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                string sql = "SELECT COUNT(*) FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
                string sql1 = "SELECT * FROM deposit JOIN deposit_dopolnitelnaya ON deposit.id_deposit=deposit_dopolnitelnaya.id_deposit AND deposit_dopolnitelnaya.id_client=" + id_client;
                string sql2 = "SELECT snytie,nakopleniya,close FROM deposit_dopolnitelnaya WHERE id_client=" + id_client;
                vivoddeposit(sql, sql1, sql2);
                this.Focus();
            }
            form.FormClosed += Selected;
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            var a = (Label)sender;
            Cursor = Cursors.Hand;
            a.Font = new Font(a.Font, FontStyle.Regular);
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            var a = (Label)sender;
            Cursor = Cursors.Arrow;
            a.Font = new Font(a.Font, FontStyle.Underline);
        }

        string getcurs(string valyta)   //получение курса валюты
        {
            // Адрес сайта с курсом валюты
            string url = "http://www.nbrb.by/API/ExRates/Rates/";
            if (valyta == "RUB") url += 298;
            if (valyta == "USD") url += 145;
            if (valyta == "EUR") url += 292;
            string html = "";
            try
            {
                // Отправляем GET запрос и получаем в ответ HTML-код сайта с курсом валюты
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());
                html = myStreamReader.ReadToEnd();
            }
            catch 
            {
                
            }
            // Вытаскиваем из HTML-кода нужные данные
            Match match = Regex.Match(html, @"\d\.\d{4}");
            return match.ToString();
        }

        private void label14_Click(object sender, EventArgs e) //обновление курса валют
        {
            string rub = getcurs("RUB");        
            string usd = getcurs("USD");
            string eur = getcurs("EUR");
            if (rub != "" && usd != "" && eur != "")
            {
                label10.Text += rub;
                label11.Text += usd;
                label12.Text += eur;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = false;
                label14.Visible = false;
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            credit form = new credit(mas_id_card, mas_id_credit, col_cards, col_credit, id_client, RUB, EUR, USD);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                string sql = "SELECT COUNT(*) FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
                string sql1 = "SELECT * FROM credit JOIN credit_dopolnitelnaya ON credit.id_credit=credit_dopolnitelnaya.id_credit AND credit_dopolnitelnaya.id_client=" + id_client;
                string sql2 = "SELECT vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
                vivodcredit(sql, sql1, sql2);
            }
            form.FormClosed += Selected;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            linkLabel2.Text = "Руководство \rпользователя. \rКредиты";
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            string sql = "SELECT COUNT(*) FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
            string sql1 = "SELECT * FROM credit JOIN credit_dopolnitelnaya ON credit.id_credit=credit_dopolnitelnaya.id_credit AND credit_dopolnitelnaya.id_client=" + id_client;
            string sql2 = "SELECT vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
            vivodcredit(sql, sql1, sql2);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            creditopen form = new creditopen(mas_id_card, mas_id_credit, col_cards, col_credit, id_client, RUB, EUR, USD);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                string sql = "SELECT COUNT(*) FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
                string sql1 = "SELECT * FROM credit JOIN credit_dopolnitelnaya ON credit.id_credit=credit_dopolnitelnaya.id_credit AND credit_dopolnitelnaya.id_client=" + id_client;
                string sql2 = "SELECT vystavleno FROM credit_dopolnitelnaya WHERE id_client=" + id_client;
                vivodcredit(sql, sql1, sql2);
                this.Focus();
            }
            form.FormClosed += Selected;
        }

        private void label21_Click(object sender, EventArgs e)
        {
            card form = new card(mas_id_card, col_cards, id_client);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                vivodcard();
            }
            form.FormClosed += Selected;
        }

        private void label20_Click(object sender, EventArgs e)
        {
            cardopen form = new cardopen(mas_id_card,col_cards,id_client);
            form.Show();
            void Selected(object sender2, EventArgs e2)
            {
                vivodcard();
            }
            form.FormClosed += Selected;
        }
        int col_zapros = 0;
        string[,] derevo_plategei = new string[20,3];

        private void button2_Click(object sender, EventArgs e)
        {
            linkLabel2.Text = "Руководство \rпользователя. \rПлатежи";
            col_zapros = 0;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = true;
            panel6.Visible = false;
            panel7.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            label30.Visible = false;
            label31.Visible = false;
            string sql = "SELECT COUNT(*) FROM category";
            string sql1 = "SELECT * FROM category ORDER by id_category";
            derevo_plategei[0, 0] = "Платежи";
            derevo_plategei[0, 1] = sql;
            derevo_plategei[0, 2] = sql1;
            vivodplategi(sql,sql1);
        }

        void vivodplategi(string sql,string sql1)
        {
            col_zapros++;
            try
            {
                foreach (var label in groupBox5.Controls.OfType<Label>().Except(new[] {label22, label23 , label24 , label25 , label26 , label27 , label28 , label29 , label30 , label31}).ToList())
                {
                    groupBox5.Controls.Remove(label);
                }
                foreach (var img in groupBox5.Controls.OfType<PictureBox>().ToList())
                {
                    groupBox5.Controls.Remove(img);
                }
                col_deposit = 0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                int col = 0;
                while (reader.Read())
                {
                    col++;
                    col_deposit = Convert.ToInt32(reader[0]);
                }
                if(col>1||col_zapros>1) col_deposit = col;
                reader.Close();
                conn.Close();

                if (col != 0)
                {
                    PictureBox[] picturebox = new PictureBox[20];
                    Label[] column = new Label[20];

                    int topimg = 100;
                    int toptext = 100;
                    if (col_zapros == 1)
                    {
                        toptext = 200;
                    }
                    int left = 20;
                    int width = 245;

                    int col_on_column = col_deposit / 3;
                    int dop_on_column = col_deposit % 3;
                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 0; i < col_deposit; i++)
                    {
                        reader.Read();
                        mas_id_plateg[i] = Convert.ToInt32(reader[0]);

                        picturebox[i] = new PictureBox();
                        column[i] = new Label();

                        column[i].Width = width;
                        column[i].Top = toptext;
                        column[i].Left = left;
                        column[i].Name = i.ToString();
                        column[i].Click += column_Click;
                        column[i].MouseEnter += column_MouseEnter;
                        column[i].MouseLeave += column_MouseLeave;
                        column[i].Font = new Font("Tahoma", 10, FontStyle.Underline);
                        column[i].TextAlign = ContentAlignment.TopCenter;
                        groupBox5.Controls.Add(column[i]);



                        column[i].Text = reader[1].ToString();

                        if (col_zapros == 1)
                        {
                            picturebox[i].Width = width;
                            picturebox[i].Height =100;
                            picturebox[i].Top = topimg;
                            picturebox[i].Left = left;
                            picturebox[i].Name = i.ToString();
                            picturebox[i].Click += picturebox_Click;
                            picturebox[i].MouseEnter += image_MouseEnter;
                            picturebox[i].MouseLeave += image_MouseLeave;
                            groupBox5.Controls.Add(picturebox[i]);
                            topimg += picturebox[i].Height + column[i].Height + 50;
                            toptext += picturebox[i].Height + column[i].Height + 50;
                            picturebox[i].BackgroundImage = Image.FromFile(reader[2].ToString());
                            picturebox[i].BackgroundImageLayout = ImageLayout.Center;
                        }
                        else
                            toptext += column[i].Height + 50;

                        if (i == col_on_column - 1 && dop_on_column == 0 || i == col_on_column && dop_on_column > 0)
                        {
                            groupBox5.Height = toptext;
                            if (col_zapros == 1)
                                groupBox5.Height = 600;
                        }
                        if (i == col_on_column - 1 && dop_on_column == 0 || i == col_on_column * 2 - 1 && dop_on_column == 0 || i == col_on_column && dop_on_column > 0 || i == col_on_column * 2 + 1 && dop_on_column == 2|| i == col_on_column * 2 && dop_on_column == 1)
                        {
                            topimg = 100;
                            toptext = 100;
                            if (col_zapros == 1) toptext = 200;
                            left += width + 50;
                        }
                    }
                    reader.Close();
                    conn.Close();
                    groupBox5.Width = 870;
                    panel5.Width = groupBox5.Width + 50;
                    panel5.Height = groupBox5.Height + 15;

                    if (col_zapros == 2)
                    {
                        label24.Visible = true;
                        label25.Visible = true;
                        label25.Text = derevo_plategei[1, 0];
                        label25.Click += label_Click;
                    }
                    if (col_zapros == 3)
                    {
                        label26.Visible = true;
                        label27.Visible = true;
                        label26.Left = label25.Left + label25.Width + 5;
                        label27.Left = label26.Left + label26.Width + 5;
                        label27.Text = derevo_plategei[2, 0];
                        label27.Click += label_Click;
                    }
                    if (col_zapros == 4)
                    {
                        label28.Visible = true;
                        label29.Visible = true;
                        if (derevo_plategei[3, 0] == "")
                        {
                            label29.Text = derevo_plategei[2, 0];
                            label28.Left = label25.Left + label25.Width + 5;
                            label29.Left = label28.Left + label28.Width + 5;
                        }
                        else
                        {
                            label29.Text = derevo_plategei[3, 0];
                            label28.Left = label27.Left + label27.Width + 5;
                            label29.Left = label28.Left + label28.Width + 5;
                        }
                        label29.Click += label_Click;
                    }
                    if (col_zapros == 5)
                    {
                        label30.Visible = true;
                        label31.Visible = true;
                        if (derevo_plategei[4, 0] == ""&& derevo_plategei[3, 0] == "")
                        {
                            label31.Text = derevo_plategei[2, 0];
                            label30.Left = label25.Left + label25.Width + 5;
                            label31.Left = label30.Left + label30.Width + 5;
                        }
                        else
                        {
                            if (derevo_plategei[4, 0] == "")
                            {
                                label31.Text = derevo_plategei[3, 0];
                                label30.Left = label27.Left + label27.Width + 5;
                                label31.Left = label30.Left + label30.Width + 5;
                            }
                            else
                            {

                                label30.Left = label29.Left + label29.Width + 5;
                                label31.Left = label30.Left + label30.Width + 5;
                                label31.Text = derevo_plategei[4, 0];

                            }
                        }
                        label31.Click += label_Click;
                    }
                    void picturebox_Click(object sender, EventArgs e)
                    {
                        var a = (PictureBox)sender;
                        if (a != null)
                        {
                            int b = Convert.ToInt32(a.Name);
                            string sql11 = "";
                            string sql12 = "";
                            sql11 = "SELECT COUNT(*) FROM podcategory JOIN plategi ON podcategory.id_podcategory=plategi.id_podcategory AND plategi.id_category=" + mas_id_plateg[b] + " GROUP BY plategi.id_podcategory";
                            sql12 = "SELECT podcategory.id_podcategory,podcategory FROM podcategory JOIN plategi ON podcategory.id_podcategory=plategi.id_podcategory AND plategi.id_category=" + mas_id_plateg[b] + " GROUP BY plategi.id_podcategory";
                            derevo_plategei[col_zapros, 0] = column[b].Text;
                            derevo_plategei[col_zapros, 1] = sql11;
                            derevo_plategei[col_zapros, 2] = sql12;
                            vivodplategi(sql11, sql12);
                        }
                    }
                }
                else
                {
                    if (col_zapros == 3)
                    {
                        sql = "SELECT COUNT(podpodcategory) FROM podpodcategory JOIN plategi ON podpodcategory.id_podpodcategory=plategi.id_podpodcategory AND plategi.id_category=(SELECT id_category FROM category WHERE category='" + derevo_plategei[1, 0] + "') GROUP BY plategi.id_podpodcategory";
                        sql1 = "SELECT podpodcategory.id_podpodcategory,podpodcategory FROM podpodcategory JOIN plategi ON podpodcategory.id_podpodcategory=plategi.id_podpodcategory AND plategi.id_category=(SELECT id_category FROM category WHERE category='" + derevo_plategei[1, 0] + "') GROUP BY plategi.id_podpodcategory";
                    }
                    if (col_zapros == 4)
                    {
                        sql = "SELECT COUNT(*) FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') AND plategi.id_category=(SELECT id_category FROM category WHERE category='" + derevo_plategei[1, 0] + "') GROUP BY plategi.id_naznachenie";
                        sql1 = "SELECT naznachenie.id_naznachenie,naznachenie FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') AND plategi.id_category=(SELECT id_category FROM category WHERE category='" + derevo_plategei[1, 0] + "')GROUP BY plategi.id_naznachenie";
                    }
                    derevo_plategei[col_zapros, 0] = "";
                    derevo_plategei[col_zapros, 1] = sql;
                    derevo_plategei[col_zapros, 2] = sql1;
                    vivodplategi(sql, sql1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void label_Click(object sender, EventArgs e)
        {
            var a = (Label)sender;
            if (a != null)
            {
                if (a.Name == "label23")
                {
                    col_zapros = 0;
                    vivodplategi(derevo_plategei[0, 1], derevo_plategei[0, 2]);
                    label24.Visible = false;
                    label25.Visible = false;
                    label26.Visible = false;
                    label27.Visible = false;
                    label28.Visible = false;
                    label29.Visible = false;
                    label30.Visible = false;
                    label31.Visible = false;
                }
                if (a.Name == "label25")
                {
                    col_zapros = 1;
                    vivodplategi(derevo_plategei[1, 1], derevo_plategei[1, 2]);
                    label26.Visible = false;
                    label27.Visible = false;
                    label28.Visible = false;
                    label29.Visible = false;
                    label30.Visible = false;
                    label31.Visible = false;
                }
                if (a.Name == "label27")
                {
                    col_zapros = 2;
                    vivodplategi(derevo_plategei[2, 1], derevo_plategei[2, 2]);
                    label28.Visible = false;
                    label29.Visible = false;
                    label30.Visible = false;
                    label31.Visible = false;
                }
                if (a.Name == "label29")
                {
                    col_zapros = 3;
                    vivodplategi(derevo_plategei[3, 1], derevo_plategei[3, 2]);
                    label30.Visible = false;
                    label31.Visible = false;
                }
                if (a.Name == "label31")
                {
                    col_zapros = 4;
                    vivodplategi(derevo_plategei[4, 1], derevo_plategei[4, 2]);
                }
            }
        }
        
        void column_Click(object sender, EventArgs e)
        {
            var a = (Label)sender;
            if (a != null)
            {
                int b = Convert.ToInt32(a.Name) ;
                if (b == 0 && col_zapros == 1)
                {
                    oplatakodyslygi form = new oplatakodyslygi(mas_id_card, col_cards, id_client, RUB, EUR, USD);
                    form.Show();
                }
                else
                {
                    string sql = "";
                    string sql1 = "";
                    if (col_zapros == 1)
                    {
                        sql = "SELECT COUNT(*) FROM podcategory JOIN plategi ON podcategory.id_podcategory=plategi.id_podcategory AND plategi.id_category=" + mas_id_plateg[b] + " GROUP BY plategi.id_podcategory";
                        sql1 = "SELECT podcategory.id_podcategory,podcategory FROM podcategory JOIN plategi ON podcategory.id_podcategory=plategi.id_podcategory AND plategi.id_category=" + mas_id_plateg[b] + " GROUP BY plategi.id_podcategory";
                        derevo_plategei[col_zapros, 0] = a.Text;
                        derevo_plategei[col_zapros, 1] = sql;
                        derevo_plategei[col_zapros, 2] = sql1;
                        vivodplategi(sql, sql1);
                        return;
                    }
                    if (col_zapros == 2)
                    {
                        sql = "SELECT COUNT(oblast) FROM oblast JOIN plategi ON oblast.id_oblast=plategi.id_oblast AND plategi.id_podcategory=" + mas_id_plateg[b] + " GROUP BY plategi.id_oblast";
                        sql1 = "SELECT oblast.id_oblast,oblast FROM oblast JOIN plategi ON oblast.id_oblast=plategi.id_oblast AND plategi.id_podcategory=" + mas_id_plateg[b] + " GROUP BY plategi.id_oblast";
                        derevo_plategei[col_zapros, 0] = a.Text;
                        derevo_plategei[col_zapros, 1] = sql;
                        derevo_plategei[col_zapros, 2] = sql1;
                        vivodplategi(sql, sql1);
                        return;
                    }
                    if (col_zapros == 3)
                    {
                        sql = "SELECT COUNT(podpodcategory) FROM podpodcategory JOIN plategi ON podpodcategory.id_podpodcategory=plategi.id_podpodcategory AND plategi.id_oblast=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') GROUP BY plategi.id_podpodcategory";
                        sql1 = "SELECT podpodcategory.id_podpodcategory,podpodcategory FROM podpodcategory JOIN plategi ON podpodcategory.id_podpodcategory=plategi.id_podpodcategory AND plategi.id_oblast=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') GROUP BY plategi.id_podpodcategory";
                        derevo_plategei[col_zapros, 0] = a.Text;
                        derevo_plategei[col_zapros, 1] = sql;
                        derevo_plategei[col_zapros, 2] = sql1;
                        vivodplategi(sql, sql1);
                        return;
                    }
                    if (col_zapros == 4)
                    {
                        if (derevo_plategei[3, 0] == "")
                        {
                            sql = "SELECT COUNT(*) FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podpodcategory=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') GROUP BY plategi.id_naznachenie";
                            sql1 = "SELECT naznachenie.id_naznachenie,naznachenie FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podpodcategory=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') GROUP BY plategi.id_naznachenie";
                        }
                        else
                        {
                            sql = "SELECT COUNT(*) FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podpodcategory=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') AND plategi.id_oblast=(SELECT id_oblast FROM oblast WHERE oblast='" + derevo_plategei[3, 0] + "') GROUP BY plategi.id_naznachenie";
                            sql1 = "SELECT naznachenie.id_naznachenie,naznachenie FROM naznachenie JOIN plategi ON naznachenie.id_naznachenie=plategi.id_naznachenie AND plategi.id_podpodcategory=" + mas_id_plateg[b] + " AND plategi.id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') AND plategi.id_oblast=(SELECT id_oblast FROM oblast WHERE oblast='" + derevo_plategei[3, 0] + "') GROUP BY plategi.id_naznachenie";
                        }
                        derevo_plategei[col_zapros, 0] = a.Text;
                        derevo_plategei[col_zapros, 1] = sql;
                        derevo_plategei[col_zapros, 2] = sql1;
                        vivodplategi(sql, sql1);
                        return;
                    }
                    if (col_zapros == 5)
                    {
                        sql = "SELECT id_plateg,kod_uslygi FROM plategi WHERE id_category=(SELECT id_category FROM category WHERE category='" + derevo_plategei[1, 0] + "') AND id_podcategory=(SELECT id_podcategory FROM podcategory WHERE podcategory='" + derevo_plategei[2, 0] + "') AND id_naznachenie=(SELECT id_naznachenie FROM naznachenie WHERE naznachenie='" + a.Text + "')";
                        if (derevo_plategei[3, 0] != "")
                        {
                            sql += " AND id_oblast=(SELECT id_oblast FROM oblast WHERE oblast='" + derevo_plategei[3, 0] + "')";
                        }
                        if (derevo_plategei[4, 0] != "")
                        {
                            sql += " AND id_podpodcategory=(SELECT id_podpodcategory FROM podpodcategory WHERE podpodcategory='" + derevo_plategei[4, 0] + "')";
                        }
                        try
                        {
                            MySqlConnection conn = DBUtils.GetDBConnection();
                            conn.Open();
                            MySqlCommand command = new MySqlCommand(sql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            string id_plateg = reader[0].ToString();
                            string nomer_uslygi = reader[1].ToString();
                            reader.Close();
                            conn.Close();
                            plategi form = new plategi(mas_id_card, col_cards, id_client, id_plateg, a.Text, RUB, EUR, USD, nomer_uslygi);
                            form.Show();
                            form.FormClosed += plategi_Close;
                            void plategi_Close(object sender2, EventArgs e2)
                            {
                                label24.Visible = false;
                                label25.Visible = false;
                                label26.Visible = false;
                                label27.Visible = false;
                                label28.Visible = false;
                                label29.Visible = false;
                                label30.Visible = false;
                                label31.Visible = false;
                                col_zapros = 0;
                                vivodplategi(derevo_plategei[0, 1], derevo_plategei[0, 2]);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        void image_MouseEnter(object sender, EventArgs e)
        {
            var a = (PictureBox)sender;
            Cursor = Cursors.Hand;
        }

        void image_MouseLeave(object sender, EventArgs e)
        {
            var a = (PictureBox)sender;
            Cursor = Cursors.Arrow;
        }

        void column_MouseEnter(object sender, EventArgs e)
        {
            var a = (Label)sender;
            a.Font = new Font(a.Font, FontStyle.Regular);
            Cursor = Cursors.Hand;
        }

        void column_MouseLeave(object sender, EventArgs e)
        {
            var a = (Label)sender;
            a.Font = new Font(a.Font, FontStyle.Underline);
            Cursor = Cursors.Arrow;
        }

        int id_selected;
        int[] mas_id_history = new int[200];
        private void button5_Click(object sender, EventArgs e)
        {
            linkLabel2.Text = "Руководство \rпользователя. \rИстория платежей";
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = true;
            panel7.Visible = false;
            dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(-1);
            for (int i = 0; i < col_cards; i++)
            {
                try
                {
                    string str = "SELECT type_card,name,valyta,number_card,raschetnaya FROM bank_card,type,type_card,valyta WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")) AND type.id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND id_card =" + mas_id_card[i];
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(str, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string[] mas = new string[10];
                    mas = reader[3].ToString().Split(' ');
                    comboBox1.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString() + " " + mas[0] + "**** ****" + mas[3] );
                    if (reader[4].ToString() == "YES")
                    {
                        comboBox1.SelectedIndex = i;
                        id_selected = i;
                    }
                    reader.Close();
                    conn.Close();

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            string sql = "SELECT COUNT(*) FROM history WHERE id_client="+id_client+" AND id_card="+mas_id_card[id_selected]+" AND datetime BETWEEN '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd")+" 00:00' AND '"+ dateTimePicker2.Value.ToString("yyyy-MM-dd")+" 23:59:59'";
            string sql1 = "SELECT id_history, datetime, summa, information_history,id_plateg,id_valyta FROM history WHERE id_client=" + id_client + " AND id_card=" + mas_id_card[id_selected] + " AND datetime BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            vivodhistory(sql,sql1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = (ComboBox)sender;
            if (a != null)
            {
                id_selected = a.SelectedIndex;
            }
            string sql = "SELECT COUNT(*) FROM history WHERE id_client=" + id_client + " AND id_card=" + mas_id_card[id_selected] + " AND datetime BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            string sql1 = "SELECT id_history, datetime, summa, information_history,id_plateg,id_valyta FROM history WHERE id_client=" + id_client + " AND id_card=" + mas_id_card[id_selected] + " AND datetime BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            vivodhistory(sql, sql1);
        }

        private void label36_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
        }

        private void label37_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-6);
        }

        private void label38_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-12);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM history WHERE id_client=" + id_client + " AND id_card=" + mas_id_card[id_selected] + " AND datetime BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            string sql1 = "SELECT id_history, datetime, summa, information_history,id_plateg,id_valyta FROM history WHERE id_client=" + id_client + " AND id_card=" + mas_id_card[id_selected] + " AND datetime BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
            vivodhistory(sql, sql1);
        }

        void vivodhistory(string sql, string sql1) //вывод истории
        {
            foreach (var label in panel6.Controls.OfType<Label>().Except(new[] { label32, label33, label34, label35, label36, label37, label38, label39 }).ToList())
            {
                panel6.Controls.Remove(label);
            }
            foreach (var box in panel6.Controls.OfType<ComboBox>().Except(new[] { comboBox1 }).ToList())
            {
                panel6.Controls.Remove(box);
            }
            try
            {
                label39.Visible = false;
                int col_history = 0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                col_history = Convert.ToInt32(reader[0]);
                reader.Close();
                conn.Close();

                if (col_history != 0)
                {

                    Label[] data = new Label[col_history + 1];
                    Label[] operation = new Label[col_history + 1];
                    Label[] symma = new Label[col_history + 1];
                    ComboBox[] combobox = new ComboBox[col_history + 1];

                    int coordtop = 130;

                    data[0] = new Label();
                    operation[0] = new Label();
                    symma[0] = new Label();

                    panel6.Controls.Add(data[0]);
                    panel6.Controls.Add(operation[0]);
                    panel6.Controls.Add(symma[0]);

                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 1; i <= col_history; i++)
                    {
                        reader.Read();
                        mas_id_history[i] = Convert.ToInt32(reader[0]);
                        int coordleft = 20;

                        data[i] = new Label();
                        operation[i] = new Label();
                        symma[i] = new Label();
                        combobox[i] = new ComboBox();

                        data[i].Width = 150;
                        operation[i].Width = 200;
                        symma[i].Width = 100;
                        combobox[i].Width = 100;

                        data[i].Top = coordtop;
                        operation[i].Top = coordtop;
                        symma[i].Top = coordtop;
                        combobox[i].Top = coordtop;

                        data[0].Left = coordleft;
                        data[i].Left = coordleft;
                        coordleft += data[i].Width + 20;
                        operation[0].Left = coordleft;
                        operation[i].Left = coordleft;
                        coordleft += operation[i].Width + 20;
                        symma[0].Left = coordleft;
                        symma[i].Left = coordleft;
                        coordleft += symma[i].Width + 20;
                        combobox[i].Left = coordleft;
                        coordleft += combobox[i].Width + 20;
                        panel6.Width = coordleft + 100;

                        combobox[i].Name = i.ToString();
                        combobox[i].SelectedIndexChanged += comboBoxhistory_SelectedIndexChanged;
                        if (reader[4].ToString() != "")
                        {
                            combobox[i].Text = "Действия";
                            combobox[i].Items.Add("Сохранить");
                            combobox[i].Items.Add("Печать");
                        }
                        else
                            combobox[i].Visible = false;
                        combobox[i].KeyPress += comboBox_KeyPress;
                        data[i].TextAlign = ContentAlignment.TopCenter;
                        operation[i].TextAlign = ContentAlignment.TopLeft;
                        symma[i].TextAlign = ContentAlignment.TopCenter;

                        panel6.Controls.Add(data[i]);
                        panel6.Controls.Add(operation[i]);
                        panel6.Controls.Add(symma[i]);

                        panel6.Controls.Add(combobox[i]);

                        coordtop += 50;

                        data[i].Text = reader[1].ToString();
                        operation[i].Text = reader[3].ToString();

                        symma[i].Text = Math.Round(Convert.ToDouble(reader[2]),2)+ " ";
                        if (reader[5].ToString() == "1")
                            symma[i].Text += "BYN";
                        if (reader[5].ToString() == "2")
                            symma[i].Text += "RUB";
                        if (reader[5].ToString() == "3")
                            symma[i].Text += "EUR";
                        if (reader[5].ToString() == "4")
                            symma[i].Text += "USD";
                    }
                    if(coordtop < (Height - 250))
                    {
                        panel6.Height = coordtop;
                    }
                    else
                        panel6.Height = Height - 250;
                    reader.Close();
                    conn.Close();

                    data[0].Width = 150;
                    operation[0].Width = 200;
                    symma[0].Width = 100;

                    data[0].Text = "Дата платежа";
                    symma[0].Text = "Сумма";
                    operation[0].Text = "Операция";

                    data[0].Top = 90;
                    symma[0].Top = 90;
                    operation[0].Top = 90;

                    data[0].TextAlign = ContentAlignment.MiddleCenter;
                    symma[0].TextAlign = ContentAlignment.MiddleCenter;
                    operation[0].TextAlign = ContentAlignment.MiddleCenter;
                }
                else
                {
                    label39.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            void comboBoxhistory_SelectedIndexChanged(object sender1, EventArgs ex)
            {
                var a = (ComboBox)sender1;
                if (a != null)
                {
                    int b = Convert.ToInt32(a.Name);
                    if(a.SelectedItem.ToString() == "Сохранить")
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
                                    Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(@"D:\Колледж\4 КУРС\ПРАКТИКА\интернет-банкинг\история\" + mas_id_history[b] + ".docx");
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
                    if (a.SelectedItem.ToString() == "Печать")
                    {
                        Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();
                        app.Visible = false;

                        if (pDialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(@"D:\Колледж\4 КУРС\ПРАКТИКА\интернет-банкинг\история\" + mas_id_history[b] + ".docx");
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
        }

        int kod;
        string valyta3;
        private void button6_Click(object sender, EventArgs e)
        {
            linkLabel2.Text = "Руководство \rпользователя. \rПереводы";
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = true;
            comboBox3.SelectedIndex = 0;
            for (int i = 0; i < col_cards; i++)
            {
                try
                {
                    string str = "SELECT type_card,name,valyta,number_card,ostatok,raschetnaya FROM bank_card,type,type_card,valyta WHERE type_card.id_type_card=(SELECT type FROM type WHERE id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ")) AND type.id_type=(SELECT id_type FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND valyta.id_valyta=(SELECT id_valyta FROM bank_card WHERE id_card=" + mas_id_card[i] + ") AND id_card =" + mas_id_card[i];
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(str, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string ostatok = reader[4].ToString();
                    ostatok = Convert.ToString(Convert.ToDouble(ostatok) - Convert.ToDouble(ostatok) % 0.01);
                    string[] mas = new string[10];
                    mas = reader[3].ToString().Split(' ');
                    comboBox2.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + mas[0] + "**** ****" + mas[3] + " остаток:" + ostatok + " " + reader[2].ToString());
                    if (reader[5].ToString() == "YES")
                    {
                        comboBox2.SelectedIndex = i;
                        id_selected = i;
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            button7.Click += button7_Click;
            void button7_Click(object sender1, EventArgs ea)
            {
                try
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Введите номер карты получателя!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (textBox2.Text == "")
                        {
                            MessageBox.Show("Введите сумму перевода!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string ostatok1;
                            string ostatok2;
                            string valyta1;
                            string valyta2;
                            valyta3 = (comboBox3.SelectedIndex + 1).ToString();
                            string sql = "SELECT ostatok,id_valyta,PIN FROM bank_card WHERE id_card=" + mas_id_card[id_selected];
                            MySqlConnection conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            MySqlCommand command1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = command1.ExecuteReader();
                            reader1.Read();
                            ostatok1 = reader1[0].ToString();
                            valyta1 = reader1[1].ToString();
                            if (label43.Text == "PIN-код")
                            {
                                kod = Convert.ToInt32(reader1[2]);
                            }
                            reader1.Close();
                            conn1.Close();

                            sql = "SELECT ostatok,id_valyta,id_card FROM bank_card WHERE number_card='"+textBox1.Text+"'";
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(sql, conn1);
                            reader1 = command1.ExecuteReader();
                            if (reader1.Read())
                            {
                                ostatok2 = reader1[0].ToString();
                                valyta2 = reader1[1].ToString();
                                string id_card2 = reader1[2].ToString();
                                if (textBox3.Text != kod.ToString())
                                {
                                    MessageBox.Show("Неверный код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBox3.Text = "";
                                }
                                else
                                {
                                    string summazapyataya = textBox2.Text;
                                    string summazapyataya1 = textBox2.Text;
                                    bool sredstva = false;
                                    if (valyta1 == valyta3)
                                    {
                                        if (Convert.ToDouble(ostatok1.Replace('.',',')) >= Convert.ToDouble(summazapyataya))
                                        {
                                            sredstva = true;
                                        }
                                    }
                                    else
                                    {
                                        if (valyta3 == "1")
                                        {
                                            if (valyta1 == "2") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(RUB) * 100);
                                            if (valyta1 == "3") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(EUR));
                                            if (valyta1 == "4") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(USD));
                                        }
                                        else
                                        {
                                            if (valyta3 == "2") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / 100 * Convert.ToDouble(RUB));
                                            if (valyta3 == "3") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) * Convert.ToDouble(EUR));
                                            if (valyta3 == "4") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) * Convert.ToDouble(USD));
                                            if (valyta1 == "2") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(RUB) * 100);
                                            if (valyta1 == "3") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(EUR));
                                            if (valyta1 == "4") summazapyataya1 = Convert.ToString(Convert.ToDouble(summazapyataya1) / Convert.ToDouble(USD));
                                        }
                                        if (Convert.ToDouble(ostatok1.Replace('.', ',')) >= Convert.ToDouble(summazapyataya1))
                                        {
                                            sredstva = true;
                                        }
                                    }
                                    if (sredstva == true)
                                    {
                                        sql = "UPDATE bank_card SET ostatok=ostatok-'" + summazapyataya1.Replace(',','.') + "' WHERE id_card=" + mas_id_card[id_selected];
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(sql, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        if (valyta2 != valyta3)
                                        {
                                            if (valyta3 == "1")
                                            {
                                                if (valyta2 == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                                if (valyta2 == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                                if (valyta2 == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                            }
                                            else
                                            {
                                                if (valyta3 == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / 100 * Convert.ToDouble(RUB));
                                                if (valyta3 == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(EUR));
                                                if (valyta3 == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) * Convert.ToDouble(USD));
                                                if (valyta2 == "2") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(RUB) * 100);
                                                if (valyta2 == "3") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(EUR));
                                                if (valyta2 == "4") summazapyataya = Convert.ToString(Convert.ToDouble(summazapyataya) / Convert.ToDouble(USD));
                                            }
                                        }

                                        sql = "UPDATE bank_card SET ostatok=ostatok+'" + summazapyataya.Replace(',', '.') + "' WHERE id_card=" + id_card2;
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(sql, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();

                                        sql = "INSERT INTO history(id_client, id_card, datetime, summa, information_history,id_valyta) VALUES ('" + id_client + "','" + mas_id_card[id_selected] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + textBox2.Text.Replace(',', '.') + "','Перевод на карту " + textBox1.Text + "',"+(comboBox3.SelectedIndex+1)+")";
                                        conn1 = DBUtils.GetDBConnection();
                                        conn1.Open();
                                        command1 = new MySqlCommand(sql, conn1);
                                        reader1 = command1.ExecuteReader();
                                        reader1.Read();
                                        reader1.Close();
                                        conn1.Close();
                                        MessageBox.Show("Выполнен перевод на сумму "+textBox2.Text+" "+comboBox3.SelectedItem, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        textBox1.Text = ""; 
                                        textBox2.Text = "";
                                        col_vvod = -1;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Недостаточно средств!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Карта с номером "+textBox1.Text+" не найдена!\r\nПроверьте введенные данные.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            reader1.Close();
                            conn1.Close();
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = (ComboBox)sender;
            if (a != null)
            {
                id_selected = a.SelectedIndex;
            }
            try
            {
                string str = "SELECT number_phone FROM bank_card WHERE id_card =" + mas_id_card[id_selected];
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(str, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader[0].ToString() == "")
                {
                    label43.Text = "PIN-код";
                    linkLabel1.Visible = false;
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = (ComboBox)sender;
            if (a != null)
            {
                valyta3 = (a.SelectedIndex+1).ToString();
            }
        }
        int col_vvod = -1;
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
            else
            {
                if (number == 8)
                {
                    if (col_vvod > -1)
                    {
                        col_vvod--;
                    }
                }  
                else
                {
                    if (textBox1.Text.Length <= 18)
                    {
                        col_vvod++;
                    }
                }
            }
            if((col_vvod==4|| col_vvod == 9|| col_vvod == 14) && number!=8)
            {
                textBox1.Text += ' ';
                textBox1.SelectionStart = textBox1.Text.Length;
                col_vvod++;
            }
            if (textBox1.Text.Length > 18)
            {
                if (number != 8)
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            passwordchange form = new passwordchange(id_client);
            form.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            training training = new training(this, 4,id_client);
            training.Show();
            training.Activate();
            timer1.Enabled = false;
            timer1.Dispose();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Browser form = new Browser(this);
            if(linkLabel2.Text== "Руководство \rпользователя. \rКарты")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#cards");
            if (linkLabel2.Text == "Руководство \rпользователя. \rПлатежи")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#pays");
            if (linkLabel2.Text == "Руководство \rпользователя. \rПереводы")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#transfers");
            if (linkLabel2.Text == "Руководство \rпользователя. \rКредиты")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#credits");
            if (linkLabel2.Text == "Руководство \rпользователя. \rДепозиты")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#deposits");
            if (linkLabel2.Text == "Руководство \rпользователя. \rИстория платежей")
                form.webBrowser1.Navigate("file:///D:/Колледж/4%20КУРС/ПРАКТИКА/интернет-банкинг/help/help.html#history");
            form.Show();
        }

        bool training()
        {
            try
            {
                string sql = "SELECT training FROM client WHERE id_client=" + id_client;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);                     // получение информации о прохождении обучения
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                bool a = reader[0].ToString() == "YES";
                reader.Close();
                conn.Close();
                if (a == true) return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }
    }
}
