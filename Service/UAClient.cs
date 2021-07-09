using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;

namespace ClientAppGiaBuild.Service
{
    public class UAClient
    {
        #region Constructor

        public UAClient(ApplicationConfiguration configuration, Action<IList, IList> validateResponse)
        {
            _validateResponse = validateResponse;
            _configuration = configuration;
            _configuration.CertificateValidator.CertificateValidation += CertificateValidation;
        }

        #endregion

        #region Properties

        public Session Session => _session;
        public string ServerUrl { get; set; }

        public Form1 Parent = null;

        private Session _session;
        private ApplicationConfiguration _configuration;
        private Action<IList, IList> _validateResponse;

        #endregion

        #region Methods

        public async Task<Tuple<bool, string>> ConnectAsync()
        {
            try
            {
                string result;

                if (_session != null && _session.Connected == true)
                {
                    result = $"Session already connected!";
                }
                else
                {
                    EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(ServerUrl, false);

                    EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(_configuration);
                    ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

                    Session session = await Session.Create(
                        _configuration,
                        endpoint,
                        false,
                        false,
                        _configuration.ApplicationName,
                        30 * 60 * 1000,
                        new UserIdentity(),
                        null
                    );

                    if (session != null && session.Connected == true)
                    {
                        _session = session;
                    }

                    result = $"Session Connected";
                }

                return Tuple.Create(true, result);
            }
            catch (Exception ex)
            {
                string error = $"Create Session Error : {ex.Message}";
                return Tuple.Create(false, error);
            }
        }

        public Tuple<bool, string> Disconnect()
        {
            try
            {
                if (_session != null)
                {
                    _session.Close();
                    _session.Dispose();
                    _session = null;

                    return Tuple.Create(true, "Session Disconnected.");
                }
                else
                {
                    return Tuple.Create(false, "Session not created!");
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, $"Disconnect Error : {ex.Message}");
            }
        }

        public void WriteBoolValue(string nodeId, bool value)
        {
            if (_session == null || _session.Connected == false)
                return;

            try
            {
                WriteValueCollection nodesToWrite = new WriteValueCollection(); // Write the configured nodes

                WriteValue Item = new WriteValue();
                Item.NodeId = new NodeId(nodeId);
                Item.AttributeId = Attributes.Value;
                Item.Value = new DataValue();
                Item.Value.Value = value;
                nodesToWrite.Add(Item);

                StatusCodeCollection results = null;    // Write the node attributes
                DiagnosticInfoCollection diagnosticInfos;

                _session.Write(null,
                                nodesToWrite,
                                out results,
                                out diagnosticInfos);   // Call Write Service

                ClientBase.ValidateResponse(results, nodesToWrite); // Validate the response
            }
            catch { }
        }
        public void WriteInt32Value(string nodeId, Int32 value)
        {
            if (_session == null || _session.Connected == false)
                return;

            try
            {
                WriteValueCollection nodesToWrite = new WriteValueCollection();

                WriteValue Item = new WriteValue();
                Item.NodeId = new NodeId(nodeId);
                Item.AttributeId = Attributes.Value;
                Item.Value = new DataValue();
                Item.Value.Value = (Int32)value;
                nodesToWrite.Add(Item);

                StatusCodeCollection results = null;
                DiagnosticInfoCollection diagnosticInfos;

                _session.Write(null,
                                nodesToWrite,
                                out results,
                                out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToWrite);
            }
            catch { }
        }
        public void WriteInt16Value(string nodeId, Int16 value)
        {
            if (_session == null || _session.Connected == false)
                return;

            try
            {
                WriteValueCollection nodesToWrite = new WriteValueCollection();

                WriteValue Item = new WriteValue();
                Item.NodeId = new NodeId(nodeId);
                Item.AttributeId = Attributes.Value;
                Item.Value = new DataValue();
                Item.Value.Value = (Int16)value;
                nodesToWrite.Add(Item);

                StatusCodeCollection results = null;
                DiagnosticInfoCollection diagnosticInfos;

                _session.Write(null,
                                nodesToWrite,
                                out results,
                                out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToWrite);
            }
            catch { }
        }
        public void WriteFloatValue(string nodeId, float value)
        {
            if (_session == null || _session.Connected == false)
                return;

            try
            {
                WriteValueCollection nodesToWrite = new WriteValueCollection();

                WriteValue Item = new WriteValue();
                Item.NodeId = new NodeId(nodeId);
                Item.AttributeId = Attributes.Value;
                Item.Value = new DataValue();
                Item.Value.Value = (float)value;
                nodesToWrite.Add(Item);

                StatusCodeCollection results = null;    
                DiagnosticInfoCollection diagnosticInfos;

                _session.Write(null,
                                nodesToWrite,
                                out results,
                                out diagnosticInfos);   

                ClientBase.ValidateResponse(results, nodesToWrite); 
            }
            catch { }
        }
      
