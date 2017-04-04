using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface IMoverState
    {
        string ToMove();
    }
    static class MoveRule
    {
        public static void AddMoveRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.NN))
                _rule[State.NN] = new StateConfigurator(State.NN);
            _rule[State.NN].Permit(Trigger._M, State.NM);
        }        
    }
}
