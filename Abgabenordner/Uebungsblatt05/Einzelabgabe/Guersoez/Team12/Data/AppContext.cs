using Microsoft.EntityFrameworkCore;
namespace Team12.Data
{
    public class AppContext : DbContext
    {
        public AppContext() { }
        public AppContext(DbContextOptions<AppContext> options) :base(options)
        { }
        }
}
