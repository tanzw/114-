using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }
        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _RealName;

        public string RealName
        {
            get { return _RealName; }
            set { _RealName = value; }
        }
        private string _Role;

        public string Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        private bool _IsLogin = false;

        public bool IsLogin
        {
            get { return _IsLogin; }
            set { _IsLogin = value; }
        }
    }
}
