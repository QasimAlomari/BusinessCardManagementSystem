namespace Domain.Base
{
    public class BaseEntity
    {
        public string? CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
