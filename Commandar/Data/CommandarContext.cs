using Commandar.Models;
using Microsoft.EntityFrameworkCore;

namespace Commandar.Data
{
    public class CommandarContext: DbContext
    {
        public CommandarContext(DbContextOptions<CommandarContext> opt): base(opt)
        {

        }

        public DbSet<Command> Commands { get; set; }
    }
}
