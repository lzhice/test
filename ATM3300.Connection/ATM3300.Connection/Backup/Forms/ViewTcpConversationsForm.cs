using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading;

namespace ATM3300.Connection.Forms
{
    public partial class ViewTcpConversationsForm : Form
    {
        private TcpServer _Server;
        
        [Obsolete()]
        public ViewTcpConversationsForm()
        {
            InitializeComponent();
        }

        public ViewTcpConversationsForm(TcpServer server)
        {
            
            
            InitializeComponent();
            _Server = server;
            
            
        }

        void _Server_ConversationRemoved(object sender, TcpConversation conversation)
        {
            Invoke(new ThreadStart(delegate()
            {
                for (int i = 0; i < lstvwConversations.Items.Count; i++)
                {
                    if (lstvwConversations.Items[i].Tag == conversation)
                    {
                        lstvwConversations.Items.RemoveAt(i);
                        return;
                    }
                }
            }));
        }

        void _Server_ConversationAdded(object sender, TcpConversation conversation)
        {
            AddNewConversationToUI(conversation);
        }

        private void AddNewConversationToUI(TcpConversation conversation)
        {
            Invoke(new ThreadStart(delegate()
            {
                ListViewItem item = new ListViewItem(
                    new string[]{
                        conversation.RemoteAddress.Address.ToString(),
                        conversation.RemoteAddress.Port.ToString(),
                        conversation.CreateTime.ToString(),
                        conversation.LastActivityTime.ToString()});

                item.Tag = conversation;

                lstvwConversations.Items.Add(item);
            }));
        }

        private void ShowAllConversations()
        {
            ReadOnlyCollection<TcpConversation> conversations = _Server.GetAllConversations();

            lstvwConversations.Items.Clear();
            for (int i = 0; i < conversations.Count; i++)
            {
                AddNewConversationToUI(conversations[i]);
            }
        }

        private void paneCaption1_Load(object sender, EventArgs e)
        {
            
        }

        private void ViewTcpConversationsForm_Load(object sender, EventArgs e)
        {
            ShowAllConversations();
            _Server.ConversationAdded += new TcpServer.ConversationChangedEventHandler(_Server_ConversationAdded);
            _Server.ConversationRemoved += new TcpServer.ConversationChangedEventHandler(_Server_ConversationRemoved);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lstvwConversations.SelectedItems.Count > 0)
            {
                (lstvwConversations.SelectedItems[0].Tag as TcpConversation).CloseSession();
            }
        }

        private void ViewTcpConversationsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Server.ConversationAdded -= new TcpServer.ConversationChangedEventHandler(_Server_ConversationAdded);
            _Server.ConversationRemoved -= new TcpServer.ConversationChangedEventHandler(_Server_ConversationRemoved);
        }
    }
}