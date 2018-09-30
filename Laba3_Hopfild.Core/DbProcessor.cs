using System.Data.Entity;

namespace Laba3_Hopfild.Core
{
    public class DbProcessor: DbContext
    {
        public DbProcessor() : base("DefaultConnection")
        {
            Database.SetInitializer(new DbInitializer());
        }

        public DbSet<WeightModel> Weights { get; set; }
    }

    class DbInitializer: DropCreateDatabaseAlways<DbProcessor>
    {
        protected override void Seed(DbProcessor context)
        {
            var val = new WeightModel()
            {
                Index = 0,
                Value = 0.0f
            };

            context.Weights.Add(val);
            context.SaveChanges();
        }
    }
}
