using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace git4win
{
    public partial class FormRemoteEdit : Form
    {
        public FormRemoteEdit(ClassRepo repo)
        {
            InitializeComponent();

            userControlRemoteEdit.setRepo(repo);
        }
    }
}
