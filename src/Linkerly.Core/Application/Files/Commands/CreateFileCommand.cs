using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.Files.Commands;

public class CreateFileCommand : ICommand<Unit>
{
	public CreateFileCommand(int folderID, int extensionId, int mimeTypeId, string? safeName, string? unsafeName, string? location, long size)
	{
		FolderID = folderID;
		ExtensionID = extensionId;
		MimeTypeID = mimeTypeId;
		SafeName = safeName;
		UnsafeName = unsafeName;
		Location = location;
		Size = size;
	}

	public int FolderID { get; }
	public int ExtensionID { get; }
	public int MimeTypeID { get; }
	public string? SafeName { get; }
	public string? UnsafeName { get; }
	public string? Location { get; }
	public long Size { get; }
}

public class CreateFileCommandHandler : ICommandHandler<CreateFileCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public CreateFileCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(CreateFileCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var fileToCreate = _mapper.Map<FileEntity>(request);

		_ = _databaseContext.Files.Add(fileToCreate);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}