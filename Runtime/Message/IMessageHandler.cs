using System;

namespace KazegamesKit
{
    public abstract class MessageData
    { 
        public Type MsgType { get { return GetType(); } }
    }

    public interface IMessageHandler
    {
        void OnMessage(MessageData msg);
    }
}
