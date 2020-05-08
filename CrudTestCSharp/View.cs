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
    public partial class View : Form
    {
        #region "Initializing"

        public View()
        {
            InitializeComponent();
        }

        #endregion

        #region "GlobalForm"

        private static View GlobalVar;
        public static View GlobalForm 
        {
            get 
            {
                if (GlobalVar == null || GlobalVar.IsDisposed) 
                {
                    GlobalVar = new View();
                }

                return GlobalVar;
            }
            set { GlobalVar = value; }
        }

        #endregion

        #region "DML"

        private void GetList()
        {
            listView1.Items.Clear();

            OleDbConnection con = new OleDbConnection(GlobalVariables.ConnectionString);
            con.Open();
            string Query = "SELECT * FROM Profile";
            using (OleDbCommand cmd = new OleDbCommand(Query, con))
            {
                OleDbDataReader read = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(read);
                if (dt.Rows.Count > 0)
                {
                    foreach (var r in dt.AsEnumerable())
                    {
                        ListViewItem lst = new ListViewItem(r["ID"].ToString());
                        //MessageBox.Show(r["ID"].GetType().ToString());
                        lst.SubItems.Add(r["FirstName"].ToString());
                        lst.SubItems.Add(r["LastName"].ToString());
                        lst.SubItems.Add(r["Email"].ToString());
                        lst.SubItems.Add(r["PhoneNo"].ToString());

                        listView1.Items.AddRange(new ListViewItem[] { lst });
                    }
                }
            }
            if (con.State == System.Data.ConnectionState.Open) { con.Close(); }
            con.Close();

        }

        private void DeleteRow()
        {
            OleDbConnection con = new OleDbConnection(GlobalVariables.ConnectionString);
            con.Open();
            string Query = "DELETE FROM Profile WHERE ID = @param1";
            using (OleDbCommand cmd = new OleDbCommand(Query, con))
            {
                cmd.Parameters.Add("@param1", OleDbType.Integer).Value = GlobalVariables.GetRow;

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();

            }
            if (con.State == System.Data.ConnectionState.Open) { con.Close(); }
            con.Close();
        }

        #endregion

        #region "Form Event"

        private void View_Load(object sender, EventArgs e)
        {
            GlobalVariables.GetRow = 0;
            GetList();
        }

        #endregion

        #region "Button Event"

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalVariables.PathButton = "New";
            ProfileView.GlobalForm.ShowDialog();
            GetList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.GetRow > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this Record..?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteRow();
                    MessageBox.Show("Deleted..");
                    GetList();
                }

            }
        }

        #endregion

        #region "ListView Event"

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GlobalVariables.GetRow = 0;
            GlobalVariables.GetRow = Convert.ToInt32(listView1.Items[listView1.FocusedItem.Index].SubItems[0].Text);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GlobalVariables.PathButton = "Update";

            GlobalVariables.GetRow = Convert.ToInt32(listView1.Items[listView1.FocusedItem.Index].SubItems[0].Text);
            GlobalVariables.GlobalFname = listView1.Items[listView1.FocusedItem.Index].SubItems[1].Text;
            GlobalVariables.GlobalLname = listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text;
            GlobalVariables.GlobalEmail = listView1.Items[listView1.FocusedItem.Index].SubItems[3].Text;
            GlobalVariables.GlobalPhoneNo = listView1.Items[listView1.FocusedItem.Index].SubItems[4].Text;

            GlobalVariables.GlobalActive = true;

            ProfileView.GlobalForm.ShowDialog();
            GetList();
        }

        #endregion
    }
}
