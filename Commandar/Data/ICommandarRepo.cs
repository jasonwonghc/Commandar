using Commandar.Models;
using System.Collections.Generic;

namespace Commandar.Data
{
    public interface ICommandarRepo
    {
        bool SaveChanges();

        IEnumerable<Command> GetAllCommand();
        Command GetCommandById(int Id);
        void CreateCommand(Command cmd);
        void UpdateCommand(Command cmd);
        void DeleteCommand(Command cmd);
    }
}
