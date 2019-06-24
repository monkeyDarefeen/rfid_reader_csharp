using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NFCSharp;
using System.Diagnostics;


namespace WpfApp2
{

    public partial class MainWindow : Window
    {
        public NFCTag theTag = null;
        public string tagid { get; private set; }
        public string tagid_1 { get; private set; }
        public MainWindow()
        {
            InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            getid.IsEnabled = false;
            NFCHandler.Init();
            dReaders.Items.Clear();
            foreach (NFCReader rdr in NFCHandler.Readers)
            {
                dReaders.Items.Add(rdr.Name);
            }
            if (dReaders.Items.Count > 0)
                dReaders.SelectedIndex = 0;

            if (!NFCHandler.IsInitialized)
            {
                MessageBox.Show("card not installed");
                getid.IsEnabled = true;
            }
            else if (dReaders.Items.Count == 0) { MessageBox.Show("Reader Not Found!!!"); getid.IsEnabled = true; }

            else
            {
                while (true)
                {
                    theTag = NFCHandler.Readers[dReaders.SelectedIndex].Connect();
                    if (theTag != null)
                    {
                        var tagId = (BitConverter.ToUInt32(theTag.bUID, 0) % 10000000);  //this is an example to play with your card number, if you are developing a system, you may need something like this
                        //var tagId = theTag.bUID;   //original tagid
                        //var tagId = (BitConverter.ToUInt32(theTag.bUID, 0); //sometimes some rfid uses this format to show tag number
                        idno.Text = tagId.ToString();
                        refno.Text = theTag.ATR.ToString();

                        await Task.Delay(2000);
                        NFCHandler.Release();
                        getid.IsEnabled = true;
                        break;

                    }
                }
            }
        }

        //do your work here. happy coding



    }
}