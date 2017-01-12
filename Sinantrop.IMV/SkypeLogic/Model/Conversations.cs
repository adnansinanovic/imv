using Sinantrop.IMV.Db;
using SQLite;

namespace Sinantrop.IMV.SkypeLogic.Model
{   
    public class SkypeConversations
    {
        [PrimaryKey]
        public virtual long Id { get; set; }
        public virtual string identity { get; set; }

        public virtual string given_displayname { get; set; }

        public virtual string displayname { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(this.given_displayname))
                return this.given_displayname;
            if (!string.IsNullOrWhiteSpace(this.displayname))
                return this.displayname;
            if (!string.IsNullOrWhiteSpace(this.identity))
                return this.identity;
            return this.Id.ToString();
        }
    }
}
