using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientDekstopApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TcpClient? client = null;
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var ipAddress = IPAddress.Parse("127.0.0.1");
                var port = 45001;
                client = new TcpClient();
                client.Connect(ipAddress, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (client!.Connected)
                {
                    var stream = client.GetStream();
                    var br = new BinaryReader(stream);
                    var bw = new BinaryWriter(stream);


                    if (!string.IsNullOrWhiteSpace(txbUsername.Text))
                    {
                        bw.Write(txbUsername.Text);

                        var responseFromServer = br.ReadString();
                        MessageBox.Show(responseFromServer);
                    }
                }
                else
                {
                    MessageBox.Show("Client connect olunmayib!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
