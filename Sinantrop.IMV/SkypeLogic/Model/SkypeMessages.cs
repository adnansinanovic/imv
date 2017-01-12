using System;
using Sinantrop.IMV.Db;

namespace Sinantrop.IMV.SkypeLogic.Model
{
    public class SkypeMessage : Entity<long>
    {
        public virtual long is_permanent { get; set; }

        public virtual long convo_id { get; set; }

        public virtual string chatname { get; set; }

        public virtual string author { get; set; }

        public virtual string from_dispname { get; set; }

        public virtual long author_was_live { get; set; }

        public virtual string dialog_partner { get; set; }

        public virtual long timestamp { get; set; }

        public virtual long type { get; set; }

        public virtual long sending_status { get; set; }

        public virtual long consumption_status { get; set; }

        public virtual string edited_by { get; set; }

        public virtual long edited_timestamp { get; set; }

        public virtual long param_key { get; set; }

        public virtual long param_value { get; set; }

        public virtual string body_xml { get; set; }

        public virtual string identities { get; set; }

        public virtual string reason { get; set; }

        public virtual long leavereason { get; set; }

        public virtual long participant_count { get; set; }

        public virtual long error_code { get; set; }

        public virtual long chatmsg_type { get; set; }

        public virtual long chatmsg_status { get; set; }

        public virtual long body_is_rawxml { get; set; }

        public virtual long oldoptions { get; set; }

        public virtual long newoptions { get; set; }

        public virtual long newrole { get; set; }

        public virtual long pk_id { get; set; }

        public virtual long crc { get; set; }

        public virtual long remote_id { get; set; }

        public virtual string call_guid { get; set; }

        public virtual string extprop_contact_review_date { get; set; }

        public virtual long extprop_contact_received_stamp { get; set; }

        public virtual long extprop_contact_reviewed { get; set; }

        public DateTime Date
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)this.timestamp).ToLocalTime();
            }
        }        
    }
}
