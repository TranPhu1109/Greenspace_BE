using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteProductCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteProductCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork, IHubContext<SignalrHub> hubContext)
            {
               _unitOfWork = unitOfWork;
                _hubContext = hubContext;
            }

            public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
                if (product is null) throw new NotFoundException($"Product with Id-{request.Id} is not exist!");
                _unitOfWork.ProductRepository.SoftRemove(product);

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateProduct", $"{request.Id}");
                return result;
            }
        }
    }
}
