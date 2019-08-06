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
    public partial class deposit : Form
    {
        public deposit(int[] mas_id_card, int[] mas_id_deposit, int col_cards, int col_deposit, string id_client, string RUB, string EUR, string USD)
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
        string id_client;//id клиента
        string RUB; //курс рубля
        string EUR;// курс евро
        string USD;// курс доллара

        bool label5click = false;
        private void label5_Click(object sender, EventArgs e)
        {
            if(label5click==false)
            {
                label6.Visible = true;
                label8.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                panel2.Height = panel2.Height + 150;
                button1.Top = button1.Top + 150;
                label5.Top = label5.Top + 150;
                label11.Top = label11.Top + 150;
                label5click = true;
                panel1.Top += 150;
                label10.Top = button1.Top + button1.Height + 70;
            }
            else
            {
                label6.Visible = false;
                label8.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                panel2.Height = panel2.Height - 150;
                button1.Top = button1.Top - 150;
                label5.Top = label5.Top - 150;
                label11.Top = label11.Top - 150;
                label5click = false;
                panel1.Top -= 150;
                label10.Top = button1.Top + button1.Height + 70;
            }
        }

        int valyta;
        string procent1;
        string procent2;
        string srok1;
        string srok2;
        string summa1;
        string summa2;
        private void button1_Click(object sender, EventArgs e)
        {
            bool warning = false;
            summa1 = textBox4.Text;
            summa2 = textBox3.Text;
            string sql = "SELECT COUNT(*) FROM deposit WHERE id_deposit>0";
            string sql1 = "SELECT * FROM deposit WHERE id_deposit>0";
            if (valyta != 0)
            {
                sql += " AND id_valyta=" + valyta;
                sql1 += " AND id_valyta=" + valyta;
            }
            if (procent1 != null && procent2 != null)
            {
                sql += " AND procent_deposit BETWEEN " + procent1 + " AND " + procent2;
                sql1 += " AND procent_deposit BETWEEN " + procent1 + " AND " + procent2;
            }
            else
            {
                if (procent1 == null && procent2!=null)
                {
                    MessageBox.Show("Введите процентную ставку", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
                if (procent1 != null && procent2 == null)
                {
                    MessageBox.Show("Введите процентную ставку", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
            }
            if (srok1 != null && srok2 != null)
            {
                sql += " AND srok_deposit BETWEEN " + srok1 + " AND " + srok2;
                sql1 += " AND srok_deposit BETWEEN " + srok1 + " AND " + srok2;
            }
            else
            {
                if (srok1 == null && srok2 != null)
                {
                    MessageBox.Show("Введите срок", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
                if (srok1 != null && srok2 == null)
                {
                    MessageBox.Show("Введите срок", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
            }
            if (summa1 != "" && summa2 != "")
            {
                sql += " AND min_vznos>=" + summa1 + " AND max_vznos<=" + summa2;
                sql1 += " AND min_vznos>=" + summa1 + " AND max_vznos<=" + summa2;
            }
            else
            {
                if (summa1 == "" && summa2 != "")
                {
                    MessageBox.Show("Введите сумму", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
                if (summa1 != "" && summa2 == "")
                {
                    MessageBox.Show("Введите сумму", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    warning = true;
                }
            }
            if (checkBox1.Checked)
            {
                sql += " AND dosrochnoe_snyatie='YES'";
                sql1 += " AND dosrochnoe_snyatie='YES'";
            }
            if (checkBox2.Checked)
            {
                sql += " AND dopolnitelnyi_vznos='YES'";
                sql1 += " AND dopolnitelnyi_vznos='YES'";
            }
            if (checkBox3.Checked)
            {
                sql += " AND ejemesyachnoe_snyatie='YES'";
                sql1 += " AND ejemesyachnoe_snyatie='YES'";
            }
            if (warning == false)
            {
                panel1.Controls.Clear();
                vivoddeposit(sql, sql1);
            } 
        }

        private void deposit_Load(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM deposit WHERE id_deposit>0";
            string sql1 = "SELECT * FROM deposit WHERE id_deposit>0";
            vivoddeposit(sql, sql1);
        }

        void vivoddeposit(string sql,string sql1)
        {
            try
            {
                panel1.Controls.Clear();
                label10.Visible = false;
                int col_deposit=0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                col_deposit = Convert.ToInt32(reader[0]);
                reader.Close();
                conn.Close();

                if(col_deposit!=0)
                {
                    PictureBox[] picturebox = new PictureBox[20];
                    Label[] name = new Label[20];
                    Label[] valyta = new Label[20];
                    Label[] procent = new Label[20];
                    Label[] srok = new Label[20];
                    Label[] summa = new Label[20];
                    Label[] information = new Label[20];
                    Button[] button = new Button[20];

                    int coord = 50;
                    int coordtop = 75;

                    name[0] = new Label();
                    valyta[0] = new Label();
                    procent[0] = new Label();
                    srok[0] = new Label();
                    summa[0] = new Label();
                    information[0] = new Label();
                    panel1.Controls.Add(name[0]);
                    panel1.Controls.Add(valyta[0]);
                    panel1.Controls.Add(procent[0]);
                    panel1.Controls.Add(srok[0]);
                    panel1.Controls.Add(summa[0]);
                    panel1.Controls.Add(information[0]);

                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 1; i <= col_deposit; i++)
                    {
                        int coordleft = 20;
                        picturebox[i] = new PictureBox();
                        name[i] = new Label();
                        valyta[i] = new Label();
                        procent[i] = new Label();
                        srok[i] = new Label();
                        summa[i] = new Label();
                        information[i] = new Label();
                        button[i] = new Button();

                        picturebox[i].Width = 125;
                        name[i].Width = 200;
                        valyta[i].Width = 150;
                        procent[i].Width = 100;
                        srok[i].Width = 100;
                        summa[i].Width = 250;
                        information[i].Width = 200;
                        button[i].Width = 120;



                        picturebox[i].Height = 80;
                        name[i].Height = 80;
                        information[i].Height = 80;
                        button[i].Height = 40;
                        summa[i].Height = 80;

                        button[i].FlatStyle = 0;
                        button[i].FlatAppearance.BorderColor = Color.Black;
                        button[i].Font = new Font("Tahoma", 10, FontStyle.Bold);
                        button[i].ForeColor = Color.White;

                        picturebox[i].Top = coord;
                        name[i].Top = coordtop;
                        valyta[i].Top = coordtop;
                        procent[i].Top = coordtop;
                        srok[i].Top = coordtop;
                        summa[i].Top = coordtop;
                        information[i].Top = coordtop;
                        button[i].Top = coordtop;

                        picturebox[i].Left = coordleft;
                        coordleft += picturebox[i].Width + 20;
                        name[0].Left = coordleft;
                        name[i].Left = coordleft;
                        coordleft += name[i].Width + 20;
                        valyta[0].Left = coordleft;
                        valyta[i].Left = coordleft;
                        coordleft += valyta[i].Width + 20;
                        procent[0].Left = coordleft;
                        procent[i].Left = coordleft;
                        coordleft += procent[i].Width + 20;
                        srok[0].Left = coordleft;
                        srok[i].Left = coordleft;
                        coordleft += srok[i].Width + 20;
                        summa[0].Left = coordleft;
                        summa[i].Left = coordleft;
                        coordleft += summa[i].Width + 20;
                        information[0].Left = coordleft;
                        information[i].Left = coordleft;
                        coordleft += information[i].Width + 20;
                        button[i].Left = coordleft;
                        button[i].BackColor = Color.Red;
                        coordleft += button[i].Width + 20;
                        panel1.Width = coordleft;

                        button[i].Name = "btn"+i;
                        button[i].Click += ButtonOnClick;
                        button[i].Text = "Оформить";
                        name[i].TextAlign = ContentAlignment.TopCenter;
                        valyta[i].TextAlign = ContentAlignment.TopCenter;
                        procent[i].TextAlign = ContentAlignment.TopCenter;
                        srok[i].TextAlign = ContentAlignment.TopCenter;
                        summa[i].TextAlign = ContentAlignment.TopCenter;
                        information[i].TextAlign = ContentAlignment.TopCenter;
                        picturebox[i].BackgroundImageLayout = ImageLayout.Center;

                        panel1.Controls.Add(picturebox[i]);
                        panel1.Controls.Add(name[i]);
                        panel1.Controls.Add(valyta[i]);
                        panel1.Controls.Add(procent[i]);
                        panel1.Controls.Add(srok[i]);
                        panel1.Controls.Add(summa[i]);
                        panel1.Controls.Add(information[i]);
                        panel1.Controls.Add(button[i]);
                        coord += picturebox[i].Height + 20;
                        coordtop += picturebox[i].Height + 20;

                        reader.Read();
                        name[i].Text = reader[2].ToString();
                        if (reader[1].ToString() == "1") valyta[i].Text = "Белорусский рубль";
                        if (reader[1].ToString() == "2") valyta[i].Text = "Российский рубль";
                        if (reader[1].ToString() == "3") valyta[i].Text = "Евро";
                        if (reader[1].ToString() == "4") valyta[i].Text = "Доллар США";
                        procent[i].Text = reader[4].ToString() + " %";
                        srok[i].Text = reader[3].ToString() + " дней";
                        summa[i].Text = "Минимальная сумма депозита: " + reader[5].ToString() + "\r\n" + "Максимальная сумма депозита: " + reader[6].ToString();
                        information[i].Text = reader[9].ToString();
                        picturebox[i].BackgroundImage = Image.FromFile(reader[10].ToString());
                    }
                    reader.Close();
                    conn.Close();

                    name[0].Width = name[1].Width;
                    valyta[0].Width = valyta[1].Width;
                    procent[0].Width = procent[1].Width;
                    srok[0].Width = srok[1].Width;
                    summa[0].Width = summa[1].Width;
                    information[0].Width = information[1].Width;

                    name[0].Text = "Название";
                    valyta[0].Text = "Валюта";
                    procent[0].Text = "Процентная ставка";
                    srok[0].Text = "Срок";
                    summa[0].Text = "Минимальная-максимальная сумма";
                    information[0].Text = "Дополнительная информация";

                    name[0].TextAlign = ContentAlignment.MiddleCenter;
                    valyta[0].TextAlign = ContentAlignment.MiddleCenter;
                    procent[0].TextAlign = ContentAlignment.MiddleCenter;
                    srok[0].TextAlign = ContentAlignment.MiddleCenter;
                    summa[0].TextAlign = ContentAlignment.MiddleCenter;
                    information[0].TextAlign = ContentAlignment.MiddleCenter;

                    panel1.Height = (picturebox[1].Height + 20) * col_deposit + 50;
                    this.Width = panel1.Width + 100;

                    void ButtonOnClick(object sender1, EventArgs eventArgs)
                    {
                        try
                        {
                            var a = (Button)sender1;
                            if (a != null)
                            {
                                int b = Convert.ToInt32(a.Name.Replace("btn", ""));

                                string str = "SELECT id_deposit FROM deposit WHERE name_deposit='" + name[b].Text+"';";
                                MySqlConnection conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                MySqlCommand command1 = new MySqlCommand(str, conn1);
                                MySqlDataReader reader1 = command1.ExecuteReader();
                                reader1.Read();
                                int id = Convert.ToInt32(reader1[0]);
                                bool warning = false;
                                for (int i = 1; i <= col_deposit; i++)
                                {
                                    if (mas_id_deposit[i] == id)
                                    {
                                        MessageBox.Show("Данный тип депозита уже открыт!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        warning = true;
                                    }
                                }
                                if (warning == false)
                                {
                                    depositopen form = new depositopen(mas_id_card, mas_id_deposit, col_cards, col_deposit, id_client, RUB, EUR, USD);
                                    form.Show();
                                    form.comboBox1.SelectedItem = name[b].Text;
                                }
                                reader1.Close();
                                conn1.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    label10.Visible = true;
                    label10.Top = button1.Top + button1.Height + 70;
                    this.Width = 1220;
                    panel1.Width = 1000;
                    panel1.Height = 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            valyta=comboBox1.SelectedIndex+1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = comboBox2.SelectedItem.ToString();
            string[] b = new string[3];
            b=a.Split(' ');
            srok1 = b[0];
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = comboBox5.SelectedItem.ToString();
            string[] b = new string[3];
            b = a.Split(' ');
            srok2 = b[0];
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           procent1 = comboBox3.SelectedItem.ToString();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            procent2 = comboBox4.SelectedItem.ToString();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM deposit WHERE id_deposit>0";
            string sql1 = "SELECT * FROM deposit WHERE id_deposit>0";
            vivoddeposit(sql, sql1);
        }

        private void label11_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void label11_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
    }
}
