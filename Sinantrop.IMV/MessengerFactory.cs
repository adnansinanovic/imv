using System;
using Sinantrop.IMV.SkypeLogic;
using Sinantrop.IMV.SyncLogic;
using Sinantrop.IMV.ViberLogic;

namespace Sinantrop.IMV
{
    public static class MessengerFactory
    {
        public static IMessenger Create(MessengerType t)
        {
            switch (t)
            {
                case MessengerType.Skype:
                    return new Skype();                    
                case MessengerType.Viber:
                    return new Viber();                    
                case MessengerType.Sync:
                    return new Sync();                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }

        public static IMessengerUploader CreateUploader(MessengerType t)
        {
            switch (t)
            {
                case MessengerType.Skype:
                    return new Skype();
                case MessengerType.Viber:
                    return new Viber();
                case MessengerType.Sync:
                    //return new Sync();
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }
    }
}
