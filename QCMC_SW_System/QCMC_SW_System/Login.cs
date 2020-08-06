using QCMC_SW_System.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace QCMC_SW_System
{
    public partial class Login : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int VM_NCLBUTTONDOWN = 0XA1;//定义鼠标左键按下
        private const int HTCAPTION = 2;
        OperationDatabaseClass operationDatabaseClass = new OperationDatabaseClass();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }
        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            //为当前应用程序释放鼠标捕获
            ReleaseCapture();
            //发送消息 让系统误以为在标题栏上按下鼠标
            SendMessage((IntPtr)this.Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Dispose();
        }
        
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { txtPassWord.Focus(); }
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { Login_User(); }
        }

        private void labelResetPassWord_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Do you Forget your password?");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login_User();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
        }

        private void Login_User()
        {
            string User_ID = txtUserName.Text;
            string PW = txtPassWord.Text;
            int length = operationDatabaseClass.Query("Login", "*", "UserName='" + User_ID + "' and Password='" + PW + "'").Table.Rows.Count;
            if (length == 1)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show("NG");
            }
        }
        
    }
}
