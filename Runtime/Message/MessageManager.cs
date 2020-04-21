using System;
using System.Collections.Generic;

namespace KazegamesKit
{
    public class MessageManager : Singleton
    {
        Dictionary<Type, Array<IMessageHandler>> _msgTable;

        public void RegisterHandler<T>(IMessageHandler handler) where T : MessageData
        {
            Array<IMessageHandler> handlers;
            if(!_msgTable.TryGetValue(typeof(T), out handlers))
            {
                handlers = new Array<IMessageHandler>(6);
            }

            handlers.Push(handler);
        }

        public void UnRegisterHandler<T>(IMessageHandler handler) where T : MessageData
        {
            Array<IMessageHandler> handlers;
            if (_msgTable.TryGetValue(typeof(T), out handlers))
            {
                handlers.Remove(handler);

                if (handlers.IsEmpty())
                    _msgTable.Remove(typeof(T));
            }
        }

        public void ClearMessage<T>() where T : MessageData
        {
            if(_msgTable.ContainsKey(typeof(T)))
            {
                _msgTable.Remove(typeof(T));
            }
        }

        public void ClearAllMessages()
        {
            _msgTable.Clear();
        }

        public void TriggerMessage(MessageData msg)
        {
            Array<IMessageHandler> handlers = null;
            if(_msgTable.TryGetValue(msg.MsgType, out handlers))
            {
                for (int i = 0; i < handlers.Length; i++)
                    handlers[i].OnMessage(msg);
            }
        }

        public override void Init()
        {
            _msgTable = new Dictionary<Type, Array<IMessageHandler>>();
        }

        public override void Dispose()
        {
            _msgTable = null;
        }
    }
}