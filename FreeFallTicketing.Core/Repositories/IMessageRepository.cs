using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        void MessageHasBeenSeen(Message message);
    }
}
