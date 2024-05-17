﻿using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.Repositories.GenericRepositories;
using EducationPortalApp.DataAccess.Repositories.Interfaces;
using EducationPortalApp.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationPortalApp.DataAccess.Repositories.Concrete
{
    public class CourseContentRepository : GenericRepository<CourseContent>, ICourseContentRepository
    {
        public CourseContentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<IEnumerable<CourseContent>> GetAllFilterAsync(Expression<Func<CourseContent, bool>> filter)
        {
            return await _appDbContext.Set<CourseContent>().Include(x => x.Course).ThenInclude(x => x.Category).Include(x => x.CourseContentType).Where(filter).ToListAsync();
        }

        public override async Task<IEnumerable<CourseContent>> GetAllAsync()
        {
            return await _appDbContext.Set<CourseContent>().Include(x => x.Course).ThenInclude(x => x.Category).Include(x => x.CourseContentType).ToListAsync();
        }

        public override async Task<CourseContent> GetByFilterAsync(Expression<Func<CourseContent, bool>> filter)
        {
            return await _appDbContext.Set<CourseContent>().Include(x => x.Course).ThenInclude(x => x.Category).Include(x => x.CourseContentType).Where(filter).SingleOrDefaultAsync();
        }
    }
}
