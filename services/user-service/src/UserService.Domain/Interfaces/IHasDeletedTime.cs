namespace UserService.Domain.Interfaces
{
    public interface IHasDeletedTime
    {
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

