using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;
using Linkerly.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Folders.Commands;

public class UpdateFolderCommand : IQuery<Unit>
{
	public UpdateFolderCommand(int folderId, int userId, int? parentId, int typeId, string? name, long totalSize, long totalCount)
	{
		FolderID = folderId;
		UserID = userId;
		ParentID = parentId;
		TypeID = typeId;
		Name = name;
		TotalSize = totalSize;
		TotalCount = totalCount;
	}

	public int FolderID { get; }
	public int UserID { get; }
	public int? ParentID { get; }
	public int TypeID { get; }
	public string? Name { get; }
	public long TotalSize { get; }
	public long TotalCount { get; }
}

public class UpdateFolderCommandHandler : IQueryHandler<UpdateFolderCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public UpdateFolderCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(UpdateFolderCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var originalFolder = _databaseContext.Folders.AsTracking().SingleOrDefault(folder => folder.FolderID == request.FolderID);

		if (originalFolder is null)
		{
			throw new EntityNotFoundException(request.FolderID.ToString(), nameof(FolderEntity));
		}

		_ = _mapper.Map(request, originalFolder);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}