using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeLists.Queries;

public class GetAllCodeListsQuery : IQuery<IEnumerable<CodeListEntity>>
{
}

public class GetAllCodeListsQueryHandler : IQueryHandler<GetAllCodeListsQuery, IEnumerable<CodeListEntity>>
{
    private static readonly Func<CloudContext, IEnumerable<CodeListEntity>> _query = EF.CompileQuery((CloudContext databaseContext) => databaseContext.CodeLists.AsTracking().AsEnumerable());

    private readonly CloudContext _databaseContext;

    public GetAllCodeListsQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<IEnumerable<CodeListEntity>> Handle(GetAllCodeListsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        IEnumerable<CodeListEntity> originalCodeLists = _query(_databaseContext);

        return Task.FromResult(originalCodeLists);
    }
}