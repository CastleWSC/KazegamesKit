using System;
using KazegamesKit.Collections;
using UnityEngine;

namespace KazegamesKit.Touch
{
    public class TouchRecognizer : GestureRecognizer
    {
        public event Action<TouchRecognizer> onBegin;
        public event Action<TouchRecognizer> onMoved;
        public event Action<TouchRecognizer> onStay;
        public event Action<TouchRecognizer> onEnded;


        public int numOfTouchesRequired = 1;

        private float _touchBeganTime = 0;

        public Vector2 position { get; private set; }
        public float ElpasedTime { get; private set; }


        public TouchRecognizer() : this(1)
        {

        }

        public TouchRecognizer(int touchesRequired)
        {
            numOfTouchesRequired = touchesRequired;
        }

        public override void OnTriggerEvents()
        {
            onEnded?.Invoke(this);
        }

        public override bool OnTouchBegan(Array<UTouch> touches)
        {
            if(state == EState.Ready)
            {
                for(int i=0; i<touches.Length; i++)
                {
                    if(touches[i].phase == TouchPhase.Began)
                    {
                        _tracking.Push(touches[i]);

                        if (_tracking.Length == numOfTouchesRequired)
                            break;
                    }
                }

                if(_tracking.Length == numOfTouchesRequired)
                {
                    _touchBeganTime = Time.realtimeSinceStartup;

                    position = TouchLocation();
                    ElpasedTime = 0;

                    onBegin?.Invoke(this);

                    state = EState.Began;
                }
            }

            return false;
        }

        public override void OnTouchMoved(Array<UTouch> touches)
        {
            if (state == EState.Began)
            {
                position = TouchLocation();
                ElpasedTime = Time.realtimeSinceStartup - _touchBeganTime;

                onMoved?.Invoke(this);
            }
        }

        public override void OnTouchStay(Array<UTouch> touches)
        {
            if (state == EState.Began)
            {
                position = TouchLocation();
                ElpasedTime = Time.realtimeSinceStartup - _touchBeganTime;

                onStay?.Invoke(this);
            }
        }

        public override void OnTouchEnded(Array<UTouch> touches)
        {
            if(state == EState.Began)
            {
                position = TouchLocation();
                ElpasedTime = Time.realtimeSinceStartup - _touchBeganTime;

                state = EState.Recognized;
            }
        }
    }
}
