using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Files.Queries;

public class GetAllFolderFilesQuery : IQuery<IEnumerable<FileEntity>>
{
	public GetAllFolderFilesQuery(int folderId)
	{
		FolderID = folderId;
	}

	public int FolderID { get; }
}

public class GetAllFolderFilesQueryHandler : IQueryHandler<GetAllFolderFilesQuery, IEnumerable<FileEntity>>
{
	private static readonly Func<CloudContext, int, IEnumerable<FileEntity>> _query = EF.CompileQuery((CloudContext databaseContext, int folderID) => databaseContext.Files.AsTracking().Where(file => file.FolderID == folderID).AsEnumerable());

	private readonly CloudContext _databaseContext;

	public GetAllFolderFilesQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<IEnumerable<FileEntity>> Handle(GetAllFolderFilesQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (request.FolderID < 1)
		{
			return Task.FromResult(Enumerable.Empty<FileEntity>());
		}

		var originalFiles = _query(_databaseContext, request.FolderID);

		return Task.FromResult(originalFiles);
	}
}