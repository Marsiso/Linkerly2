using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Files.Queries;

public class GetFileQuery : IQuery<FileEntity?>
{
    public GetFileQuery(int fileId)
    {
        FileID = fileId;
    }

    public int FileID { get; }
}

public class GetFileQueryHandler : IQueryHandler<GetFileQuery, FileEntity?>
{
    private static readonly Func<CloudContext, int, FileEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int fileID) => databaseContext.Files.AsTracking().SingleOrDefault(file => file.FileID == fileID));

    private readonly CloudContext _databaseContext;

    public GetFileQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<FileEntity?> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.FileID < 0) return Task.FromResult<FileEntity?>(default);

        var originalEntity = _query(_databaseContext, request.FileID);

        return Task.FromResult(originalEntity);
    }
}