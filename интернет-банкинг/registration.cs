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
    public partial class registration : MetroFramework.Forms.MetroForm
    {
        public registration(main main)
        {
            InitializeComponent();
            this.main = main;
        }

        main main;
        private void registration_Load(object sender, EventArgs e)
        {
            ActiveControl=button1;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
        int shag = 1;
        string PIN;
        string id;
        int popytki = 3;
        private void button1_Click(object sender, EventArgs e)
        {
            if(shag==1)
            {
                string number_card = textBox1.Text;
                string FIO = textBox2.Text;
                if (number_card != "" && number_card!="XXXX-XXXX-XXXX-XXXX" && FIO != "" && FIO != "Иванов Иван Иванович")
                {
                    try
                    {
                        string sql = "SELECT id_card,PIN FROM bank_card WHERE number_card='" + number_card + "' AND FIO='" + FIO + "';";
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader[0].ToString();
                            PIN = reader[1].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели неверные данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        reader.Close();
                        conn.Close();

                        if (id != null)
                        {
                            sql = "SELECT id_client FROM bank_card WHERE id_card='" + id + "';";
                            conn = DBUtils.GetDBConnection();
                            conn.Open();
                            command = new MySqlCommand(sql, conn);
                            reader = command.ExecuteReader();
                            reader.Read();
                            if (reader[0].ToString() == "" || reader[0].ToString() == "0")
                            {
                                shag = 2;
                                label1.Text = "Шаг 2: подтверждение карты";
                                label2.Text = "PIN-код (4 цифры)";
                                textBox1.Text = "XXXX";
                                textBox1.Width = 70;
                                textBox2.Visible = false;
                                label3.Visible = false;
                                button1.Location = new Point(135, 265);
                                this.Height = 360;
                            }
                            else
                            {
                                MessageBox.Show("Пользователь с такой картой уже зарегистрирован!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        
                        reader.Close();
                        conn.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            if(shag==2)
            {
                if(textBox1.Text!="" && textBox1.Text != "XXXX")
                {
                    if(PIN==textBox1.Text)
                    {
                        shag = 3;
                        label1.Text = "Шаг 3: выбор логина и пароля";
                        label2.Text = "Логин";
                        label3.Text = "Пароль (минимум 8 символов)";
                        textBox1.Text = "";
                        textBox1.Width = 170;
                        textBox2.Text = "";
                        textBox2.Width = 170;
                        textBox2.Visible = true;
                        label3.Visible = true;
                        button1.Location = new Point(142, 362);
                        button1.Text = "Зарегистрироваться";
                        this.Height = 440;
                    }
                    else
                    {
                        popytki--;
                        if(popytki>0)
                        {
                            MessageBox.Show("Введен неверный PIN-код! \r\n Количество оставшихся попыток" + popytki, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Введен неверный PIN-код 3 раза! \r\n Регистрация прекращена" + popytki, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите PIN-код!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            if(shag==3)
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    try
                    {
                        string sql = "SELECT id_client FROM client WHERE login='" + textBox1.Text + "';";
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            reader.Close();
                            conn.Close();
                        }
                        else
                        {
                            reader.Close();
                            conn.Close();

                            string sql_last_id = "SELECT id_client FROM client ORDER BY id_client DESC ;"; //определение последнего id в таблице client
                            conn.Open();
                            command = new MySqlCommand(sql_last_id, conn);
                            reader = command.ExecuteReader();
                            reader.Read();
                            int last_id = Convert.ToInt32(reader[0]) + 1;
                            reader.Close();
                            conn.Close();

                            sql = "INSERT INTO client(id_client,login,password) VALUES ('" + last_id + "','" + textBox1.Text + "','" + textBox2.Text + "');"; //создание записи в таблице client
                            conn = DBUtils.GetDBConnection();
                            conn.Open();
                            command = new MySqlCommand(sql, conn);
                            reader = command.ExecuteReader();
                            reader.Read();
                            reader.Close();
                            conn.Close();

                            sql = "UPDATE bank_card SET id_client=" + last_id; //создание записи в таблице client
                            conn = DBUtils.GetDBConnection();
                            conn.Open();
                            command = new MySqlCommand(sql, conn);
                            reader = command.ExecuteReader();
                            reader.Read();
                            reader.Close();
                            conn.Close();

                            MessageBox.Show("Вы успешно зарегистрированы!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.Close();
                            main.Show();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } 
                }
                else
                {
                    MessageBox.Show("Введите данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.Show();
        }
    }
}
