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

public class GetAllFolderFilesQueryHandler : IQueryHandler<GetAllSubfoldersQuery, IEnumerable<FolderEntity>>
{
    private static readonly Func<CloudContext, int, IEnumerable<FolderEntity>> _query = EF.CompileQuery((CloudContext databaseContext, int parentFolderID) => databaseContext.Folders.AsTracking().Where(folder => folder.ParentID == parentFolderID).AsEnumerable());

    private readonly CloudContext _databaseContext;

    public GetAllFolderFilesQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<IEnumerable<FolderEntity>> Handle(GetAllSubfoldersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.ParentFolderID < 1)
        {
            return Task.FromResult(Enumerable.Empty<FolderEntity>());
        }

        IEnumerable<FolderEntity> originalSubfolders = _query(_databaseContext, request.ParentFolderID);

        return Task.FromResult(originalSubfolders);
    }
}