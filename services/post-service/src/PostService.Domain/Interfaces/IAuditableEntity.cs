namespace PostService.Domain.Interfaces
{
    public interface IAuditableEntity: IHasCreatedTime, IHasLastModifiedTime
    {
    }
}
