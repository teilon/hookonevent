using System;
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
            _rule[State.NN].Permit(Trigger._U, State.UU);
            //
            if (!_rule.ContainsKey(State.LM))
                _rule[State.LM] = new StateConfigurator(State.LM);
            _rule[State.LM].Permit(Trigger._U, State.UU);
            //
            if (!_rule.ContainsKey(State.NM))
                _rule[State.NM] = new StateConfigurator(State.NM);
            _rule[State.NM].Permit(Trigger._U, State.UU);

            if (!_rule.ContainsKey(State.UU))
                _rule[State.UU] = new StateConfigurator(State.UU);
            _rule[State.UU].Permit(Trigger._M, State.UM);
            //
            //
            //temp
            //if (!_rule.ContainsKey(State.LZ))
            //    _rule[State.LZ] = new StateConfigurator(State.LZ);
            //_rule[State.LZ].Permit(Trigger._U, State.UU);
        }
    }
}