        public void SubscribeToDataChanges()
        {
            if (_session == null || _session.Connected == false)
            {
                return;
            }

            try
            {
                Subscription subscription = new Subscription(_session.DefaultSubscription);

                subscription.DisplayName = "Console ReferenceClient Subscription";
                subscription.PublishingEnabled = true;
                subscription.PublishingInterval = 200;

                _session.AddSubscription(subscription);

                subscription.Create();

                //additem

                SubscriptionAddItem(subscription, "Q1", "ns=3;s=\"OPCUA_DB\".\"Q1\"", OnMonitoredQ1Notification);
                SubscriptionAddItem(subscription, "Level2", "ns=3;s=\"OPCUA_DB\".\"Level2\"", OnMonitoredLevelNotification);
                SubscriptionAddItem(subscription, "Out_TON", "ns=3;s=\"OPCUA_DB\".\"Out_TON\"", OnMonitoredOut_TONNotification);
                SubscriptionAddItem(subscription, "ET_TON", "ns=3;s=\"OPCUA_DB\".\"ET_TON\"", OnMonitoredET_TONNotification);
                SubscriptionAddItem(subscription, "Q_CTU", "ns=3;s=\"OPCUA_DB\".\"Q_CTU\"", OnMonitoredQ_CTUNotification);
                SubscriptionAddItem(subscription, "CV_CTU", "ns=3;s=\"OPCUA_DB\".\"CV_CTU\"", OnMonitoredCV_CTUNotification);
                SubscriptionAddItem(subscription, "DXA", "ns=3;s=\"DGT\".\"DXA\"", OnMonitoredDenXANotification);
                SubscriptionAddItem(subscription, "DVA", "ns=3;s=\"DGT\".\"DVA\"", OnMonitoredDenVANotification);
                SubscriptionAddItem(subscription, "DDA", "ns=3;s=\"DGT\".\"DDA\"", OnMonitoredDenDANotification);
                SubscriptionAddItem(subscription, "DXB", "ns=3;s=\"DGT\".\"DXB\"", OnMonitoredDenXBNotification);
                SubscriptionAddItem(subscription, "DVB", "ns=3;s=\"DGT\".\"DVB\"", OnMonitoredDenVBNotification);
                SubscriptionAddItem(subscription, "DDB", "ns=3;s=\"DGT\".\"DDB\"", OnMonitoredDenDBNotification);
                SubscriptionAddItem(subscription, "TTA", "ns=3;s=\"DGT\".\"TTA\"", OnMonitoredTTANotification);
                SubscriptionAddItem(subscription, "TTB", "ns=3;s=\"DGT\".\"TTB\"", OnMonitoredTTBNotification);
                SubscriptionAddItem(subscription, "yk", "ns=3;s=\"GS_DB\".\"yk\"", OnMonitoredYKNotification);
                SubscriptionAddItem(subscription, "uk", "ns=3;s=\"GS_DB\".\"uk\"", OnMonitoredUKNotification);
                SubscriptionAddItem(subscription, "ref", "ns=3;s=\"OnOffController_DB\".\"ref\"", OnMonitoredRefNotification);
                //
                subscription.ApplyChanges();
            }
            catch { }
        }

        private void SubscriptionAddItem(Subscription subscription, string displayName, string nodeId, MonitoredItemNotificationEventHandler eventHandler)
        {
            MonitoredItem item = new MonitoredItem(subscription.DefaultItem);
            item.StartNodeId = new NodeId(nodeId);
            item.AttributeId = Attributes.Value;
            item.DisplayName = displayName;
            item.SamplingInterval = 100;
            item.Notification += eventHandler;
            subscription.AddItem(item);
        }

