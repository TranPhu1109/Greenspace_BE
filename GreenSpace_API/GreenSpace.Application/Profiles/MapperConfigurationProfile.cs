using AutoMapper;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.DesignIdeasCategory;
using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.RecordDesign;
using GreenSpace.Application.ViewModels.RecordSketch;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Application.ViewModels.WorkTasks;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;

namespace GreenSpace.Application.Profiles;

public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {

        CreateMap<Product, ProductCreateModel>().ReverseMap();
        CreateMap<Product, ProductUpdateModel>().ReverseMap();
        CreateMap<Product, ProductViewModel>()
           .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
           .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
           .ReverseMap();
        CreateMap<ImageCreateModel, Image>().ReverseMap();



        CreateMap<DesignIdea, DesignIdeaCreateModel>().ReverseMap();
        CreateMap<DesignIdea, DesignIdeaUpdateModel>()
            .ForMember(dest => dest.ProductDetails, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.ProductDetails, opt => opt.Ignore());
        CreateMap<DesignIdea, DesignIdeaViewModel>()
           .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.DesignIdeasCategory.Name))
           .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
           .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(src => src.ProductDetails))
           .ReverseMap();


        CreateMap<ProductDetail, ProductDetailCreateModel>().ReverseMap();
        CreateMap<ProductDetail, ProductDetailViewModel>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Product.Category.Name))
            .ReverseMap();

        CreateMap<Category, CategoryViewModel>().ReverseMap();
        CreateMap<Category, CategoryCreateModel>().ReverseMap();
        CreateMap<Category, CategoryUpdateModel>().ReverseMap();

        CreateMap<ProductFeedback, ProductFeedbackViewModel>()
             .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.Name))
             .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.Product.Name))
            .ReverseMap();
        CreateMap<ProductFeedback, ProductFeedbackCreateModel>().ReverseMap();
        CreateMap<ProductFeedback, ProductFeedbackUpdateModel>().ReverseMap();

        CreateMap<ServiceFeedback, ServiceFeedbackViewModel>()
             .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.Name))
             .ForMember(x => x.DesignName, opt => opt.MapFrom(x => x.DesignIdea.Name))
             .ReverseMap();
        CreateMap<ServiceFeedback, ServiceFeedbackCreateModel>().ReverseMap();
        CreateMap<ServiceFeedback, ServiceFeedbackUpdateModel>().ReverseMap();
        //CreateMap<Role, RoleViewModel>().ReverseMap();

        CreateMap<User, UserViewModel>()
            .ForMember(
                x => x.RoleName,
                opt => opt.MapFrom(x => x.Role.RoleName)
            )
            .ReverseMap();
        CreateMap<User, UserCreateModel>().ReverseMap();
        CreateMap<User, UserUpdateModel>().ReverseMap();

        CreateMap<DesignIdeasCategory, DesignIdeasCategoryViewModel>().ReverseMap();
        CreateMap<DesignIdeasCategory, DesignIdeasCategoryCreateModel>().ReverseMap();
        CreateMap<DesignIdeasCategory, DesignIdeasCategoryUpdateModel>().ReverseMap();

        CreateMap<ServiceOrder, ServiceOrderViewModel>()
               .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
               .ForMember(dest => dest.ServiceOrderDetails, opt => opt.MapFrom(src => src.ServiceOrderDetails))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((ServiceOrderStatus)src.Status).ToString()))
               .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.Name))
               
               .ReverseMap(); 
        CreateMap<ServiceOrder, ServiceOrderCreateModel>().ReverseMap();
        CreateMap<ServiceOrder, ServiceOrderNoUsingCreateModel>().ReverseMap();
        CreateMap<ServiceOrder, ServiceOrderUpdateModel>()
            .ForMember(dest => dest.ServiceOrderDetails, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.ServiceOrderDetails, opt => opt.Ignore());

        CreateMap<ServiceOrderDetail, ServiceOrderDetailViewModel>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Product.Category.Name))
            .ReverseMap();
        CreateMap<ServiceOrderDetail, ServiceOrderDetailCreateModel>().ReverseMap();
        CreateMap<RecordSketch, RecordSketchViewModel>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ReverseMap();
        CreateMap<RecordSketch, RecordSketchUpdateModel>().ReverseMap();

        CreateMap<RecordDesign, RecordDesignUpdateModel>().ReverseMap();
        CreateMap<RecordDesign, RecordDesignViewModel>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ReverseMap();

        CreateMap<WorkTask, WorkTaskViewModel>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((WorkTasksEnum)src.Status).ToString()))
            .ReverseMap();
        CreateMap<WorkTask, WorkTaskCreateModel>().ReverseMap();
        CreateMap<WorkTask, WorkTaskUpdateModel>().ReverseMap();
        //#endregion

        //#region Wallet
        //CreateMap<Wallet, WalletViewModel>()
        //    .ForMember(x => x.WalletLogs, opt => opt.MapFrom(x => x.WalletLogs))
        //    .ForMember(x => x.Transactions, opt => opt.Ignore())
        //    .ReverseMap();
        //#endregion




    }
}
