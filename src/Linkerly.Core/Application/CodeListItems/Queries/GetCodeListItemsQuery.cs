using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeListItems.Queries;

public class GetCodeListItemsQuery : IQuery<IEnumerable<CodeListItemEntity>>
{
    public GetCodeListItemsQuery(int codeListId)
    {
        CodeListID = codeListId;
    }

    public int CodeListID { get; }
}

public class GetCodeListItemsQueryHandler : IQueryHandler<GetCodeListItemsQuery, IEnumerable<CodeListItemEntity>>
{
    private static readonly Func<CloudContext, int, IEnumerable<CodeListItemEntity>> _query = EF.CompileQuery((CloudContext databaseContext, int codeListID) =>
        databaseContext.CodeListItems
            .AsNoTracking()
            .Where(codeListItem => codeListItem.CodeListID == codeListID)
            .AsEnumerable());

    private readonly CloudContext _databaseContext;

    public GetCodeListItemsQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<IEnumerable<CodeListItemEntity>> Handle(GetCodeListItemsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.CodeListID < 1) return Task.FromResult(Enumerable.Empty<CodeListItemEntity>());

        var originalCodeListItems = _query(_databaseContext, request.CodeListID);

        return Task.FromResult(originalCodeListItems);
    }
}
