using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Folders.Queries;

public class GetAllSubfoldersQuery : IQuery<IEnumerable<FolderEntity>>
{
    public GetAllSubfoldersQuery(int parentFolderID)
    {
        ParentFolderID = parentFolderID;
    }

    public int ParentFolderID { get; }
}

public class GetAllSubfoldersQueryHandler : IQueryHandler<GetAllSubfoldersQuery, IEnumerable<FolderEntity>>
{
    private static readonly Func<CloudContext, int, IEnumerable<FolderEntity>> _query = EF.CompileQuery((CloudContext databaseContext, int parentFolderID) =>
        databaseContext.Folders
            .AsTracking()
            .Where(folder => folder.ParentID == parentFolderID));

    private readonly CloudContext _databaseContext;

    public GetAllSubfoldersQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<IEnumerable<FolderEntity>> Handle(GetAllSubfoldersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.ParentFolderID < 1) return Task.FromResult(Enumerable.Empty<FolderEntity>());

        var originalSubfolders = _query(_databaseContext, request.ParentFolderID);

        return Task.FromResult(originalSubfolders);
    }
}
