namespace MyLibrary.NET.Data.Domain {
    using System;
    using System.ComponentModel;

    public class Book {
        [DisplayName("شناسه کتاب")]
        public int BookId { get; set; }

        [DisplayName("عنوان")]
        public string Name { get; set; }

        [DisplayName("شناسه ناشر")]
        public int PublisherId { get; set; }

        [DisplayName("نویسنده")]
        public string Author { get; set; }

        [DisplayName("تاریخ انتشار")]
        public DateTime PublishedOn { get; set; }

        [DisplayName("چابک")]
        public string ISBN { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }
    }
}