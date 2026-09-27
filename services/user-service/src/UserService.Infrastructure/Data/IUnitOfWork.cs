namespace UserService.Infrastructure.Data
{
    /// <summary>
    /// Commits every pending change, including events published through the outbox, in one transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
