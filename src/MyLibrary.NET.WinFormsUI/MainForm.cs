namespace MyLibrary.NET.WinFormsUI {
    using System;
    using System.Windows.Forms;

    using MyLibrary.NET.Data;

    public partial class MainForm : Form {
        private readonly MyLibraryDatabase _database;

        public MainForm() {
            InitializeComponent();
            _database = new MyLibraryDatabase();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            ReloadBooks();
            ReloadPublishers();
        }

        private void ReloadBooks() {
            dgvBooks.DataSource = _database.GetBooks();
        }

        private void ReloadPublishers() {
            dgvPublishers.DataSource = _database.GetPublishers();
        }

        private void btnNewPublisher_Click(object sender, EventArgs e) {
            using (var insertPublisherForm = new InsertOrUpdatePublisherForm()) {
                insertPublisherForm.ShowDialog(this);

                if (insertPublisherForm.DialogResult == DialogResult.OK)
                    ReloadPublishers();
            }
        }

        private void btnNewBook_Click(object sender, EventArgs e) {
            using (var insertBookForm = new InsertOrUpdateBookForm()) {
                insertBookForm.ShowDialog(this);

                if (insertBookForm.DialogResult == DialogResult.OK)
                    ReloadBooks();
            }
        }

        private void btnEditBook_Click(object sender, EventArgs e) {
            if (dgvBooks.Rows.Count == 0)
                return;

            var selectedRow = dgvBooks.SelectedRows[0];

            if (selectedRow == null)
                return;

            var bookId = int.Parse(selectedRow.Cells[0].Value.ToString());

            using (var updateBookForm = new InsertOrUpdateBookForm(bookId)) {
                updateBookForm.ShowDialog(this);

                if (updateBookForm.DialogResult == DialogResult.OK)
                    ReloadBooks();
            }
        }

        private void btnDeleteBook_Click(object sender, EventArgs e) {
            if (dgvBooks.Rows.Count == 0)
                return;

            var selectedRow = dgvBooks.SelectedRows[0];

            if (selectedRow == null)
                return;

            var bookId = int.Parse(selectedRow.Cells[0].Value.ToString());

            if (! _database.ExecuteDelete("Books", "BookId", bookId))
                MessageBox.Show("در حذف اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                ReloadBooks();
        }

        private void btnDeletePublisher_Click(object sender, EventArgs e) {
            if (dgvPublishers.Rows.Count == 0)
                return;

            var selectedRow = dgvPublishers.SelectedRows[0];

            if (selectedRow == null)
                return;

            var publisherId = int.Parse(selectedRow.Cells[0].Value.ToString());

            if (! _database.ExecuteDelete("Publishers", "PublisherId", publisherId))
                MessageBox.Show("در حذف اطلاعات مشکلی رخ داده است!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                ReloadPublishers();
        }

        private void btnEditPublisher_Click(object sender, EventArgs e) {
            if (dgvPublishers.Rows.Count == 0)
                return;

            var selectedRow = dgvPublishers.SelectedRows[0];

            if (selectedRow == null)
                return;

            var publisherId = int.Parse(selectedRow.Cells[0].Value.ToString());

            using (var updatePublisherForm = new InsertOrUpdatePublisherForm(publisherId)) {
                updatePublisherForm.ShowDialog(this);

                if (updatePublisherForm.DialogResult == DialogResult.OK)
                    ReloadPublishers();
            }
        }
    }
}