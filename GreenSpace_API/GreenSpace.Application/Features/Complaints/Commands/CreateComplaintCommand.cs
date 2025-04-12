using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Complaints;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Commands
{
    public class CreateComplaintCommand : IRequest<ComplaintViewModel>
    {
        public ComplaintCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateComplaintCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("userId  must not be null or empty");

                RuleFor(x => x.CreateModel.ComplaintType).NotNull().WithMessage("Type must not be empty");
                RuleFor(x => x.CreateModel.Reason).NotNull().NotEmpty().WithMessage("Reason  must not be null or empty");


            }
        }
        public class CommandHandler : IRequestHandler<CreateComplaintCommand, ComplaintViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }

            public async Task<ComplaintViewModel> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Complaint:\n");
                // Tạo mới Image
                var image = _mapper.Map<Image>(request.CreateModel.Image);
                image.Id = Guid.NewGuid();
                await _unitOfWork.ImageRepository.AddAsync(image);

                // Tạo mới complaint
                var complaint = _mapper.Map<Complaint>(request.CreateModel);
                complaint.Id = Guid.NewGuid();
                complaint.ImageId = image.Id;
                complaint.Status = (int)ComplaintStatusEnum.pending;
                complaint.ComplaintType = ((ComplaintTypeEnum)request.CreateModel.ComplaintType).ToString();


                foreach (var item in request.CreateModel.ComplaintDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new ApplicationException($"Product with ID {item.ProductId} not found");
                    }

                    var detail = new ComplaintDetail
                    {
                        Id = Guid.NewGuid(),
                        ComplaintId = complaint.Id,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        TotalPrice = product.Price * item.Quantity
                    };


                    complaint.ComplaintDetails.Add(detail);
                }


                await _unitOfWork.ComplaintRepository.AddAsync(complaint);
  
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                    throw new Exception($"Lỗi khi lưu dữ liệu: {innerExceptionMessage}", ex);
                }

                _logger.LogInformation("Complaint created successfully with ID: {Id}", complaint.Id);


                var ViewModel = _mapper.Map<ComplaintViewModel>(complaint);


                return ViewModel;

            }



        }
    }
}
