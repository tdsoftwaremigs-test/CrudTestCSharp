using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CrudTestCSharp
{
    public partial class ProfileView : Form
    {
        #region "initializing"

        public ProfileView()
        {
            InitializeComponent();
        }

        #endregion

        #region "GlobalForm"

        private static ProfileView GlobalVar;
        public static ProfileView GlobalForm
        {
            get
            {
                if (GlobalVar == null || GlobalVar.IsDisposed)
                {
                    GlobalVar = new ProfileView();
                }

                return GlobalVar;
            }
            set { GlobalVar = value; }
        }

        #endregion

        #region "Functions"

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            checkBox1.Checked = false;
        }

        private bool FieldCheck()
        {
            int count = 0;

            string[] Row = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text };

            foreach (var r in Row)
            {
                if (string.IsNullOrWhiteSpace(r))
                {
                    count = 1;
                }
            }

            if (count > 0) { return true; }
            else { return false; }
        }

        #endregion

        #region "DML"

        private void UpdateProfile()
        {
            OleDbConnection con = new OleDbConnection(GlobalVariables.ConnectionString);
            con.Open();
            string Query = "UPDATE Profile SET FirstName = @param1 AND LastName = @param2 AND Email = @param3 AND PhoneNo = @param4 AND Active = @param5 Where ID = @param6";
            using (OleDbCommand cmd = new OleDbCommand(Query, con))
            {
                cmd.Parameters.AddWithValue("@param1", textBox1.Text);
                cmd.Parameters.AddWithValue("@param2", textBox2.Text);
                cmd.Parameters.AddWithValue("@param3", textBox3.Text);
                cmd.Parameters.AddWithValue("@param4", textBox4.Text);

                bool checkBoxresult = false;
                if (checkBox1.Checked == true) { checkBoxresult = true; }
                cmd.Parameters.AddWithValue("@param5", checkBoxresult);

                cmd.Parameters.AddWithValue("@param6", GlobalVariables.GetRow);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();

            }
            if (con.State == System.Data.ConnectionState.Open) { con.Close(); }
            con.Close();
        }

        private void SaveProfile()
        {
            OleDbConnection con = new OleDbConnection(GlobalVariables.ConnectionString);
            con.Open();
            string Query = "INSERT INTO Profile(FirstName, LastName, Email, PhoneNo, Active) Values(@param1, @param2, @param3, @param4, @param5)";
            using (OleDbCommand cmd = new OleDbCommand(Query, con))
            {
                cmd.Parameters.Add("@param1", OleDbType.VarChar).Value = textBox1.Text;
                cmd.Parameters.Add("@param2", OleDbType.VarChar).Value = textBox2.Text;
                cmd.Parameters.Add("@param3", OleDbType.VarChar).Value = textBox3.Text;
                cmd.Parameters.Add("@param4", OleDbType.VarChar).Value = textBox4.Text;

                bool checkBoxresult = false;
                if (checkBox1.Checked == true) { checkBoxresult = true; }
                cmd.Parameters.Add("@param5", OleDbType.Boolean).Value = checkBoxresult;

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();

            }
            if (con.State == System.Data.ConnectionState.Open) { con.Close(); }
            con.Close();
        }

        #endregion

        #region "Form Event"

        private void ProfileView_Load(object sender, EventArgs e)
        {
            ClearFields();

            if (GlobalVariables.PathButton == "Update")
            {
                textBox1.Text = GlobalVariables.GlobalFname;
                textBox2.Text = GlobalVariables.GlobalLname;
                textBox3.Text = GlobalVariables.GlobalEmail;
                textBox4.Text = GlobalVariables.GlobalPhoneNo;
                checkBox1.Checked = GlobalVariables.GlobalActive;
            }
        }

        #endregion

        #region "Button Event"

        private void button1_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.PathButton == "New")
            {
                if (!FieldCheck()) 
                {
                    SaveProfile();
                    MessageBox.Show("Saved..");
                    ClearFields();
                    this.Close();
                }
            }

            if (GlobalVariables.PathButton == "Update") 
            {
                if (!FieldCheck())
                {
                    UpdateProfile();
                    MessageBox.Show("Updated..");
                    ClearFields();
                    this.Close();
                }                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        
    }
}
