using Dapper;
using GreenSpace.Application.Commons;
using GreenSpace.Application.Data;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.User.Queries
{
   
    public class GetAllBanUserQueryModel
    {
        public Dictionary<string, string> Filter { get; set; } = default!;
        public int PageNumber { get; set; } = -1;
    }

    public class GetAllBanUserQuery : GetAllUserQueryModel, IRequest<PaginatedList<UserViewModel>?>
    {
        public class QueryHandler : IRequestHandler<GetAllBanUserQuery, PaginatedList<UserViewModel>?>
        {
            private readonly IConnectionConfiguration _connection;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                _connection = unitOfWork.DirectionConnection;
            }
            public async Task<PaginatedList<UserViewModel>?> Handle(GetAllBanUserQuery request, CancellationToken cancellationToken)
            {
                if (request.Filter.Count > 0)
                {
                    request.Filter.Remove("pageNumber");
                }
                using var connection = _connection.GetDbConnection();
                var query = SQLQueriesStorage.GET_ALL_BAN_USER;

                var result = await connection.QueryAsync<UserViewModel>(query) ?? new List<UserViewModel>();
                // Adding Fillter
                var returnResult = new List<UserViewModel>();
                System.Console.WriteLine(request.Filter?.Count);
                if (request.Filter?.Count > 0)
                {

                    //request.Filter.Remove("PageNumber");
                    foreach (var item in request.Filter)
                    {
                        returnResult = returnResult.Union(FilterUtilities.SelectItems(result, item.Key, item.Value)).ToList();
                    }
                }
                else returnResult = result.ToList();
                return PaginatedList<UserViewModel>.Create(
                    source: returnResult.AsQueryable(),
                    pageIndex: request.PageNumber >= 0 ? request.PageNumber : 0,
                    pageSize: request.PageNumber >= 0 ? 100 : returnResult.Count
                );
            }
        }
    }
}
