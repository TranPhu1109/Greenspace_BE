using AutoMapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;

namespace GreenSpace.Application;
public interface IUnitOfWork
{
    IMapper Mapper { get; }
    IConnectionConfiguration DirectionConnection { get; }
    IUserRepository UserRepository { get; }
    Task<bool> SaveChangesAsync();
}