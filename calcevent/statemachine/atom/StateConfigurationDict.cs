using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using calcevent.status;

namespace calcevent.statemachine.atom
{
    class StateConfigurationDict
    {
        
    }
    /*
    class Rule : IDictionary<State, RuleSet>
    {

    }
    */
    class RuleSet : ICollection<KeyValuePair<Trigger, State>>
    {
        int _count = 0;
        bool _isreadonly = true;
        public int Count { get { return _count; } }
        public bool IsReadOnly { get { return _isreadonly; } }

        public void Add(KeyValuePair<Trigger, State> item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<Trigger, State> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<Trigger, State>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<Trigger, State>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Trigger, State> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
