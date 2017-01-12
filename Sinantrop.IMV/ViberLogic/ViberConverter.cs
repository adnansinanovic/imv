using Sinantrop.IMV.ViberLogic.Model;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.ViberLogic
{
    internal class ViberConverter
    {
        public static Message GetMessage(ViberMessage item)
        {
            Message m = new Message();
            m.MessengerType = MessengerType.Viber;

            m.Id = item.EventId;
            m.ConversationId = item.ChatId.ToString();
            m.Timestamp = item.Timestamp;
            m.Identity = item.Number;

            //set author
            string author = string.Empty;

            if (!string.IsNullOrEmpty(item.Name))
                author = item.Name;

            // Note: In viber database, empty value is marked with string '<EmptyValue>.'
            if (item.ClientName != "<EmptyValue>." && !string.IsNullOrEmpty(item.ClientName))
                author = string.IsNullOrEmpty(author) ? item.ClientName : $"{author} {item.ClientName}";

            if (string.IsNullOrEmpty(author))
                author = item.Number;

            m.Author = author;

            //set message text
            string content = string.Empty;

            if (!string.IsNullOrEmpty(item.Body))
                m.Content = item.Body;
            else if (!string.IsNullOrEmpty(item.PayloadPath))
                m.Content = $"<<PAYLOAD: {item.PayloadPath} >>";
            else if (!string.IsNullOrEmpty(item.ThumbnailPath))
                m.Content = $"<<THUMBNAIL: {item.ThumbnailPath} >>";
            else if (item.StickerId != 0)
                m.Content = $"<<Sticker: {item.ThumbnailPath} >>";
            else
                m.Content = $"<<UNKNOWN ACTION: {item.EventId} >>";

            return m;
        }
    }
}
