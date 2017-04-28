using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface IOutagerState
    {
        string ToStop();
    }
    static class OutageRule
    {
        public static void AddOutageRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.LM))
                _rule[State.LM] = new StateConfigurator(State.LM);
            _rule[State.LM].Permit(Trigger._O, State.LO);
            //
            if (!_rule.ContainsKey(State.UM))
                _rule[State.UM] = new StateConfigurator(State.UM);
            _rule[State.UM].Permit(Trigger._O, State.UO);
            //
            if (!_rule.ContainsKey(State.PM))
                _rule[State.PM] = new StateConfigurator(State.PM);
            _rule[State.PM].Permit(Trigger._O, State.PO);
            //
            if (!_rule.ContainsKey(State.NM))
                _rule[State.NM] = new StateConfigurator(State.NM);
            _rule[State.NM].Permit(Trigger._O, State.NO);
            //
            if (!_rule.ContainsKey(State.LO))
                _rule[State.LO] = new StateConfigurator(State.LO);
            //_rule[State.LO].Permit(Trigger.L_, State.LM);
            _rule[State.LO].Permit(Trigger._M, State.LM);
            //
            if (!_rule.ContainsKey(State.UO))
                _rule[State.UO] = new StateConfigurator(State.UO);
            //_rule[State.UO].Permit(Trigger.U_, State.UM);
            _rule[State.UO].Permit(Trigger._M, State.UM);
            //
            if (!_rule.ContainsKey(State.PO))
                _rule[State.PO] = new StateConfigurator(State.PO);
            //_rule[State.PO].Permit(Trigger.P_, State.PM);
            _rule[State.PO].Permit(Trigger._M, State.PM);
            //
            if (!_rule.ContainsKey(State.NO))
                _rule[State.NO] = new StateConfigurator(State.NO);
            //_rule[State.NO].Permit(Trigger.N_, State.NM);
            _rule[State.NO].Permit(Trigger._M, State.NM);
            //
        }
    }
}
