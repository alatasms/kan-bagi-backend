namespace PostService.Domain.Interfaces
{
    public interface IFullAuditableEntity<T>: IHasCreatedTime, IHasLastModifiedTime, IHasDeletedTime
    {
        public T Id { get; set; }
    }
}