        #region Notification Event Function
        private void OnMonitoredLevelNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event level
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                Parent.txtLevel2.Text = Convert.ToSingle(notification.Value.Value).ToString();             
            }
            catch { }
        }

        private void OnMonitoredQ1Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event Q1
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirQ1.BackColor = Color.Lime;
                else
                    Parent.cirQ1.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredOut_TONNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event Out_TON
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirTONOUT.BackColor = Color.Lime;
                else
                    Parent.cirTONOUT.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredET_TONNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event ET_TON
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                Parent.txtTONET.Text = Convert.ToInt64(notification.Value.Value).ToString();
            }
            catch { }
        }
        private void OnMonitoredQ_CTUNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event Q_CTU
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirCTUQ.BackColor = Color.Lime;
                else
                    Parent.cirCTUQ.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredCV_CTUNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event CV_CTU
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                Parent.txtCTUCV.Text = Convert.ToInt64(notification.Value.Value).ToString();
            }
            catch { }
        }
        private void OnMonitoredDenXANotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DXA
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirXA.BackColor = Color.Lime;
                else
                    Parent.cirXA.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredDenVANotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DVA
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirVA.BackColor = Color.Yellow;
                else
                    Parent.cirVA.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredDenDANotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DDA
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirDA.BackColor = Color.Red;
                else
                    Parent.cirDA.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredDenXBNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DXB
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirXB.BackColor = Color.Lime;
                else
                    Parent.cirXB.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredDenVBNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DVB
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirVB.BackColor = Color.Yellow;
                else
                    Parent.cirVB.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredDenDBNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event DDB
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (Convert.ToBoolean(notification.Value.Value))
                    Parent.cirDB.BackColor = Color.Red;
                else
                    Parent.cirDB.BackColor = Color.DarkGray;
            }
            catch { }
        }
        private void OnMonitoredTTANotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event TTA
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;

                int TTA = Convert.ToInt32(notification.Value.Value)/1000;
                int chucA = Convert.ToInt32(TTA / 10);
                int donviA = TTA % 10;
                Color color;

                if (Parent.cirXA.BackColor == Color.Lime)
                    color = Color.Lime;
                else if (Parent.cirVA.BackColor == Color.Yellow)
                    color = Color.Yellow;
                else
                    color = Color.Red;

                Parent.led7SegChucA.SetData(chucA, color);
                Parent.led7SegDvA.SetData(donviA, color);
            }
            catch { }
        }
        private void OnMonitoredTTBNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event TTB
        {
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;

            int TTB = Convert.ToInt32(notification.Value.Value)/1000;
            int chucB = Convert.ToInt32(TTB / 10);
            int donviB = TTB % 10;
            Color color;

            if (Parent.cirXB.BackColor == Color.Lime)
                color = Color.Lime;
            else if (Parent.cirVB.BackColor == Color.Yellow)
                color = Color.Yellow;
            else
                color = Color.Red;

            Parent.led7SegChucB.SetData(chucB, color);
            Parent.led7SegDvB.SetData(donviB, color);
        }
        private void OnMonitoredYKNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event yk
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                float temp = Convert.ToSingle(notification.Value.Value);

                Parent.txtyk.Text = temp.ToString();
                Parent.Gragh.UpdateGraph(Parent.zedGraphControl1, 0, Parent.Tick, temp, GraphMode.AutoScale);
                Parent.Gragh.UpdateGraph(Parent.zedGraphControl1, 1, Parent.Tick, Parent.reference, GraphMode.AutoScale);
                Parent.Tick += 0.2;
            }
            catch { }
        }
        private void OnMonitoredUKNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event uk
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                Parent.txtuk.Text = Convert.ToSingle(notification.Value.Value).ToString();
            }
            catch { }
        }
        private void OnMonitoredRefNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) //Event uk
        {
            try
            {
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                float reference;
                Single.TryParse(Parent.txtRef.Text, out reference);
                Parent.reference = reference;
            }
            catch { }
        }
        #endregion
        private void CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            bool certificateAccepted = true;

            ServiceResult error = e.Error;
            while (error != null)
            {
                error = error.InnerResult;
            }

            e.AcceptAll = certificateAccepted;
        }
        #endregion

    }
}
