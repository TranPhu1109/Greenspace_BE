using MediatR;
using Microsoft.Extensions.Logging;
using GreenSpace.Application.ViewModels;
using GreenSpace.Domain.Entities;
using GreenSpace.Application.ViewModels.Report;

namespace GreenSpace.Application.Features.Dashboard.Queries
{
    public class GetAllRevenueQuery : IRequest<ReportViewModel>
    {
        public class QueryHandler : IRequestHandler<GetAllRevenueQuery, ReportViewModel>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ILogger<GetAllRevenueQuery> logger;
            private readonly IMediator mediator;

            public QueryHandler(IUnitOfWork unitOfWork,
                ILogger<GetAllRevenueQuery> logger,
                IMediator mediator)
            {
                this.mediator = mediator;
                this.unitOfWork = unitOfWork;
                this.logger = logger;
            }

            public async Task<ReportViewModel> Handle(GetAllRevenueQuery request, CancellationToken cancellationToken)
            {
                var bills = await unitOfWork.BillRepository.GetAllAsync();

                DateTime today = DateTime.Today;

                // 1. Doanh thu hôm nay
                var dailyRevenue = bills
                    .Where(b => b.CreationDate.Date == today)
                    .Sum(b => b.Price);

                // 2. Doanh thu tuần này (Thứ 2 - Chủ nhật)
                var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                var sunday = monday.AddDays(6);
                var weeklyRevenue = bills
                    .Where(b => b.CreationDate.Date >= monday && b.CreationDate.Date <= sunday)
                    .Sum(b => b.Price);

                // 3. Doanh thu tháng này
                var monthlyRevenue = bills
                    .Where(b => b.CreationDate.Month == today.Month && b.CreationDate.Year == today.Year)
                    .Sum(b => b.Price);

                // 4. Doanh thu năm nay
                var yearlyRevenue = bills
                    .Where(b => b.CreationDate.Year == today.Year)
                    .Sum(b => b.Price);

                logger.LogInformation("Doanh thu hôm nay: {Revenue}", dailyRevenue);
                logger.LogInformation("Doanh thu tuần này: {Revenue}", weeklyRevenue);
                logger.LogInformation("Doanh thu tháng này: {Revenue}", monthlyRevenue);
                logger.LogInformation("Doanh thu năm nay: {Revenue}", yearlyRevenue);

                return new ReportViewModel
                {
                    DailyRevenue = dailyRevenue,
                    WeeklyRevenue = weeklyRevenue,
                    MonthlyRevenue = monthlyRevenue,
                    YearlyRevenue = yearlyRevenue
                };
            }
        }
    }
}
