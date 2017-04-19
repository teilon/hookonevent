using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.statemachine.atom
{
    public enum TypeState { S, M }

    public class DumpState
    {
        Dictionary<string, string> _args;
        protected TypeState _state;
        public DumpState()
        {
            _args = new Dictionary<string, string>();
        }
    }
    public class StandState : DumpState
    {
        public StandState()
        {
            _state = TypeState.S;
        }
    }
    public class MoveState : DumpState
    {
        public MoveState()
        {
            _state = TypeState.M;
        }
    }
}
