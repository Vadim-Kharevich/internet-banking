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
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            if(login!=""&&password!=""&&login!="логин"&&password!="пароль")
            {
                try
                {
                    string sql = "SELECT id_client FROM client WHERE login='" + login + "' AND password='" + password + "';";
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        this.Hide();
                        user form = new user(reader[0].ToString(),this);
                        form.Show();
                        textBox1.Text = "логин";
                        textBox2.Text = "пароль";
                        textBox1.ForeColor = Color.Gray;
                        textBox2.ForeColor = Color.Gray;
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели неверный логин или пароль!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this,"Вы ввели неккоректные данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Перейти в режим администратора?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                groupBox2.Visible = false;
                groupBox3.Visible = true;
            }   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string password = textBox3.Text;
            if (password != ""&&password != "пароль")
            {
                try
                {
                    string sql = "SELECT FIO FROM admin WHERE password='" + password + "';";
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show(reader[0].ToString());
                        admin form = new admin(reader[0].ToString());
                        form.Show();
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели неверный логин или пароль!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Вы ввели неккоректные данные!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            registration form = new registration(this);
            form.Show();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox3.ForeColor = Color.Black;
        }

        private void main_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(173, 158, 128);
            timer1.Enabled = true;
            timer1.Interval = 100;
            panel1.Left = Width / 2 - panel1.Width / 2;
            toolTip1.SetToolTip(button1, "Необходима для входа в режим пользователя");
            toolTip2.SetToolTip(button2, "Необходима для входа в режим администратора");
            toolTip3.SetToolTip(button4, "Необходима для регистрации новых пользователей");
            toolTip4.SetToolTip(button3, "Необходима для предоставления справочной информации");
        }
        int time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            if(textBox1.Text==""&&!textBox1.Focused)
            {
                textBox1.Text = "логин";
                textBox1.ForeColor = Color.Gray;
            }
            if (textBox2.Text == "" && !textBox2.Focused)
            {
                textBox2.Text = "пароль";
                textBox2.ForeColor = Color.Gray;
            }
            if (time == 3)
            {
                training form = new training(this,0,null);
                form.Show();
                form.Activate();
                this.Enabled = false;
            }
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Browser form = new Browser(this);
            form.Show();
        }
    }
    
}
