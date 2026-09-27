namespace UserService.Domain.Interfaces
{
    public interface IHasLastModifiedTime
    {
        public DateTime? LastModified { get; set; }
        public long LastModifiedBy { get; set; }
    }
}

