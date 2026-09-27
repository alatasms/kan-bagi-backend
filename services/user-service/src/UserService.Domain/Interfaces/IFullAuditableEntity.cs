namespace UserService.Domain.Interfaces
{
    public interface IFullAuditableEntity<T> : IAuditableEntity, IHasDeletedTime
    {
        public T Id { get; set; }
    }
}
