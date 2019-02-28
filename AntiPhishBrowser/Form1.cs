using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiPhishBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            Navigate(txtUrl.Text);
        }

        private void btnGo_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(btnGo, "Go");
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (txtUrl.Text == "")
            {
                lblurl.Visible = true;
            }
            else
            {
                lblurl.Visible = false;
            }
            
        }

        void Navigate(string url)
        {
            antiPhisEngine myEngine = new antiPhisEngine(txtUrl.Text);
            myEngine.startAntiPhishEngine();
            webBrowser.Navigate(myEngine.URL);
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            picNavigating.Visible = true;
            statlabelConnection.Visible = true;
            statlabelConnection.Text = webBrowser.StatusText;
        }

        private void txtUrl_Enter(object sender, EventArgs e)
        {
            if (txtUrl.Text == "")
            {
                lblurl.Visible = true;
            }
            else
            {
                lblurl.Visible = false;
            }       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUrl.Clear();
            lblurl.Visible = true;
            //picNavigating.Visible = false;
           // statlabelConnection.Visible = false;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            statlabelConnection.Text = e.Url.ToString() + " loaded";
            picNavigating.Visible = false;
        }
    }
}
