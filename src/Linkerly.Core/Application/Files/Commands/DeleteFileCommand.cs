using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Files.Commands;

public class DeleteFileCommand : ICommand<Unit>
{
    public DeleteFileCommand(int fileID)
    {
        FileID = fileID;
    }

    public int FileID { get; }
}

public class DeleteFileCommandHandler : ICommandHandler<DeleteFileCommand, Unit>
{
    private readonly CloudContext _databaseContext;

    public DeleteFileCommandHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalFile =
            _databaseContext.Files.AsTracking().SingleOrDefault(file => file.FileID == request.FileID);

        if (originalFile is null) throw new EntityNotFoundException(request.FileID.ToString(), nameof(FileEntity));

        originalFile.IsActive = false;

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}