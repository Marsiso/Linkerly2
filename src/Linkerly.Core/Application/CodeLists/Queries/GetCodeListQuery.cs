using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.CodeLists.Queries;

public class GetCodeListQuery : IQuery<CodeListEntity?>
{
    public GetCodeListQuery(int codeListID)
    {
        CodeListID = codeListID;
    }

    public int CodeListID { get; }
}

public class GetCodeListQueryHandler : IQueryHandler<GetCodeListQuery, CodeListEntity?>
{
    private static readonly Func<CloudContext, int, CodeListEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int codeListID) => databaseContext.CodeLists.AsTracking().SingleOrDefault(codeList => codeList.CodeListID == codeListID));

    private readonly CloudContext _databaseContext;

    public GetCodeListQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<CodeListEntity?> Handle(GetCodeListQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.CodeListID < 1)
        {
            return Task.FromResult<CodeListEntity?>(default);
        }

        CodeListEntity? originalCodeList = _query(_databaseContext, request.CodeListID);

        return Task.FromResult(originalCodeList);
    }
}