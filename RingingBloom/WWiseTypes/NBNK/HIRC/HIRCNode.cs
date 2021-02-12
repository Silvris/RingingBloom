using RingingBloom.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RingingBloom.NBNK
{
    public class HIRCNode : IEnumerable<HIRCNode>
    {
        HIRCTypes eHircType { get; set; }
        uint dwSectionSize { get; set; }
        uint ulID { get; set; }
        List<HIRCNode> children;
        IEnumerator<HIRCNode> IEnumerable<HIRCNode>.GetEnumerator()
        {
            return (IEnumerator<HIRCNode>)GetEnumerator();
        }

        public HIRCNodeEnum GetEnumerator()
        {
            return new HIRCNodeEnum(children);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(HIRCNode[] childs)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                children.Add(childs[i]);
            }
        }
    }
    public class HIRCNodeEnum : IEnumerator
    {
        public List<HIRCNode> nodes;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public HIRCNodeEnum(List<HIRCNode> list)
        {
            nodes = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < nodes.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public HIRCNode Current
        {
            get
            {
                try
                {
                    return nodes[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
