using AutoMapper;
using GreenSpace.Application;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;

namespace GreenSpace.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbcontext, 
    IUserRepository userRepository, IMapper mapper,
    IConnectionConfiguration connectionConfiguration)
    {
        _dbContext = dbcontext;
        DirectionConnection = connectionConfiguration;
        UserRepository = userRepository;
        Mapper = mapper;
    }
    public IUserRepository UserRepository { get; }
    public IMapper Mapper { get; }

    public IConnectionConfiguration DirectionConnection { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}