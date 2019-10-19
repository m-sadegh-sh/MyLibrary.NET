namespace MyLibrary.NET.WinFormsUI {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using MyLibrary.NET.Data;
    using MyLibrary.NET.Data.Domain;

    public partial class InsertOrUpdatePublisherForm : Form {
        private readonly MyLibraryDatabase _database;
        private readonly bool _isUpdate;
        private Publisher _publisher;

        public InsertOrUpdatePublisherForm(int? publisherId = null) {
            InitializeComponent();
            _database = new MyLibraryDatabase();

            if (publisherId.HasValue) {
                _isUpdate = true;
                _publisher = _database.GetPublishers(p => p.PublisherId == publisherId).Single();

                txtName.Text = _publisher.Name;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e) {
            if (IsValid()) {
                if (!_isUpdate)
                    _publisher = new Publisher();

                _publisher.Name = txtName.Text.Trim();

                var nameValuePairs = new Dictionary<string, object>();

                nameValuePairs.Add("Name", _publisher.Name);

				if (_isUpdate) {
					if (!_database.ExecuteUpdate("Publishers", "PublisherId", _publisher.PublisherId, nameValuePairs))
						MessageBox.Show("در درج/ویرایش اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					else
						Close();
				}
				else {
					if (!_database.ExecuteInsert<Publisher>("Publishers", nameValuePairs.Values.ToArray()))
						MessageBox.Show("در درج/ویرایش اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					else
						Close();
				}
            }
        }

        private bool IsValid() {
            return true;
        }
    }
}