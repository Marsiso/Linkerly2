using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeLists.Commands;

public class UpdateCodeListCommand : ICommand<Unit>
{
    public UpdateCodeListCommand(int codeListID, string? name)
    {
        CodeListID = codeListID;
        Name = name;
    }

    public int CodeListID { get; }
    public string? Name { get; }
}

public class UpdateCodeListCommandHandler : ICommandHandler<UpdateCodeListCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public UpdateCodeListCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(UpdateCodeListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalCodeList = _databaseContext.CodeLists
            .AsTracking()
            .SingleOrDefault(codeList => codeList.CodeListID == request.CodeListID);

        if (originalCodeList is null) throw new EntityNotFoundException(request.CodeListID.ToString(), nameof(CodeListEntity));

        _ = _mapper.Map(request, originalCodeList);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}
