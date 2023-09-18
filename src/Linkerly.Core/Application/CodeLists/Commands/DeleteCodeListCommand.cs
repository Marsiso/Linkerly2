using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeLists.Commands;

public class DeleteCodeListCommand : ICommand<Unit>
{
    public DeleteCodeListCommand(int codeListID)
    {
        CodeListID = codeListID;
    }

    public int CodeListID { get; }
}

public class DeleteCodeListCommandHandler : ICommandHandler<DeleteCodeListCommand, Unit>
{
    private readonly CloudContext _databaseContext;

    public DeleteCodeListCommandHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Unit> Handle(DeleteCodeListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalCodeList = _databaseContext.CodeLists.AsTracking().SingleOrDefault(codeList => codeList.CodeListID == request.CodeListID);

        if (originalCodeList is null) throw new EntityNotFoundException(request.CodeListID.ToString(), nameof(CodeListEntity));

        originalCodeList.IsActive = false;

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}