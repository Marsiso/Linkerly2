using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.CodeLists.Commands;

public class CreateCodeListCommand : ICommand<Unit>
{
    public CreateCodeListCommand(string? name)
    {
        Name = name;
    }

    public string? Name { get; }
}

public class CreateCodeListCommandHandler : ICommandHandler<CreateCodeListCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public CreateCodeListCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(CreateCodeListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var codeListToCreate = _mapper.Map<CodeListEntity>(request);

        _ = _databaseContext.CodeLists.Add(codeListToCreate);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}
