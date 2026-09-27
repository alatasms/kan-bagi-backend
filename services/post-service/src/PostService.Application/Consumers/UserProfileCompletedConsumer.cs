using MassTransit;
using PostService.Application.Contracts;
using PostService.Domain.Entities;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Consumers
{
    public class UserProfileCompletedConsumer : IConsumer<UserProfileCompletedEvent>
    {
        private readonly IPostOwnerRepository _postOwnerRepository;

        public UserProfileCompletedConsumer(IPostOwnerRepository postOwnerRepository)
        {
            _postOwnerRepository = postOwnerRepository;
        }

        public Task Consume(ConsumeContext<UserProfileCompletedEvent> context)
        {
            var message = context.Message;
            return _postOwnerRepository.UpsertAsync(new PostOwner
            {
                UserId = message.UserId,
                Name = message.Name,
                Surname = message.Surname,
                UpdatedAt = DateTime.UtcNow
            }, context.CancellationToken);
        }
    }
}
