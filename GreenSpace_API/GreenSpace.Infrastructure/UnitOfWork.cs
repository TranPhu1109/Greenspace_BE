using AutoMapper;
using GreenSpace.Application;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;

namespace GreenSpace.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbcontext, 
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IWalletRepository walletRepository,
    IMapper mapper,
    IConnectionConfiguration connectionConfiguration)
    {
        _dbContext = dbcontext;
        DirectionConnection = connectionConfiguration;
        UserRepository = userRepository;
        RoleRepository  = roleRepository;
        WalletRepository = walletRepository;
        Mapper = mapper;
    }
    public IUserRepository UserRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IWalletRepository WalletRepository { get; }
    public IMapper Mapper { get; }

    public IConnectionConfiguration DirectionConnection { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}