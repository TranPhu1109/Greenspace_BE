using Dapper;
using GreenSpace.Application.Commons;
using GreenSpace.Application.Data;
using GreenSpace.Application.ViewModels.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.User.Queries
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public class QueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
        {
            private readonly IConnectionConfiguration _connection;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                _connection = unitOfWork.DirectionConnection;
            }
            public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                //var user = await  _

                using var connection = _connection.GetDbConnection();
                var query = SQLQueriesStorage.GET_USER_BY_ID;
                var parameters = new DynamicParameters();
                parameters.Add("@id", request.Id);
                var result = await connection.QueryFirstOrDefaultAsync<UserViewModel>(
                    sql: query,
                    param: parameters,
                    transaction: null,
                    commandTimeout: 90,
                    commandType: CommandType.Text)
                            ?? throw new Exception($"Error: {nameof(GetUserByIdQuery)}: no_data_found");
                return result;
            }
        }
    }
}
