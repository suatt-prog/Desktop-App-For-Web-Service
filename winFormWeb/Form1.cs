using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winFormWeb
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        public void clientSetting()
        {
            try
            {
                client.BaseAddress = new Uri("https://localhost:44315/WebService1.asmx");
            }
            catch(Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "sqlConnection" + h.Message + Environment.NewLine;
            }
            /*
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
        }
        string clicked_id;
        int searchtype = 0;
        public Form1()
        {
            InitializeComponent();
        }
        firstwebService.WebService1 proxy = new firstwebService.WebService1();
        public WebRequest executeWebMethod(string method,string parameter)
        {
            WebRequest req = WebRequest.Create("https://localhost:44315/WebService1.asmx/"+method+parameter);
            req.Method = "GET";
            return req;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            string contriesJSON = proxy.countries();
            DataTable veri=JsonConvert.DeserializeObject<DataTable>(contriesJSON);
            dataGridView1.DataSource = veri;
            */
            clientSetting();
        }
        public string transform(string trans)
        {
            string[] dizi = trans.Split('>');
            string[] dizi1 = dizi[2].Split('<');
            return dizi1[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            searchtype = 1;
            clicked_id = textBox1.Text;
            try
            {
                WebRequest req = executeWebMethod("select", "?id=" + textBox1.Text);
                HttpWebResponse respo = (HttpWebResponse)req.GetResponse();
                string weu = null;
                using (Stream stream = respo.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    weu = reader.ReadToEnd();
                    reader.Close();
                }
                DataTable final1 = JsonConvert.DeserializeObject<DataTable>(transform(weu));
                dataGridView1.DataSource = final1;
            }
            catch (Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button1_click" + h.Message + Environment.NewLine;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest req = executeWebMethod("insert", "?ulke=" + textBox2.Text + "&kita=" + textBox3.Text+"&baskent="+comboBox1.Items[comboBox1.SelectedIndex].ToString());
                req.GetResponse();
                button3_Click(sender, e);
            }
            catch (Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click" + h.Message + Environment.NewLine;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                searchtype = 2;
                WebRequest req = executeWebMethod("search", "?ulke=" + textBox2.Text + "&kita=" + textBox3.Text);
                HttpWebResponse respo = (HttpWebResponse)req.GetResponse();
                string weu = null;
                using (Stream stream = respo.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    weu = reader.ReadToEnd();
                    reader.Close();
                }
                DataTable final1 = JsonConvert.DeserializeObject<DataTable>(transform(weu));
                dataGridView1.DataSource = final1;
            }
            catch (Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button3_click" + h.Message + Environment.NewLine;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clicked_id= dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest req = executeWebMethod("delete", "?id=" + clicked_id);
                req.GetResponse();
                if (searchtype == 1)
                {
                    button1_Click(sender, e);
                }
                if (searchtype == 2)
                {
                    button3_Click(sender, e);
                }
            }
            catch (Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button4_click" + h.Message + Environment.NewLine;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest req = executeWebMethod("update", "?ulke=" + textBox2.Text + "&kita=" + textBox3.Text + "&id=" + clicked_id+"&baskent=" + comboBox1.Items[comboBox1.SelectedIndex].ToString());
                req.GetResponse();
                if (searchtype == 1)
                {
                    button1_Click(sender, e);
                }
                if (searchtype == 2)
                {
                    button3_Click(sender, e);
                }
            }
            catch(Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button5_click" + h.Message + Environment.NewLine;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            try
            {
                WebRequest req = executeWebMethod("baskent", "?kita=" + textBox3.Text);
                HttpWebResponse respo = (HttpWebResponse)req.GetResponse();
                string weu = null;
                using (Stream stream = respo.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    weu = reader.ReadToEnd();
                    reader.Close();
                }
                DataTable final1 = JsonConvert.DeserializeObject<DataTable>(transform(weu));
                for (int y = 0; y < final1.Rows.Count; y++)
                {
                    comboBox1.Items.Insert(y, final1.Rows[y][0].ToString());
                }
            }
            catch(Exception h)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "textbox3_TextChanged" + h.Message + Environment.NewLine;
            }
        }
    }
}
