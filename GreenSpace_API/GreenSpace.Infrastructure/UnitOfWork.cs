using AutoMapper;
using GreenSpace.Application;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;
using GreenSpace.Application.Repositories.MongoDbs;
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
    IWorkTaskRepository workTaskRepository,
    IBillRepository billRepository,
    IWalletLogRepository walletLogRepository,
    IBlogRepository blogRepository,
    IContractRepository contractRepository,
    IOrderRepository orderRepository,
    IOrderDetailRepository orderDetailRepository,
    IComplaintRepository complaintRepository,
    IWebManagerRepository webManagerRepository,
    IDocumentRepository documentRepository,
    IAddressRepository addressRepository,
    IComplaintReasonRepository complaintReasonRepository,
    ITransactionPercentageRepository transactionPercentageRepository,
    IExternalProductsRepository externalProductsRepository,
    IComplaintDetailRepository complaintDetaislRepository
    )
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
        BillRepository = billRepository;
        WorkTaskRepository = workTaskRepository;
        BlogRepository = blogRepository;
        WalletLogRepository = walletLogRepository;
        ContractRepository = contractRepository;
        OrderRepository = orderRepository;
        OrderDetailRepository = orderDetailRepository;
        ComplaintRepository = complaintRepository;
        WebManagerRepository = webManagerRepository;
        DocumentRepository = documentRepository;
        AddressRepository = addressRepository;
        ComplaintReasonRepository = complaintReasonRepository;
        TransactionPercentageRepository = transactionPercentageRepository;
        ExternalProductsRepository = externalProductsRepository;
        ComplaintDetailRepository = complaintDetaislRepository;
        
    }
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IImageRepository ImageRepository { get; }
    public IBillRepository BillRepository { get; }
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

    public IBlogRepository BlogRepository { get; }
    public IContractRepository ContractRepository { get; }
    public IMapper Mapper { get; }
    public IConnectionConfiguration DirectionConnection { get; }
    public IWalletLogRepository WalletLogRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IOrderDetailRepository OrderDetailRepository { get; }

    public IComplaintRepository ComplaintRepository { get; }
    public IWebManagerRepository WebManagerRepository { get; }
    public IDocumentRepository DocumentRepository { get; }
    public IAddressRepository AddressRepository { get; }

    public IComplaintReasonRepository ComplaintReasonRepository { get; }
    public ITransactionPercentageRepository TransactionPercentageRepository { get; }
    public IExternalProductsRepository ExternalProductsRepository { get; }
    public IComplaintDetailRepository ComplaintDetailRepository {  get; }
    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;

}