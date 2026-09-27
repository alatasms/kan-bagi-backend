namespace UserService.Domain.Interfaces
{
    public interface IAuditableEntity : IEntity, IHasCreatedTime, IHasLastModifiedTime
    {
    }
}
