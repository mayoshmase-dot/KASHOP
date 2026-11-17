namespace KASHOP.DAL.Models
{
    public class AudiTableEntity
    {
        public DateTime CreatedAt { get; set; }
        public string? CreatedById { get; set; }
        public ApplictionUser? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedById { get; set; }
        public ApplictionUser? UpdatedBy { get; set; }
    }
}