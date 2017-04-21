using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface ITheLoaderState
    {
        string OnLoad();
    }
    static class TheLoadRule
    {
        public static void AddLoadRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.NO))
                _rule[State.NO] = new StateConfigurator(State.NO);
            _rule[State.NO].Permit(Trigger._L, State.LL);

            if (!_rule.ContainsKey(State.LL))
                _rule[State.LL] = new StateConfigurator(State.LL);
            _rule[State.LL].Permit(Trigger._O, State.NO);

            if (!_rule.ContainsKey(State.NN))
                _rule[State.NN] = new StateConfigurator(State.NN);
            _rule[State.NN].Permit(Trigger._L, State.LL);
        }
    }
}
