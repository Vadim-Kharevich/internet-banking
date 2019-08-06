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
    public partial class card : Form
    {
        public card(int[] mas_id_card, int col_cards, string id_client)
        {
            InitializeComponent();
            this.mas_id_card = mas_id_card;
            this.col_cards = col_cards;
            this.id_client = id_client;
        }
        int[] mas_id_card = new int[10]; //id карт 
        int col_cards; //кол-во карт
        string id_client;//id клиента

        string system;
        string vid_card;
        private void button1_Click(object sender, EventArgs e)
        {
           
            string sql = "SELECT COUNT(*) FROM type WHERE id_type>0";
            string sql1 = "SELECT * FROM type WHERE id_type>0";
            if (system != null && system != "0")
            {
                sql += " AND type='" + system + "'";
                sql1 += " AND type='" + system + "'";
            }
            if (vid_card != null && vid_card != "Выбрать все")
            {
                sql += " AND vid='" + vid_card + "'";
                sql1 += " AND vid='" + vid_card + "'";
            }
            if (checkBox1.Checked)
            {
                sql += " AND sms='YES'";
                sql1 += " AND sms='YES'";
            }
            panel1.Controls.Clear();
            vivodcard(sql, sql1);

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            system = comboBox3.SelectedIndex.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            vid_card = comboBox2.SelectedItem.ToString();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM type WHERE id_type>0";
            string sql1 = "SELECT * FROM type WHERE id_type>0";
            vivodcard(sql, sql1);
        }

        private void label11_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void label11_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        void vivodcard(string sql, string sql1)
        {
            try
            {
                panel1.Controls.Clear();
                label10.Visible = false;
                int col_card = 0;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                col_card = Convert.ToInt32(reader[0]);
                reader.Close();
                conn.Close();

                if (col_card != 0)
                {
                    PictureBox[] picturebox = new PictureBox[20];
                    Label[] name = new Label[20];
                    Label[] type = new Label[20];
                    Label[] valyta = new Label[20];
                    Label[] srok = new Label[20];
                    Label[] sms = new Label[20];
                    Label[] vid = new Label[20];
                    Button[] button = new Button[20];

                    int coord = 50;
                    int coordtop = 75;

                    name[0] = new Label();
                    type[0] = new Label();
                    valyta[0] = new Label();
                    srok[0] = new Label();
                    sms[0] = new Label();
                    vid[0] = new Label();
                    panel1.Controls.Add(name[0]);
                    panel1.Controls.Add(type[0]);
                    panel1.Controls.Add(valyta[0]);
                    panel1.Controls.Add(srok[0]);
                    panel1.Controls.Add(sms[0]);
                    panel1.Controls.Add(vid[0]);

                    conn = DBUtils.GetDBConnection();
                    conn.Open();
                    command = new MySqlCommand(sql1, conn);
                    reader = command.ExecuteReader();
                    for (int i = 1; i <= col_card; i++)
                    {
                        int coordleft = 20;
                        picturebox[i] = new PictureBox();
                        name[i] = new Label();
                        type[i] = new Label();
                        valyta[i] = new Label();
                        srok[i] = new Label();
                        sms[i] = new Label();
                        vid[i] = new Label();
                        button[i] = new Button();

                        picturebox[i].Width = 160;
                        name[i].Width = 100;
                        type[i].Width = 100;
                        valyta[i].Width = 150;
                        srok[i].Width = 100;
                        sms[i].Width = 100;
                        vid[i].Width = 100;
                        button[i].Width = 120;

                        picturebox[i].Height = 100;
                        type[i].Height = 80;
                        vid[i].Height = 80;
                        valyta[i].Height = 80;
                        button[i].Height = 40;
                        srok[i].Height = 80;

                        picturebox[i].Top = coord;
                        name[i].Top = coordtop;
                        type[i].Top = coordtop;
                        valyta[i].Top = coordtop;
                        srok[i].Top = coordtop;
                        sms[i].Top = coordtop;
                        vid[i].Top = coordtop;
                        button[i].Top = coordtop;

                        picturebox[i].Left = coordleft;
                        coordleft += picturebox[i].Width + 20;
                        name[0].Left = coordleft;
                        name[i].Left = coordleft;
                        coordleft += name[i].Width + 20;
                        type[0].Left = coordleft;
                        type[i].Left = coordleft;
                        coordleft += type[i].Width + 20;
                        valyta[0].Left = coordleft;
                        valyta[i].Left = coordleft;
                        coordleft += valyta[i].Width + 20;
                        srok[0].Left = coordleft;
                        srok[i].Left = coordleft;
                        coordleft += srok[i].Width + 20;
                        sms[0].Left = coordleft;
                        sms[i].Left = coordleft;
                        coordleft += sms[i].Width + 20;
                        vid[0].Left = coordleft;
                        vid[i].Left = coordleft;
                        coordleft += vid[i].Width + 20;
                        button[i].Left = coordleft;
                        button[i].BackColor = Color.Red;
                        coordleft += button[i].Width + 20;
                        panel1.Width = coordleft;

                        button[i].FlatStyle = 0;
                        button[i].FlatAppearance.BorderColor = Color.Black;
                        button[i].Font=new Font("Tahoma",10,FontStyle.Bold);
                        button[i].ForeColor = Color.White;
                        button[i].Name = "btn" + i;
                        button[i].Click += ButtonOnClick;
                        button[i].Text = "Оформить";
                        name[i].TextAlign = ContentAlignment.TopCenter;
                        type[i].TextAlign = ContentAlignment.TopCenter;
                        valyta[i].TextAlign = ContentAlignment.TopCenter;
                        srok[i].TextAlign = ContentAlignment.TopCenter;
                        sms[i].TextAlign = ContentAlignment.TopCenter;
                        vid[i].TextAlign = ContentAlignment.TopCenter;
                        picturebox[i].BackgroundImageLayout = ImageLayout.Center;

                        panel1.Controls.Add(picturebox[i]);
                        panel1.Controls.Add(name[i]);
                        panel1.Controls.Add(type[i]);
                        panel1.Controls.Add(valyta[i]);
                        panel1.Controls.Add(srok[i]);
                        panel1.Controls.Add(sms[i]);
                        panel1.Controls.Add(vid[i]);
                        panel1.Controls.Add(button[i]);
                        coord += picturebox[i].Height + 20;
                        coordtop += picturebox[i].Height + 20;

                        reader.Read();
                        if (reader[1].ToString() == "1") name[i].Text = "Visa";
                        if (reader[1].ToString() == "2") name[i].Text = "MasterCard";
                        if (reader[1].ToString() == "3") name[i].Text = "Maestro";
                        type[i].Text = reader[2].ToString();
                        valyta[i].Text = "Белорусский рубль\r\nРоссийский рубль\r\nЕвро\r\nДоллар США";
                        srok[i].Text = "3-5 лет";
                        if (reader[5].ToString() == "YES")
                            sms[i].Text = "+";
                        else
                            sms[i].Text = "-";
                        vid[i].Text = reader[4].ToString();
                        picturebox[i].BackgroundImage = Image.FromFile(reader[3].ToString());
                    }
                    reader.Close();
                    conn.Close();

                    name[0].Width = name[1].Width;
                    name[0].Height = 30;
                    type[0].Width = type[1].Width;
                    valyta[0].Width = valyta[1].Width;
                    srok[0].Width = srok[1].Width;
                    sms[0].Width = sms[1].Width;
                    vid[0].Width = vid[1].Width;

                    name[0].Text = "Платежная\r\nсистема";
                    type[0].Text = "Тип";
                    valyta[0].Text = "Валюта";
                    srok[0].Text = "Срок действия";
                    sms[0].Text = "SMS-оповещение";
                    vid[0].Text = "Класс";

                    name[0].Top = 10;
                    type[0].Top = 10;
                    valyta[0].Top = 10;
                    srok[0].Top = 10;
                    sms[0].Top = 10;
                    vid[0].Top = 10;

                    name[0].TextAlign = ContentAlignment.TopCenter;
                    type[0].TextAlign = ContentAlignment.TopCenter;
                    valyta[0].TextAlign = ContentAlignment.TopCenter;
                    srok[0].TextAlign = ContentAlignment.TopCenter;
                    sms[0].TextAlign = ContentAlignment.TopCenter;
                    vid[0].TextAlign = ContentAlignment.TopCenter;

                    panel1.Height = (picturebox[1].Height + 20) * col_card + 50;
                    this.Width = panel1.Width + 100;

                    void ButtonOnClick(object sender1, EventArgs eventArgs)
                    {
                        try
                        {
                            var a = (Button)sender1;
                            if (a != null)
                            {
                                int b = Convert.ToInt32(a.Name.Replace("btn", ""));
                                string type_card = "";
                                if (name[b].Text == "Visa") type_card = "1";
                                if (name[b].Text == "MasterCard") type_card = "2";
                                if (name[b].Text == "Maestro") type_card = "3";

                                string str = "SELECT id_type FROM type WHERE name='" + type[b].Text + "' AND type='"+type_card+"'";
                                MySqlConnection conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                MySqlCommand command1 = new MySqlCommand(str, conn1);
                                MySqlDataReader reader1 = command1.ExecuteReader();
                                reader1.Read();
                                int id_open = Convert.ToInt32(reader1[0]);
                                reader1.Close();
                                conn1.Close();

                                bool warning = false;
                                str = "SELECT id_card FROM bank_card WHERE id_client='" + id_client + "' AND id_type='" + id_open + "'";
                                conn1 = DBUtils.GetDBConnection();
                                conn1.Open();
                                command1 = new MySqlCommand(str, conn1);
                                reader1 = command1.ExecuteReader();
                                if (reader1.Read())
                                {
                                    MessageBox.Show("У вас уже имеется карта данного типа!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    warning = true;
                                }
                                if (warning == false)
                                {
                                    cardopen form = new cardopen(mas_id_card,  col_cards,  id_client);
                                    form.Show();
                                    form.comboBox1.SelectedItem = name[b].Text + " " + type[b].Text;
                                }
                               
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

        private void card_Load(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM type WHERE id_type>0";
            string sql1 = "SELECT * FROM type WHERE id_type>0";
            vivodcard(sql, sql1);
        }
    }
}
