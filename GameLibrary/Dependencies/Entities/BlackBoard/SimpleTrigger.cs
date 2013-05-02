using System;

namespace GameLibrary.Dependencies.Entities
{
    public class SimpleTrigger : Trigger
    {
        private Func<BlackBoard, TriggerState, bool> Condition;
        private Action<TriggerState> onFire;

        public SimpleTrigger(String Name, Func<BlackBoard, TriggerState, bool> Condition, Action<TriggerState> onFire)
        {
            this.WorldPropertiesMonitored.Add(Name);
            this.Condition = Condition;
            this.onFire = onFire;
        }

        protected override bool CheckConditionToFire()
        {
            return Condition(BlackBoard, TriggerState);
        }

        protected override void CalledOnFire(TriggerState TriggerState)
        {
            if (onFire != null)
                onFire(TriggerState);
        }
    }
}