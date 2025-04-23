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

namespace GreenSpace.Application.Features.DesignIdeas.Commands
{
    public class DeleteDesignIdeaCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteDesignIdeaCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteDesignIdeaCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork, IHubContext<SignalrHub> hubContext)
            {
                _unitOfWork = unitOfWork;
                _hubContext = hubContext;
            }

            public async Task<bool> Handle(DeleteDesignIdeaCommand request, CancellationToken cancellationToken)
            {
                var design = await _unitOfWork.DesignIdeaRepository.GetByIdAsync(request.Id);
                if (design is null) throw new NotFoundException($"DesignIdea with Id-{request.Id} is not exist!");
                _unitOfWork.DesignIdeaRepository.SoftRemove(design);
                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateDesignIdea", $"{request.Id}");
                return result;
            }
        }
    }
}
