using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.ComplaintDetail;
using GreenSpace.Application.ViewModels.Complaints;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Commands
{
    public class UpdateComplaintDetailCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ComplaintDetailsUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateComplaintDetailCommand>
        {
            public CommandValidation()
            {
               
            }
        }
        public class CommandHandler : IRequestHandler<UpdateComplaintDetailCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings,
                   IHubContext<SignalrHub> hubContext)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
                _hubContext = hubContext;
            }

            public async Task<bool> Handle(UpdateComplaintDetailCommand request, CancellationToken cancellationToken)
            {
                var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.Id, p => p.ComplaintDetails);

                if (complaint == null)
                    throw new ApplicationException("Complaint not found.");

                foreach (var detail in complaint.ComplaintDetails)
                {
                    var updatedDetail = request.UpdateModel.ComplaintDetails
                        .FirstOrDefault(x => x.ProductId == detail.ProductId);

                    if (updatedDetail != null)
                    {
                        detail.IsCheck = updatedDetail.IsCheck;
                        _unitOfWork.ComplaintDetailRepository.Update(detail);
                    }
                }

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateComplaint", $"{request.Id}");
                return result;
            }


        }
    }
}
