using ApprendreDotNet.model.Entities.kanban;
using ApprendreDotNet.model.Entities.user;
using ApprendreDotNet.services.user;
using Microsoft.EntityFrameworkCore;

namespace ApprendreDotNet.DbCore
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskHistoryEntity> TaskHistories { get; set; }
    }
}
