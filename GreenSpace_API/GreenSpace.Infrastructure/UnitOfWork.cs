using AutoMapper;
using GreenSpace.Application;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;
using GreenSpace.Infrastructure.Repositories;

namespace GreenSpace.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbcontext, 
    IUserRepository userRepository, IMapper mapper,
    IConnectionConfiguration connectionConfiguration, IProductRepository productRepository,IImageRepository imageRepository,ICategoryRepository categoryRepository)
    {
        _dbContext = dbcontext;
        DirectionConnection = connectionConfiguration;
        UserRepository = userRepository;
        Mapper = mapper;
        ProductRepository = productRepository;
        ImageRepository =  imageRepository ;
        CategoryRepository = categoryRepository;
    }
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IImageRepository ImageRepository { get; }

    public ICategoryRepository CategoryRepository { get; }
    public IMapper Mapper { get; }

    public IConnectionConfiguration DirectionConnection { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}