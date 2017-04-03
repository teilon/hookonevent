﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface IUnloaderState
    {
        string OnUnload();
        string OnUnloadingZone();
    }
    static class UnloadRule
    {
        public static void AddUnloadRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.NN))
                _rule[State.NN] = new StateConfigurator(State.NN);
            _rule[State.NN].Permit(Trigger.U_, State.UM);
            //
            if (!_rule.ContainsKey(State.LM))
                _rule[State.LM] = new StateConfigurator(State.LM);
            _rule[State.LM].Permit(Trigger._U, State.UU);
            //
            if (!_rule.ContainsKey(State.NM))
                _rule[State.NM] = new StateConfigurator(State.NM);
            _rule[State.NM].Permit(Trigger._U, State.UU);
        }
    }
}
