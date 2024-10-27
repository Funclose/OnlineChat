using System.IO;
using System.Net.Sockets;
using System.Text;

namespace OnlyneChat
{
    //public class User
    //{ 
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //}
    public partial class Form1 : Form
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _tcpClient = new TcpClient("127.0.0.1", 8888);
            _stream = _tcpClient.GetStream();
        }

        private void bntSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;

            if (!string.IsNullOrEmpty(message))
            {
                // Отправка сообщения на сервер
                byte[] data = Encoding.UTF8.GetBytes(message);
                _stream.Write(data, 0, data.Length);

                // Добавляем сообщение в ListBox
                lstMessages.Items.Add("Вы: " + message);
                txtMessage.Clear();
            }
        }

        private void ReceivMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                try
                {
                    
                    bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        continue;

                    
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Invoke((MethodInvoker)delegate
                    {
                        lstMessages.Items.Add("Сервер: " + message);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    break;
                }
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (_tcpClient != null)
             {
                _stream.Close();
                _tcpClient.Close(); 
             }
        }
    }
}
