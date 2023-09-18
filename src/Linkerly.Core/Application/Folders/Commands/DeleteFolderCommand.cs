using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Folders.Commands;

public class DeleteFolderCommand : ICommand<Unit>
{
    public DeleteFolderCommand(int folderID)
    {
        FolderID = folderID;
    }

    public int FolderID { get; }
}

public class DeleteFolderCommandHandler : ICommandHandler<DeleteFolderCommand, Unit>
{
    private readonly CloudContext _databaseContext;

    public DeleteFolderCommandHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Unit> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalFolder = _databaseContext.Folders.AsTracking().SingleOrDefault(folder => folder.FolderID == request.FolderID);

        if (originalFolder is null) throw new EntityNotFoundException(request.FolderID.ToString(), nameof(FolderEntity));

        originalFolder.IsActive = false;

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}