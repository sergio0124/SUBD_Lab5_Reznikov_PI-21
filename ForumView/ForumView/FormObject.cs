﻿using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.BusinessLogics;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unity;

namespace ForumView
{
    public partial class FormObject : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int? id;
        private ObjectLogic obj;
        public FormObject(ObjectLogic objLogic, int objid)
        {
            InitializeComponent();
            obj = objLogic;
            id = objid;
            List<ObjectViewModel> list = obj.Read(null);
            list.Remove(new ObjectViewModel { Id = (int)id });
            if (list != null)
            {
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "Id";
                comboBox.DataSource = list;
                comboBox.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == null)
            {
                MessageBox.Show("Заполните поле Название", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxDescription == null)
            {
                MessageBox.Show("Заполните поле Описание", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                obj.CreateOrUpdate(new ObjectBindingModel
                {
                    Description = textBoxDescription.Text,
                    Name = textBoxName.Text,
                    Id = id,
                    ObjectId = Convert.ToInt32(comboBox.SelectedValue)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormObject_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                ObjectViewModel model = obj.Read(new ObjectBindingModel { Id = id })?[0];
                textBoxDescription.Text = model.Description;
                textBoxName.Text = model.Name;
            }
        }

    }
}
