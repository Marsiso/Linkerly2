using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Files.Commands;

public class UpdateFileCommand : ICommand<Unit>
{
	public UpdateFileCommand(int fileId, int folderId, int extensionId, int mimeTypeId, string? safeName, string? unsafeName, string? location, long size)
	{
		FileID = fileId;
		FolderID = folderId;
		ExtensionID = extensionId;
		MimeTypeID = mimeTypeId;
		SafeName = safeName;
		UnsafeName = unsafeName;
		Location = location;
		Size = size;
	}

	public int FileID { get; }
	public int FolderID { get; }
	public int ExtensionID { get; }
	public int MimeTypeID { get; }
	public string? SafeName { get; }
	public string? UnsafeName { get; }
	public string? Location { get; }
	public long Size { get; }
}

public class UpdateFileCommandHandler : ICommandHandler<UpdateFileCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public UpdateFileCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var originalFile = _databaseContext.Files.AsTracking().SingleOrDefault(file => file.FileID == request.FileID);

		if (originalFile is null)
		{
			throw new EntityNotFoundException(request.FileID.ToString(), nameof(FileEntity));
		}

		_ = _mapper.Map(request, originalFile);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}