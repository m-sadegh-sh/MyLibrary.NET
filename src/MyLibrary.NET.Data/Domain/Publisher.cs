namespace MyLibrary.NET.Data.Domain {
    using System.ComponentModel;

    public class Publisher {
        [DisplayName("شناسه ناشر")]
        public int PublisherId { get; set; }

        [DisplayName("عنوان")]
        public string Name { get; set; }
    }
}