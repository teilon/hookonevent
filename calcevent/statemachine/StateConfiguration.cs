using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    public enum State
    {
        LL, UU, PP, NN, OO,
        LM, UM, PM, NM, OM,
        ZU, UZ,
        LO, UO, PO, NO,
        UZL, PZL, NZL, LZL, LZ
    }

    public enum Trigger
    {
        N_, L_, U_, P_, _L, _U, _P,
        _M,
        _Z, Z_,
        _O, O_,
        ////
        _UX, _OX, _LX,
        UX_, OX_, LX_
    }
    public class StateInterface : IOutagerState, IMoverState
    {
        protected Dictionary<State, StateConfigurator> _rules;
        protected State _currentState;
        protected StateConfigurator CurrentState { get { return _rules[_currentState]; } }        
        
        public StateInterface()
        {
            _currentState = State.NN;
            _rules = new Dictionary<State, StateConfigurator>();

            MoveRule.AddMoveRules(ref _rules);
            OutageRule.AddOutageRules(ref _rules);
        }

        public string GetCurrentState()
        {
            switch (_currentState)
            {
                case State.UU: return "UU";
                case State.PP: return "PP";
                case State.PM: return "PM";
                case State.UM: return "UM";
                case State.LM: return "LM";
                case State.NM: return "NM";
                case State.LO:
                case State.UO:
                case State.PO:
                case State.NO:
                case State.OO: return "OO";
                case State.OM: return "OM";
                case State.UZL:
                case State.PZL:
                case State.NZL:
                case State.LZL:
                case State.LZ: return "LZ";
                case State.LL: return "LL";
                default: return "NN";
            }
        }

        public string ToMove()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._M);
            return GetCurrentState();
        }
        public string ToStop()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._O);
            return GetCurrentState();
        }
    }    
    public class TruckInterface : StateInterface, ILoaderState, IUnloaderState
    {
        public TruckInterface()
        {
            LoadRule.AddLoadRules(ref _rules);
            UnloadRule.AddUnloadRules(ref _rules);
        }
        public string OnLoad()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._L);
            return GetCurrentState();
        }

        public string OnLoadingZone()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._Z);
            return GetCurrentState();
        }

        public string OnUnload()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._U);
            return GetCurrentState();
        }

        public string OnUnloadingZone()
        {
            throw new NotImplementedException();
        }

        
    }
    public class ExcavatorInterface : StateInterface, ITheLoaderState
    {        
        public ExcavatorInterface()
        {
            TheLoadRule.AddLoadRules(ref _rules);
        }
        public string OnLoad()
        {
            _currentState = CurrentState.GetDestinationState(Trigger._L);
            return GetCurrentState();
        }
    }
}
