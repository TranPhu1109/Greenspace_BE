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
    IConnectionConfiguration connectionConfiguration, IProductRepository productRepository,IImageRepository imageRepository,ICategoryRepository categoryRepository,
      IDesignIdeaRepository designIdeaRepository, IProductDetailRepository productDetailRepository)
    {
        _dbContext = dbcontext;
        DirectionConnection = connectionConfiguration;
        UserRepository = userRepository;
        Mapper = mapper;
        ProductRepository = productRepository;
        ImageRepository =  imageRepository ;
        CategoryRepository = categoryRepository;
        DesignIdeaRepository = designIdeaRepository;
        ProductDetailRepository = productDetailRepository;
    }
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IImageRepository ImageRepository { get; }

    public IDesignIdeaRepository DesignIdeaRepository { get; }

    public IProductDetailRepository ProductDetailRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IMapper Mapper { get; }

    public IConnectionConfiguration DirectionConnection { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}