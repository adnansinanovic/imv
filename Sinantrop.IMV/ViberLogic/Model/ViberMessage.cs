namespace Sinantrop.IMV.ViberLogic.Model
{
    public class ViberMessage
    {
        public string Body { get; internal set; }
        public long ChatId { get; internal set; }
        public long EventId { get; internal set; }        
        public string Group { get; internal set; }
        public string Number { get; internal set; }
        public string PayloadPath { get; internal set; }        
        public long StickerId { get; internal set; }
        public string ThumbnailPath { get; internal set; }
        public long Timestamp { get; internal set; }

        public string Name { get; set; }

        public string ClientName { get; set; }
    }
}
