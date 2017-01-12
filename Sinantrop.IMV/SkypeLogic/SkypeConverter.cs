using System;
using System.Collections.Generic;
using Sinantrop.IMV.SkypeLogic.Model;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.SkypeLogic
{
    internal class SkypeConverter
    {
        public static Message GetMessage(SkypeMessage skypeMessage)
        {
            Message message = new Message();

            message.MessengerType = MessengerType.Skype;
            message.ConversationId = skypeMessage.convo_id.ToString();
            
            message.Identity = skypeMessage.author;
            message.Author = string.IsNullOrEmpty(skypeMessage.from_dispname) ? skypeMessage.author : skypeMessage.from_dispname;

            message.Id = skypeMessage.Id;
            message.Timestamp = skypeMessage.timestamp;

            message.Content = skypeMessage.body_xml;

            return message;
        }

        public static Conversation GetConversation(SkypeConversations skypeConversation)
        {
            Conversation conversation = new Conversation();
            conversation.MessengerType = MessengerType.Skype;            
            conversation.Id = skypeConversation.Id;

            if (!string.IsNullOrEmpty(skypeConversation.displayname))
                conversation.Title = skypeConversation.displayname;
            else if (!string.IsNullOrWhiteSpace(skypeConversation.given_displayname))
                conversation.Title = skypeConversation.given_displayname;
            else if (!string.IsNullOrWhiteSpace(skypeConversation.identity))
                conversation.Title = skypeConversation.identity;
            else
                conversation.Title = skypeConversation.Id.ToString();

            return conversation;
        }      
    }
}
