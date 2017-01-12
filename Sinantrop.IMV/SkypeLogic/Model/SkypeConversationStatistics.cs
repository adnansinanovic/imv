using Sinantrop.IMV.Db;

namespace Sinantrop.IMV.SkypeLogic.Model
{
    public class SkypeConversationStatistics : Entity<long>
    {
        public virtual long convo_id { get; set; }

        public virtual string author { get; set; }

        public virtual ulong total_messages { get; set; }
    }
}
