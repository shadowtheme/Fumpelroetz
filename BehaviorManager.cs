
using System;
using System.Collections.Generic;

namespace EscapeRoomControlPanel
{
    public class BehaviorManager
    {
        private List<Behavior> behaviors;

        public BehaviorManager()
        {
            behaviors = new List<Behavior>();
        }

        public void AddBehavior(Behavior behavior)
        {
            behaviors.Add(behavior);
        }

        public List<Behavior> GetBehaviors()
        {
            return behaviors;
        }
    }
}
