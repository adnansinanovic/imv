using Sinantrop.IMV.Db;

namespace Sinantrop.IMV.ViberLogic.Model
{
    public class ViberConversationStatistics : Entity<long>
    {
        public virtual long chatId { get; set; }

        public virtual string author { get; set; }

        public virtual ulong total_messages { get; set; }
    }
}
