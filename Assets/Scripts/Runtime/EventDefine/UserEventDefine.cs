using UniFramework.Event;

public class UserEventDefine
{
    public class UserTryInitailize : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryInitailize();
            UniEvent.SendMessage(msg);
        }
    }
}
