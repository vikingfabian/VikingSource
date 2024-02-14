using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters.AI
{
    class SelectRandomState
    {
        List<AIstate> states;
        int totalCommoness;

        public SelectRandomState()
        {
            states = new List<AIstate>();
        }
        public SelectRandomState(List<AIstate> states)
        {
            this.states = states;
            foreach (AIstate s in states)
            {
                totalCommoness += s.Commoness;
            }
        }
        public void AddState(AIstate state)
        {
            states.Add(state);
            totalCommoness += state.Commoness;
        }
        public void AddState(int state, int commoness)
        {
            this.AddState(new AIstate(state, commoness));
        }
        public AIstate GetRandom()
        {
            if (states.Count == 1) return states[0];

            int rnd = Ref.rnd.Int(totalCommoness);
            foreach (AIstate s in states)
            {
                if (rnd < s.Commoness)
                    return s;
                rnd -= s.Commoness;
            }
            throw new Exception();
        }

        public int GetRandomAndRunEvent(ref float stateTime)
        {
            AI.AIstate s = GetRandom();
            stateTime = s.StateTime.GetRandom();
            s.RunEvent();
            return s.State;
        }
    }
    struct AIstate
    {
        public int State;
        public int Commoness;
        public IntervalF StateTime;
        public Action StateEvent;

        public AIstate(int state, int commoness)
        {
            this.State = state;
            this.Commoness = commoness;
            this.StateTime = IntervalF.Zero;
            StateEvent = null;
        }
        public AIstate(int state, int commoness, IntervalF stateTime, Action stateEvent)
        {
            this.State = state;
            this.Commoness = commoness;
            this.StateTime = stateTime;
            this.StateEvent = stateEvent;
        }
        public void RunEvent()
        {
            if (StateEvent != null) StateEvent();
        }
    }

}
