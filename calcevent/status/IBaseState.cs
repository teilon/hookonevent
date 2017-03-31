using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface IBaseState
    {
        string ToMove();
        string ToStop();
    }
    static class BaseRule
    {
        public static void AddBaseRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.NN))
                _rule[State.NN] = new StateConfigurator(State.NN);
            _rule[State.NN].Permit(Trigger._M, State.NM);
            //
            if (!_rule.ContainsKey(State.NN))
                _rule[State.NN] = new StateConfigurator(State.NN);
            _rule[State.NN].Permit(Trigger._O, State.NO);
            //
            if (!_rule.ContainsKey(State.NM))
                _rule[State.NM] = new StateConfigurator(State.NM);
            _rule[State.NM].Permit(Trigger._O, State.NO);
            //
            if (!_rule.ContainsKey(State.NO))
                _rule[State.NO] = new StateConfigurator(State.NO);
            _rule[State.NO].Permit(Trigger._M, State.NM);            
        }
    }
}
