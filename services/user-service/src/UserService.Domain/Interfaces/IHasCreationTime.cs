namespace UserService.Domain.Interfaces
{
    public interface IHasCreatedTime
    {
        public DateTime? CreationTime { get; set; }
        public long CreatedBy { get; set; }
    }
}

