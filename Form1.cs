using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientAppGiaBuild.Service;
using Opc.Ua;
using Opc.Ua.Configuration;

namespace ClientAppGiaBuild
{
    public partial class Form1 : Form
    {
        private UAClient uaClient { get; set; }

        public double Tick = 0;
        public Graph Gragh = new Graph();
        public float reference;
        ApplicationInstance application = new ApplicationInstance();

        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Gragh.Initailize(zedGraphControl1, 0, 100, 0, 100, "Controller", "Time", "Y");
            Gragh.AddGraph(zedGraphControl1, "Curent Point", Color.Blue);
            Gragh.AddGraph(zedGraphControl1, "Set Point", Color.Red);
            
            application.ApplicationName = "Quickstart Console Reference Client";
            application.ApplicationType = ApplicationType.Client;

            await application.LoadApplicationConfiguration(@"E:\hoc hanh cham chi\hk202\DLDKMT\BTL\ClientAppGiaBuild\ConsoleReferenceClient.Config.xml", silent: false);
            await application.CheckApplicationInstanceCertificate(silent: false, minimumKeySize: 0);
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;        

            uaClient = new UAClient(application.ApplicationConfiguration, ClientBase.ValidateResponse);
            uaClient.ServerUrl = txtUrl.Text;

            Tuple<bool, string> connected = await uaClient.ConnectAsync();          

            if (connected.Item1)
            {
                uaClient.Parent = this;
                uaClient.SubscribeToDataChanges();
                tabControl1.Enabled = true;
                btnDisconnect.Enabled = true;
                MessageBox.Show(connected.Item2, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                btnConnect.Enabled = true;
                MessageBox.Show(connected.Item2, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = false;
            var result = uaClient.Disconnect();

            if (result.Item1)
            {
                tabControl1.Enabled = false;
                btnConnect.Enabled = true;
                MessageBox.Show(result.Item2, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }     
            else
            {
                btnDisconnect.Enabled = true;
                MessageBox.Show(result.Item2, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"I1\"", chkI1.Checked);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            float temp;
            if (Single.TryParse(txtLevel.Text, out temp))
                uaClient.WriteFloatValue("ns=3;s=\"OPCUA_DB\".\"Level\"", temp);
            else
                MessageBox.Show("Data is not number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void chkI2_CheckedChanged(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"I2\"", chkI2.Checked);
        }

        private void chkTONIN_CheckedChanged(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"In_TON\"", chkTONIN.Checked);
        }

        private void btnWriteTONPT_Click(object sender, EventArgs e)
        {
            Int32 temp;
            if (Int32.TryParse(txtTONPT.Text, out temp))
                uaClient.WriteInt32Value("ns=3;s=\"OPCUA_DB\".\"PT_TON\"", temp);
            else
                MessageBox.Show("Data is not number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void chkCTUR_CheckedChanged(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"R_CTU\"", chkCTUR.Checked);
        }

        private void chkCTUCU_CheckedChanged(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"CU_CTU\"", chkCTUCU.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int16 temp;
            if (Int16.TryParse(txtCTUPV.Text, out temp))
                uaClient.WriteInt16Value("ns=3;s=\"OPCUA_DB\".\"PV_CTU\"", temp);
            else
                MessageBox.Show("Data is not number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnWriteRef_Click(object sender, EventArgs e)
        {
            float temp;
            if (float.TryParse(txtRef.Text, out temp))
                uaClient.WriteFloatValue("ns=3;s=\"OnOffController_DB\".\"ref\"", temp);
            else
                MessageBox.Show("Data is not number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"Clear\"", true);
            Thread.Sleep(100);
            uaClient.WriteBoolValue("ns=3;s=\"OPCUA_DB\".\"Clear\"", false);
            txtRef.Text = "0";
            Gragh.ClearGraph(zedGraphControl1);
            Gragh.DrawGraph(zedGraphControl1);
            Gragh.Initailize(zedGraphControl1, 0, 100, 0, 100, "Controller", "Time", "Y");
            Gragh.AddGraph(zedGraphControl1, "Curent Point", Color.Blue);
            Gragh.AddGraph(zedGraphControl1, "Set Point", Color.Red);
            Tick = 0;
        }
    }
}
