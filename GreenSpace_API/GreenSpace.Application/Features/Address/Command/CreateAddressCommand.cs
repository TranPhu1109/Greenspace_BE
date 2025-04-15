using AutoMapper;
using GreenSpace.Application.ViewModels.Address;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace GreenSpace.Application.Features.Address.Command
{
    public class CreateAddressCommand : IRequest<AddressViewModel>
    {
        public CreateAddressModel CreateModel { get; set; } = default!;
        public class CommandHandler : IRequestHandler<CreateAddressCommand, AddressViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<CommandHandler> _logger;
            public CommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommandHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<AddressViewModel> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
            {
                var addressExist = await _unitOfWork.AddressRepository.WhereAsync(x => x.UserId == request.CreateModel.UserId);
                
                var address = _mapper.Map<Domain.Entities.Address>(request.CreateModel);
                address.Id = Guid.NewGuid();
                await _unitOfWork.AddressRepository.AddAsync(address);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AddressViewModel>(address);
            }

            public static class StringUtils
            {
                public static string NormalizeAddress(string input)
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return string.Empty;

                    input = input.ToLowerInvariant();
                    input = RemoveDiacritics(input);
                    input = Regex.Replace(input, @"[^\w\s]", " ");
                    input = Regex.Replace(input, @"\s+", " ").Trim();

                    return input;
                }

                private static string RemoveDiacritics(string text)
                {
                    var normalized = text.Normalize(NormalizationForm.FormD);
                    var builder = new StringBuilder();

                    foreach (var c in normalized)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                            builder.Append(c);
                    }

                    return builder.ToString().Normalize(NormalizationForm.FormC);
                }
            }
        }
    }
}
