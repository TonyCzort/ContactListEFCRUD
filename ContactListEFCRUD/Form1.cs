using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactListEFCRUD
{
    public partial class Form1 : Form
    {
        ContactList model = new ContactList();
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            textName.Text = textSurname.Text = textCompany.Text = textNumber.Text = textAdress.Text = "";
            buttonSave.Text = "Save";
            buttonDelete.Enabled = false;
            model.Id = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            BindDataGridView();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            model.FName2 = textName.Text.Trim();
            model.Surname = textSurname.Text.Trim();
            model.Company = textCompany.Text.Trim();
            model.Number = textNumber.Text.Trim();
            model.Adress = textAdress.Text.Trim();

            using (DBEntities db = new DBEntities())
                {
                if (model.Id == 0)//insert
                    db.ContactList.Add(model);
                else //update
                    db.Entry(model).State = EntityState.Modified;
                     db.SaveChanges();
                }
            Clear();
            BindDataGridView();
            MessageBox.Show("Added successfully");
        }

        void BindDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.ContactList.ToList<ContactList>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["Id"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.ContactList.Where(x => x.Id == model.Id).FirstOrDefault();
                    textName.Text = model.FName2;
                    textSurname.Text = model.Surname;
                    textCompany.Text = model.Company;
                    textNumber.Text = model.Number;
                    textAdress.Text = model.Adress;
                }
                buttonSave.Text = "Update";
                buttonDelete.Enabled = true;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to delete this record?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.ContactList.Attach(model);
                    db.ContactList.Remove(model);
                    db.SaveChanges();
                    BindDataGridView();
                    Clear();
                    MessageBox.Show("Deleted successfully");
                }
            }
        }
    }
}
