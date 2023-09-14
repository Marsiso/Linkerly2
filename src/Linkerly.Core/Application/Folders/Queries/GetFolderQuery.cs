using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Folders.Queries;

public class GetFolderQuery : IQuery<FolderEntity>
{
	public GetFolderQuery(int folderID)
	{
		FolderID = folderID;
	}

	public int FolderID { get; }
}

public class GetFolderQueryHandler : IQueryHandler<GetFolderQuery, FolderEntity?>
{
	private static readonly Func<CloudContext, int, FolderEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int folderID) => databaseContext.Folders.AsTracking().SingleOrDefault(folder => folder.FolderID == folderID));

	private readonly CloudContext _databaseContext;

	public GetFolderQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<FolderEntity?> Handle(GetFolderQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (request.FolderID < 1)
		{
			return Task.FromResult<FolderEntity?>(default);
		}

		var originalFolder = _query(_databaseContext, request.FolderID);

		return Task.FromResult(originalFolder);
	}
}