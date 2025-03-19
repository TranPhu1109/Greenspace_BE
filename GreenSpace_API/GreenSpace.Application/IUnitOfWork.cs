﻿using AutoMapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.Repositories;

namespace GreenSpace.Application;
public interface IUnitOfWork
{
    IMapper Mapper { get; }
    IConnectionConfiguration DirectionConnection { get; }
    IUserRepository UserRepository { get; }
    Task<bool> SaveChangesAsync();
    IProductRepository ProductRepository { get; }
    IImageRepository ImageRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IDesignIdeaRepository DesignIdeaRepository { get; }
    IProductDetailRepository ProductDetailRepository { get; }
    IProductFeedbackRepository ProductFeedbackRepository { get; }
    IServiceFeedbackRepositoy ServiceFeedbackRepositoy { get; }
}