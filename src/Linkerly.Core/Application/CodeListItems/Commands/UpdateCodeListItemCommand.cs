using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeListItems.Commands;

public class UpdateCodeListItemCommand : ICommand<Unit>
{
    public UpdateCodeListItemCommand(int codeListItemId, int codeListId, string? value)
    {
        CodeListItemID = codeListItemId;
        CodeListID = codeListId;
        Value = value;
    }

    public int CodeListItemID { get; }
    public int CodeListID { get; }
    public string? Value { get; }
}

public class UpdateCodeListItemCommandHandler : ICommandHandler<UpdateCodeListItemCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public UpdateCodeListItemCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(UpdateCodeListItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalCodeListItem = _databaseContext.CodeListItems.AsTracking().SingleOrDefault(codeListItem => codeListItem.CodeListItemID == request.CodeListItemID);

        if (originalCodeListItem is null) throw new EntityNotFoundException(request.CodeListItemID.ToString(), nameof(CodeListItemEntity));

        _ = _mapper.Map(request, originalCodeListItem);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}