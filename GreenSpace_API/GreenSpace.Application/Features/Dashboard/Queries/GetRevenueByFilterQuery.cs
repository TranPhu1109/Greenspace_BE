using GreenSpace.Application.ViewModels.Report;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GreenSpace.Application.Features.Dashboard.Queries
{
    public class GetRevenueByFilterQuery : IRequest<ReportFillterViewModel>
    {
        public DateTime? Date { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        public class QueryHandler : IRequestHandler<GetRevenueByFilterQuery, ReportFillterViewModel>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ILogger<QueryHandler> logger;

            public QueryHandler(IUnitOfWork unitOfWork, ILogger<QueryHandler> logger)
            {
                this.unitOfWork = unitOfWork;
                this.logger = logger;
            }

            public async Task<ReportFillterViewModel> Handle(GetRevenueByFilterQuery request, CancellationToken cancellationToken)
            {
               
                var allBills = await unitOfWork.BillRepository
                    .WhereAsync(b => b.Price > 0);

                IEnumerable<Bill> filteredBills = allBills;

               
                if (request.Date.HasValue)
                {
                    var date = request.Date.Value.Date;
                    filteredBills = filteredBills.Where(b => b.CreationDate.Date == date);
                    logger.LogInformation(" doanh thu theo ngày: {date}", date);
                }
                else if (request.Month.HasValue && request.Year.HasValue)
                {
                    int month = request.Month.Value;
                    int year = request.Year.Value;
                    filteredBills = filteredBills.Where(b => b.CreationDate.Month == month && b.CreationDate.Year == year);
                    logger.LogInformation(" doanh thu theo tháng: {month}/{year}", month, year);
                }
                else if (request.Year.HasValue)
                {
                    int year = request.Year.Value;
                    filteredBills = filteredBills.Where(b => b.CreationDate.Year == year);
                    logger.LogInformation(" doanh thu theo năm: {year}", year);
                }

                decimal totalRevenue = filteredBills.Sum(b => b.Price);

                logger.LogInformation("Tổng doanh thu  : {revenue}", totalRevenue);

                return new ReportFillterViewModel
                {
                    Revenue = totalRevenue
                };
            }
        }
    }
}
