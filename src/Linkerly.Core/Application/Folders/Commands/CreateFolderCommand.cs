using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.Folders.Commands;

public class CreateFolderCommand : ICommand<Unit>
{
	public CreateFolderCommand(int userId, int? parentId, int typeId, string? name, long totalSize, long totalCount)
	{
		UserID = userId;
		ParentID = parentId;
		TypeID = typeId;
		Name = name;
		TotalSize = totalSize;
		TotalCount = totalCount;
	}

	public int UserID { get; }
	public int? ParentID { get; }
	public int TypeID { get; }
	public string? Name { get; }
	public long TotalSize { get; }
	public long TotalCount { get; }
}

public class CreateFolderCommandHandler : ICommandHandler<CreateFolderCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public CreateFolderCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var folderToCreate = _mapper.Map<FolderEntity>(request);

		_ = _databaseContext.Folders.Add(folderToCreate);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}