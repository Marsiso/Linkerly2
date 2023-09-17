using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeListItems.Queries;

public class GetCodeListItemQuery : IQuery<CodeListItemEntity>
{
    public GetCodeListItemQuery(int codeListItemId)
    {
        CodeListItemID = codeListItemId;
    }

    public int CodeListItemID { get; }
}

public class GetCodeListItemQueryHandler : IQueryHandler<GetCodeListItemQuery, CodeListItemEntity?>
{
    private readonly CloudContext _databaseContext;

    private readonly Func<CloudContext, int, CodeListItemEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int codeListItemID) => databaseContext.CodeListItems.AsTracking().SingleOrDefault(codeListItem => codeListItem.CodeListItemID == codeListItemID));

    public GetCodeListItemQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<CodeListItemEntity?> Handle(GetCodeListItemQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.CodeListItemID < 1)
        {
            return Task.FromResult<CodeListItemEntity?>(default);
        }

        CodeListItemEntity? originalCodeList = _query(_databaseContext, request.CodeListItemID);

        return Task.FromResult(originalCodeList);
    }
}