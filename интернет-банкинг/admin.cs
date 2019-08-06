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
    public partial class admin : Form
    {
        public admin(string FIO)
        {
            InitializeComponent();
            this.FIO = FIO;
            label1.Text = "Здравствуйте, "+FIO;
        }
        string FIO;
        string tablitsa;
        bool click = false;

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            string sqlinsert = "INSERT INTO "+tablitsa+"(";
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                sqlinsert += dataGridView1.Columns[i].HeaderText;
                if (i < dataGridView1.ColumnCount - 1)
                {
                    sqlinsert += ",";
                }
                else
                {
                    sqlinsert += ") VALUES (";
                }
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                sqlinsert += "'" + dataGridView1.Rows[0].Cells[i].Value.ToString() + "'";
                if (i < dataGridView1.ColumnCount - 1)
                {
                    sqlinsert += ",";
                }
                else
                {
                    sqlinsert += ");";
                }
            }
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sqlinsert, conn);
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Добавлена новая запись!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Rows[0].Cells[i].Value = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "admin";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: "+ tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
            
        }

        private void bankcardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "bank_card";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "category";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "client";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void creditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "credit";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void depositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "deposit";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "history";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void oblastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "oblast";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void plategiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "plategi";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "type";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void valytaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "valyta";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void vivod(bool flag,string delete)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tablitsa + "';";//получение кол-ва столбцов таблицы
                string sql1 = "DESCRIBE " + tablitsa + ";"; //получение списка имен столбцов
                string sql2 = "SELECT COUNT(*) FROM " + tablitsa + ";"; //получение кол-ва строк таблицы
                
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int colstolbec = Convert.ToInt32(reader[0].ToString());
                reader.Close();
                conn.Close();

                conn.Open();
                command = new MySqlCommand(sql2, conn);
                reader = command.ExecuteReader();
                reader.Read();
                int colstrok = Convert.ToInt32(reader[0].ToString());
                reader.Close();
                conn.Close();

                conn.Open();
                string[] namestolbec = new string[colstolbec];
                command = new MySqlCommand(sql1, conn);
                reader = command.ExecuteReader();
                int k = 0;
                while (reader.Read())
                {
                    namestolbec[k] = reader[0].ToString();
                    k++;
                }
                reader.Close();
                conn.Close();
                dataGridView1.RowCount = colstrok;
                dataGridView1.ColumnCount = colstolbec;
                for (int i = 0; i < colstolbec; i++)
                {
                    dataGridView1.Columns[i].HeaderText = namestolbec[i];
                }

                if(flag)
                {
                    for(int i=Convert.ToInt32(delete);i<=colstrok;i++)
                    {
                        string sqlupdate = "UPDATE " + tablitsa + " SET " + dataGridView1.Columns[0].HeaderText + "=" + i + " WHERE " + dataGridView1.Columns[0].HeaderText + "=" + (i + 1);
                        conn.Open();
                        command = new MySqlCommand(sqlupdate, conn);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                        conn.Close();
                    }
                    
                }

                string sql3 = "SELECT * FROM " + tablitsa + " ORDER BY "+ dataGridView1.Columns[0].HeaderText + ";"; //получение содержимого таблицы
                conn.Open();
                command = new MySqlCommand(sql3, conn);
                reader = command.ExecuteReader();
                int a = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < colstolbec; i++)
                    {
                        dataGridView1.Rows[a].Cells[i].Value = reader[i].ToString();
                    }
                    a++;
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void изменитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vivod(false,null);
            dataGridView1.ReadOnly = false;
            dataGridView1.Visible = true;
            textBox1.Visible = false;
            label3.Visible = false;
            button3.Visible = false;
            click = true;
            сохранитьДанныеToolStripMenuItem.Visible = true;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DataGridViewElementStates states = DataGridViewElementStates.None;
            dataGridView1.Width = dataGridView1.Columns.GetColumnsWidth(states)+20;
            button2.Visible = true;
            button1.Visible = false;
        }

        private void сохранитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Saving();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Saving()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                string sqlupdate = "UPDATE " + tablitsa + " SET ";
                for (int i = 1; i < dataGridView1.ColumnCount; i++)
                {
                    sqlupdate += dataGridView1.Columns[i].HeaderText + "= '";
                    sqlupdate += dataGridView1.Rows[j].Cells[i].Value.ToString() + "'";
                    if (i < dataGridView1.ColumnCount - 1)
                    {
                        sqlupdate += ",";
                    }
                }
                sqlupdate += " WHERE " + dataGridView1.Columns[0].HeaderText + "=" + dataGridView1.Rows[j].Cells[0].Value.ToString();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sqlupdate, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            MessageBox.Show("Записи успешно сохранены!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            click = false;
        }
        private void Message()
        {
            if (click == true)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Saving();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    click = false;
            }
        }

        private void добавитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            try
            {
                string sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tablitsa + "';";//получение кол-ва столбцов таблицы
                string sql1 = "DESCRIBE " + tablitsa + ";"; //получение списка имен столбцов
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int colstolbec = Convert.ToInt32(reader[0].ToString());
                reader.Close();
                conn.Close();

                conn.Open();
                string[] namestolbec = new string[colstolbec];
                command = new MySqlCommand(sql1, conn);
                reader = command.ExecuteReader();
                int k = 0;
                while (reader.Read())
                {
                    namestolbec[k] = reader[0].ToString();
                    k++;
                }
                reader.Close();
                conn.Close();

                dataGridView1.RowCount = 1;
                dataGridView1.ColumnCount = colstolbec;
                for (int i = 0; i < colstolbec; i++)
                {
                    dataGridView1.Columns[i].HeaderText = namestolbec[i];
                    dataGridView1.Rows[0].Cells[i].Value = "";
                }

                string sql_last_id = "SELECT "+ dataGridView1.Columns[0].HeaderText + " FROM " + tablitsa + " ORDER BY " + dataGridView1.Columns[0].HeaderText + " DESC ;"; //получение содержимого таблицы
                conn.Open();
                command = new MySqlCommand(sql_last_id, conn);
                reader = command.ExecuteReader();
                reader.Read();
                dataGridView1.Rows[0].Cells[0].Value = Convert.ToInt32(reader[0])+1;
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dataGridView1.Rows[0].Cells[0].ReadOnly=true;
            dataGridView1.ReadOnly = false;
            dataGridView1.Visible = true;
            textBox1.Visible = false;
            label3.Visible = false;
            button3.Visible = false;
            button1.Visible = true;
            button2.Visible = false;
            сохранитьДанныеToolStripMenuItem.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Saving();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void удалитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            vivod(false,null);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DataGridViewElementStates states = DataGridViewElementStates.None;
            dataGridView1.Width = dataGridView1.Columns.GetColumnsWidth(states) + 20;
            dataGridView1.Visible = true;
            dataGridView1.ReadOnly = true;
            textBox1.Visible = true;
            label3.Visible = true;
            button3.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            сохранитьДанныеToolStripMenuItem.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите id удаляемой записи!","Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Хотите удалить запись с id="+textBox1.Text+" ?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        string sqldelete = "DELETE FROM " + tablitsa + " WHERE " + dataGridView1.Columns[0].HeaderText + "=" + textBox1.Text;
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(sqldelete, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Запись успешно удалена!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        vivod(true,textBox1.Text);
                        textBox1.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Message();
        }

        private void creditdopolnitelnayaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "credit_dopolnitelnaya";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void depositdopolnitelnayaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "deposit_dopolnitelnaya";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void naznachenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "naznachenie";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void plategidopolnitelnayaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "plategi_dopolnitelnaya";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void podcategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "podcategory";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void podpodcategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "podpodcategory";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }

        private void typecardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Message();
            сохранитьДанныеToolStripMenuItem.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            tablitsa = "type_card";
            label2.Visible = true;
            label2.Text = "Выбранная таблица: " + tablitsa;
            изменитьДанныеToolStripMenuItem.Visible = true;
            добавитьДанныеToolStripMenuItem.Visible = true;
            удалитьДанныеToolStripMenuItem.Visible = true;
        }
    }
}
