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
    IUserRepository userRepository,
    IMapper mapper,
    IConnectionConfiguration connectionConfiguration, 
    IProductRepository productRepository,
    IImageRepository imageRepository,
    ICategoryRepository categoryRepository,
    IDesignIdeaRepository designIdeaRepository, 
    IProductDetailRepository productDetailRepository,
    IProductFeedbackRepository productFeedbackRepository,
    IRoleRepository roleRepository,
    IWalletRepository walletRepository,
    IDesignIdeasCategoryRepository designIdeasCategoryRepository,
    IServiceFeedbackRepositoy serviceFeedbackRepositoy,
    IServiceOrderRepository serviceOrderRepository,
    IServiceOrderDetailRepository serviceOrderDetailRepository,
    IRecordDesignRepository recordDesignRepository,
    IRecordSketchRepository recordSketchRepository,
    IWorkTaskRepository workTaskRepository)
    {
        _dbContext = dbcontext;
        DirectionConnection = connectionConfiguration;
        UserRepository = userRepository;
        RoleRepository  = roleRepository;
        WalletRepository = walletRepository;
        Mapper = mapper;
        ProductRepository = productRepository;
        ImageRepository =  imageRepository ;
        CategoryRepository = categoryRepository;
        DesignIdeaRepository = designIdeaRepository;
        ProductDetailRepository = productDetailRepository;
        ProductFeedbackRepository = productFeedbackRepository;
        ServiceFeedbackRepositoy = serviceFeedbackRepositoy;
        DesignIdeasCategoryRepository = designIdeasCategoryRepository;
        ServiceOrderRepository = serviceOrderRepository;
        ServiceOrderDetailRepository = serviceOrderDetailRepository;
        RecordDesignRepository = recordDesignRepository;
        RecordSketchRepository = recordSketchRepository;
        WorkTaskRepository = workTaskRepository;

    }
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IImageRepository ImageRepository { get; }

    public IDesignIdeaRepository DesignIdeaRepository { get; }

    public IProductDetailRepository ProductDetailRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductFeedbackRepository ProductFeedbackRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IWalletRepository WalletRepository { get; }
    public IDesignIdeasCategoryRepository DesignIdeasCategoryRepository { get; }
    public IServiceFeedbackRepositoy ServiceFeedbackRepositoy { get; }
    public IServiceOrderRepository ServiceOrderRepository { get; }
    public IServiceOrderDetailRepository ServiceOrderDetailRepository { get; }

    public IRecordDesignRepository RecordDesignRepository { get; }
    public IRecordSketchRepository RecordSketchRepository { get; }
    public IWorkTaskRepository WorkTaskRepository { get; }
    public IMapper Mapper { get; }

    public IConnectionConfiguration DirectionConnection { get; }


    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}