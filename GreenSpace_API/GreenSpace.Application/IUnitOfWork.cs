using AutoMapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;

namespace GreenSpace.Application;
public interface IUnitOfWork
{
    IMapper Mapper { get; }
    IConnectionConfiguration DirectionConnection { get; }
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    IWalletRepository WalletRepository { get; }
    Task<bool> SaveChangesAsync();
    IProductRepository ProductRepository { get; }
    IImageRepository ImageRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IDesignIdeaRepository DesignIdeaRepository { get; }
    IProductDetailRepository ProductDetailRepository { get; }
    IProductFeedbackRepository ProductFeedbackRepository { get; }
    IDesignIdeasCategoryRepository DesignIdeasCategoryRepository { get; }
    IServiceFeedbackRepositoy ServiceFeedbackRepositoy { get; }
    IServiceOrderRepository ServiceOrderRepository { get; }
    IServiceOrderDetailRepository ServiceOrderDetailRepository { get; }
    IRecordDesignRepository RecordDesignRepository { get; }
    IRecordSketchRepository RecordSketchRepository { get; }

    IWorkTaskRepository WorkTaskRepository { get; }
}