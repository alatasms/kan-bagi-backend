using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Exceptions;
using PostService.Application.Features.Commands;
using PostService.Application.Features.Common;
using PostService.Application.Security;
using PostService.Domain.Events;
using PostService.Infrastructure.Data;
using PostService.Infrastructure.Repositories;

namespace PostService.Application.Features.Handler
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, StandardResponse<object>>
    {
        private readonly IPostRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePostCommandHandler(IPostRepository repository, IPublishEndpoint publishEndpoint, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _currentUser = currentUser;
        }

        public async Task<StandardResponse<object>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Post not found!");

            if (entity.OwnerId != _currentUser.UserId.ToString())
                throw new UnauthorizedAccessException("Not Authorized!");

            // Soft delete (see PostServiceDbContext) and event are committed together through the outbox.
            _repository.Remove(entity);
            await _publishEndpoint.Publish(new PostDeletedEvent(request.PostId, entity.CorrelationId), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new StandardResponse<object>();
        }
    }
}
