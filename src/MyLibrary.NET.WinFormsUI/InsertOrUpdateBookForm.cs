namespace MyLibrary.NET.WinFormsUI {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using MyLibrary.NET.Data;
    using MyLibrary.NET.Data.Domain;

    public partial class InsertOrUpdateBookForm : Form {
        private readonly MyLibraryDatabase _database;
        private readonly bool _isUpdate;
        private Book _book;

        public InsertOrUpdateBookForm(int? bookId = null) {
            InitializeComponent();
            _database = new MyLibraryDatabase();

            cmbPublishers.DataSource = _database.GetPublishers();

            if (bookId.HasValue) {
                _isUpdate = true;
                _book = _database.GetBooks(b => b.BookId == bookId).Single();

                txtName.Text = _book.Name;
                cmbPublishers.SelectedValue = _book.PublisherId;
                txtAuthor.Text = _book.Author;
                dtPublishedOn.Value = _book.PublishedOn;
                txtISBN.Text = _book.ISBN;
                txtDescription.Text = _book.Description;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e) {
            if (IsValid()) {
                if (!_isUpdate)
                    _book = new Book();

                _book.Name = txtName.Text.Trim();
                _book.PublisherId = int.Parse(cmbPublishers.SelectedValue.ToString());
                _book.Author = txtAuthor.Text.Trim();
                _book.PublishedOn = dtPublishedOn.Value;
                _book.ISBN = txtISBN.Text.Trim();
                _book.Description = txtDescription.Text.Trim();

                var nameValuePairs = new Dictionary<string, object>();

                nameValuePairs.Add("Name", _book.Name);
                nameValuePairs.Add("PublisherId", _book.PublisherId);
                nameValuePairs.Add("Author", _book.Author);
                nameValuePairs.Add("PublishedOn", _book.PublishedOn);
                nameValuePairs.Add("ISBN", _book.ISBN);
                nameValuePairs.Add("Description", _book.Description);

                if (_isUpdate && !_database.ExecuteUpdate("Books", "BookId", _book.BookId, nameValuePairs))
                    MessageBox.Show("در درج/ویرایش اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (!_isUpdate && !_database.ExecuteInsert<Book>("Books", nameValuePairs.Values.ToArray()))
                    MessageBox.Show("در درج/ویرایش اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    Close();
            }
        }

        private bool IsValid() {
            return true;
        }
    }
}