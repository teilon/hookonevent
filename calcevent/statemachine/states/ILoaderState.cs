using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    interface ILoaderState
    {
        //LoadRule LoadRule { get; }
        string OnLoad();
        string OnLoadingZone();
    }
    static class LoadRule
    {
        public static void AddLoadRules(ref Dictionary<State, StateConfigurator> _rule)
        {
            if (!_rule.ContainsKey(State.NZL))
                _rule[State.NZL] = new StateConfigurator(State.NZL);
            _rule[State.NZL].Permit(Trigger._L, State.LL);
            _rule[State.NZL].Permit(Trigger._M, State.NM);
            if (!_rule.ContainsKey(State.NM))
                _rule[State.NM] = new StateConfigurator(State.NM);
            _rule[State.NM].Permit(Trigger._Z, State.NZL);
            //
            if (!_rule.ContainsKey(State.UZL))
                _rule[State.UZL] = new StateConfigurator(State.UZL);
            _rule[State.UZL].Permit(Trigger._L, State.LL);
            _rule[State.UZL].Permit(Trigger._M, State.UM);
            if (!_rule.ContainsKey(State.UM))
                _rule[State.UM] = new StateConfigurator(State.UM);
            _rule[State.UM].Permit(Trigger._Z, State.UZL);
            //
            if (!_rule.ContainsKey(State.PZL))
                _rule[State.PZL] = new StateConfigurator(State.PZL);
            _rule[State.PZL].Permit(Trigger._L, State.LL);
            _rule[State.PZL].Permit(Trigger._M, State.PM);
            if (!_rule.ContainsKey(State.PM))
                _rule[State.PM] = new StateConfigurator(State.PM);
            _rule[State.PM].Permit(Trigger._Z, State.PZL);
            //
            if (!_rule.ContainsKey(State.LZL))
                _rule[State.LZL] = new StateConfigurator(State.LZL);
            _rule[State.LZL].Permit(Trigger._M, State.LM);
            if (!_rule.ContainsKey(State.LM))
                _rule[State.LM] = new StateConfigurator(State.LM);
            _rule[State.LM].Permit(Trigger._Z, State.LZL);
            //
            if (!_rule.ContainsKey(State.LZ))
                _rule[State.LZ] = new StateConfigurator(State.LZ);
            _rule[State.LZ].Permit(Trigger.L_, State.LM);
            _rule[State.LZ].Permit(Trigger._M, State.LM);
            _rule[State.LZ].Permit(Trigger._L, State.LL);
            if (!_rule.ContainsKey(State.LL))
                _rule[State.LL] = new StateConfigurator(State.LL);
            _rule[State.LL].Permit(Trigger._Z, State.LZ);
        }
    }
}
