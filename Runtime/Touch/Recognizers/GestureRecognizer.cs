using System;
using UnityEngine;

namespace KazegamesKit.Touch
{
    public abstract class GestureRecognizer : IComparable<GestureRecognizer>
    {
        public enum EState
        {
            Ready,
            Began,
            FailedOrEnded,
            Recognizing,
            Recognized
        }

        protected Array<UTouch> _tracking;

        private EState _state;
        public EState state
        {
            get { return _state; }

            set
            {
                _state = value;

                if (_state == EState.Recognized || _state == EState.Recognizing)
                    OnTriggerEvents();

                if (_state == EState.Recognized || _state == EState.FailedOrEnded)
                    Reset();
            }
        }

        public UTouch PrimaryTouch
        {
            get
            {
                if(_tracking != null && _tracking.Length > 0)
                {
                    for (int i = 0; i < _tracking.Length; i++)
                        if (_tracking[i].Id == 0) 
                            return _tracking[i];
                }

                return null;
            }
        }


        public virtual uint Order { get { return 0; } }

        public GestureRecognizer()
        {
            Reset();
        }

        public Vector2 TouchLocation()
        {
            float x = 0;
            float y = 0;
            float k = 0;

            for (int i = 0; i < _tracking.Length; i++)
            {
                x += _tracking[i].position.x;
                y += _tracking[i].position.y;
                k++;
            }

            return (k > 0) ? new Vector2(x / k, y / k) : Vector2.zero;
        }

        internal void Reset()
        {
            _state = EState.Ready;
            if (_tracking == null)
            {
                _tracking = new Array<UTouch>(1);
            }
            else
            {
                if(!_tracking.IsEmpty())
                    _tracking.Erase(0, _tracking.Length - 1);
            }
        }

        internal void Recognize(Array<UTouch> touches)
        {
            for(int i=touches.Length-1; i>=0; i--)
            {
                UTouch touch = touches[i];
                switch(touch.phase)
                {
                    case TouchPhase.Began:
                        if(OnTouchBegan(touches) && this.Order > 0)
                        {
                            int removedTouches = 0;
                            for(int j=touches.Length-1; j>=0; j--)
                            {
                                if(touches[j].phase == TouchPhase.Began)
                                {
                                    touches.Erase(j);
                                    removedTouches++;
                                }
                            }

                            if(removedTouches > 0)
                            {
                                i -= (removedTouches - 1);
                            }
                        }
                        break;

                    case TouchPhase.Moved:
                        OnTouchMoved(touches);
                        break;

                    case TouchPhase.Stationary:
                        OnTouchStay(touches);
                        break;

                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        OnTouchEnded(touches);
                        break;
                }
            }
        }

        public abstract void OnTriggerEvents();
        public virtual bool OnTouchBegan(Array<UTouch> touches) { return false; }
        public virtual void OnTouchMoved(Array<UTouch> touches) { }
        public virtual void OnTouchStay(Array<UTouch> touches) { }
        public virtual void OnTouchEnded(Array<UTouch> touches) { }


        public int CompareTo(GestureRecognizer other)
        {
            return this.Order.CompareTo(other.Order);
        }
    }
}
