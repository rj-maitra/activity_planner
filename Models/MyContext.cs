using Microsoft.EntityFrameworkCore;
using ActivityPlanner.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ActivityPlanner.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options) {}
        public DbSet<User> AllUsers {get;set;}
        public DbSet<ActivityEvent> AllActivities {get;set;}
        public DbSet<Join> Joiners {get;set;}
        public User GetUserById(int UserId)
        {
            return AllUsers.FirstOrDefault(u => u.UserId == UserId);
        }
        public ActivityEvent GetActivityById(int ActId)
        {
            return AllActivities.Where(a => a.ActivityId == ActId).FirstOrDefault();
        }
        public void Remove(int ActId)
        {
            Remove(GetActivityById(ActId));
            SaveChanges();
        }
        public void Remove(int ActId, int PartId)
        {
            Join Joiner = Joiners
                .Where(a => a.ActivityId == ActId)
                .Where(p => p.UserId == PartId)
                .FirstOrDefault();
            Remove(Joiner);
            SaveChanges();
        }
    }
}

// one list in each table head
// join is the many to many third table