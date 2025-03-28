using Dapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.ViewModels.Users;
using MediatR;
using System.Data;
using System.Data.SqlClient;

namespace GreenSpace.Application.Features.User.Commands;

public class UnbanUserCommand : IRequest
{
    public Guid Id { get; set; }
    public class CommandHandler : IRequestHandler<UnbanUserCommand>
    {
        private readonly IConnectionConfiguration _connection;
        private readonly IUnitOfWork _unitOfWork;

        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connection = unitOfWork.DirectionConnection;
        }

        public async Task Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            var query = @"UPDATE Users SET IsDeleted = 0 WHERE Id = @UserId";

            if (connection.State != ConnectionState.Open)
                connection.Open();

            await connection.ExecuteAsync(query, new { UserId = request.Id });
            connection.Close();
        }
    }
}
