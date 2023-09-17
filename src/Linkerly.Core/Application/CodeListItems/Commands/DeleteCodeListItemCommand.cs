using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeListItems.Commands;

public class DeleteCodeListItemCommand : ICommand<Unit>
{
    public DeleteCodeListItemCommand(int codeListItemId)
    {
        CodeListItemID = codeListItemId;
    }

    public int CodeListItemID { get; }
}

public class DeleteCodeListItemCommandHandler : ICommandHandler<DeleteCodeListItemCommand, Unit>
{
    private readonly CloudContext _databaseContext;

    public DeleteCodeListItemCommandHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Unit> Handle(DeleteCodeListItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        CodeListItemEntity? originalCodeListItem = _databaseContext.CodeListItems.AsTracking().SingleOrDefault(codeListItem => codeListItem.CodeListItemID == request.CodeListItemID);

        if (originalCodeListItem is null)
        {
            throw new EntityNotFoundException(request.CodeListItemID.ToString(), nameof(CodeListItemEntity));
        }

        originalCodeListItem.IsActive = false;

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}