﻿using GreenSpace.Application.Repositories;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.Repositories
{
    public class DesignIdeasCategoryRepository : GenericRepository<DesignIdeasCategory>, IDesignIdeasCategoryRepository
    {
        public DesignIdeasCategoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }
    }
}
