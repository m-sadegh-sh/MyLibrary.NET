namespace MyLibrary.NET.Data {
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	using MyLibrary.NET.Data.Domain;

	public sealed class MyLibraryDatabase : DbOperationBase {
		public MyLibraryDatabase() : base("NETGUYPC",
				"MyLibrary.NET.Database") {}

		public IList<Book> GetBooks(Func<Book, bool> predicate = null) {
			const string booksSelectQuery = "select * from books";

			var books = new List<Book>();

			var sqlDataReader = InternalGetSqlDataReader(booksSelectQuery);

			if (sqlDataReader == null)
				return books;

			while (sqlDataReader.Read())
				books.Add(ParseBook(sqlDataReader));

			sqlDataReader.Close();

			if (predicate != null) {
				books = books.Where(predicate).
						ToList();
			}

			return books;
		}

		private static Book ParseBook(IDataRecord sqlDataReader) {
			var book = new Book {
					BookId = int.Parse(sqlDataReader["BookId"].ToString()),
					Name = sqlDataReader["Name"].ToString(),
					PublisherId = int.Parse(sqlDataReader["PublisherId"].ToString()),
					Author = sqlDataReader["Author"].ToString(),
					PublishedOn = DateTime.Parse(sqlDataReader["PublishedOn"].ToString()),
					ISBN = sqlDataReader["ISBN"].ToString(),
					Description = sqlDataReader["Description"].ToString()
			};

			return book;
		}

		public IList<Publisher> GetPublishers(Func<Publisher, bool> predicate = null) {
			const string publishersSelectQuery = "select * from publishers";

			var publishers = new List<Publisher>();

			var sqlDataReader = InternalGetSqlDataReader(publishersSelectQuery);

			if (sqlDataReader == null)
				return publishers;

			while (sqlDataReader.Read())
				publishers.Add(ParsePublisher(sqlDataReader));

			sqlDataReader.Close();

			if (predicate != null) {
				publishers = publishers.Where(predicate).
						ToList();
			}

			return publishers;
		}

		private static Publisher ParsePublisher(IDataRecord sqlDataReader) {
			var publisher = new Publisher {
					PublisherId = int.Parse(sqlDataReader["PublisherId"].ToString()),
					Name = sqlDataReader["Name"].ToString()
			};

			return publisher;
		}
	}
}