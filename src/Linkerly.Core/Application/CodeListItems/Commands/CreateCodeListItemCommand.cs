using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.CodeListItems.Commands;

public class CreateCodeListItemCommand : ICommand<Unit>
{
    public CreateCodeListItemCommand(int codeListId, string? value)
    {
        CodeListID = codeListId;
        Value = value;
    }

    public int CodeListID { get; }
    public string? Value { get; }
}

public class CreateCodeListItemCommandHandler : ICommandHandler<CreateCodeListItemCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public CreateCodeListItemCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(CreateCodeListItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var codeListItemToCreate = _mapper.Map<CodeListItemEntity>(request);

        _ = _databaseContext.CodeListItems.Add(codeListItemToCreate);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}