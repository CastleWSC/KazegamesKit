using System;
using UnityEngine;

namespace KazegamesKit.Touch
{
    public class DragRecognizer : GestureRecognizer
    {
        public event Action<DragRecognizer> onBegin;
        public event Action<DragRecognizer> onDrag;
        public event Action<DragRecognizer> onEnded;

        private float _startTime;

        public Vector2 position { get; private set; }
        public float ElpasedTime
        {
            get { return Time.realtimeSinceStartup - _startTime; }
        }

        public DragRecognizer()
        {

        }

        public override void OnTriggerEvents()
        {
            onEnded?.Invoke(this);
        }

        public override bool OnTouchBegan(Array<UTouch> touches)
        {
            if(state == EState.Ready)
            {
                UTouch touch = touches[0];
                
                _tracking.Push(touch);

                _startTime = Time.realtimeSinceStartup;
                position = touch.position;

                onBegin?.Invoke(this);
                state = EState.Began;

                return true;
            }

            return false;
        }

        public override void OnTouchMoved(Array<UTouch> touches)
        {
            if(state == EState.Began)
            {
                position = touches[0].position;

                onDrag?.Invoke(this);
            }
        }

        public override void OnTouchEnded(Array<UTouch> touches)
        {
            if(state == EState.Began)
            {
                position = touches[0].position;

                state = EState.Recognized;
            }
        }
    }
}