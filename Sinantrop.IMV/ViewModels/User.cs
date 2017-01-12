namespace Sinantrop.IMV.ViewModels
{
    public class UiUser
    {
        public string Username { get; set; }

        public MessengerType Messenger { get; set; }

        public UiUser(string username, MessengerType messengerType)
        {
            Username = username;
            Messenger = messengerType;
        }              

        public string Display
        {
            get
            {
                string username = Username;
                string prefix = GetDisplayPrefix();               

                if (Messenger == MessengerType.Viber)
                {
                    ulong phone;

                    if (ulong.TryParse(username, out phone))
                    {
                        string phoneFormat = "{0:+###-##-###-###}";

                        try
                        {                            
                            username = string.Format(phoneFormat, phone);
                        }
                        catch
                        {
                            username = Username;
                        }                        
                    }                                        
                }

            
                return $"{prefix} - {username}";
            }
        }
     
        private string GetDisplayPrefix()
        {
            switch (Messenger)
            {
                case MessengerType.Skype:
                    return "Skype";
                case MessengerType.Viber:
                    return "Viber";                    
                case MessengerType.Sync:
                    return "Sync";                    
                default:
                    return Messenger.ToString();
            }            
        }
    }
}
