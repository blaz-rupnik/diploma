namespace Diploma_serverless.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Diploma_serverless.Models.UsersServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Diploma_serverless.Models.UsersServiceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Users.AddOrUpdate(x => x.Id, new User() { Id = Guid.Parse("5a57d8e2-d58f-4529-9288-c740fa9a9537"), Name = "Testni uporabnik", DateOfBirth = new DateTime(1996, 9, 4) });
            context.VacationLeaves.AddOrUpdate(x => x.Id, new VacationLeave()
            {
                Id = Guid.Parse("eecd9e3b-93c6-4d91-a638-6af5b4daed99"),
                UserId = Guid.Parse("5a57d8e2-d58f-4529-9288-c740fa9a9537"),
                DateFrom = new DateTime(2019, 4, 5),
                DateTo = new DateTime(2019, 4, 10)
            });
        }
    }
}
