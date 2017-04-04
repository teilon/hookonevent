using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.status
{
    public class StateConfigurator
    {
        State _thatState;
        Dictionary<Trigger, State> _staterepresentation;
        public State CurrentTriget { get { return _thatState; } }
        public State this[Trigger trg] { get { return GetDestinationState(trg); } }
        public StateConfigurator(State state)
        {
            _thatState = state;
            _staterepresentation = new Dictionary<Trigger, State>();
        }
        public void Permit(Trigger trigger, State destinationState)
        {
            _staterepresentation[trigger] = destinationState;
        }
        public State GetDestinationState(Trigger trg)
        {
            if (_staterepresentation.ContainsKey(trg))
                return _staterepresentation[trg];
            return _thatState;
        }
    }
}
