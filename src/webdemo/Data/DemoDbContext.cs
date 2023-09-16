﻿namespace webdemo.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true);
        }

        public DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Type item in Assembly.GetEntryAssembly()!.GetTypes().Where(type => type.HasImplementedRawGeneric(typeof(IEntityTypeConfiguration<>))))
            {
                dynamic val = Activator.CreateInstance(item);
                modelBuilder.ApplyConfiguration(val);
            }
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync();
            return true;
        }
    }
}
