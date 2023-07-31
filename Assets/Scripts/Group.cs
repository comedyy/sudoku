using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GroupType
{
    horizonGroup,
    vertualGroup,
    blockGroup
}

public class Group : MonoBehaviour
{
    public List<SlotPossibleValues> possibleValues = new List<SlotPossibleValues>();
    public List<int> remains = new List<int>(){1, 2, 3, 4, 5, 6,7, 8, 9};
    public GroupType groupType;
    private int index;

    public bool IsWin => remains.Count == 0;

    public override string ToString()
    {
        return $"{groupType} {index}";
    }

    public void Add(SlotPossibleValues v)
    {
        possibleValues.Add(v);
        v.groups.Add(this);
    }

    internal void OnSetValue(SlotPossibleValues v)
    {
        remains.Remove(v.currentValue);
        foreach(var x in possibleValues)
        {
            if(x.currentValue != -1)
            {
                continue;
            }

            x.Remove(v.currentValue);

            if(x.values.Count == 1)
            {
                x.SetValue(x.values[0]);
            }
        }
    }

    internal void OnPossibleChange(SlotPossibleValues v)
    {
        foreach(var x in remains)
        {
            var count = 0;
            SlotPossibleValues p = null;
            foreach(var y in possibleValues)
            {
                if(y.values.Contains(x))
                {
                    p = y;
                    count ++;
                }
            }

            if(count == 1)
            {
                p.SetValue(x);
                break;
            }
        }
    }

    internal void Init(GroupType groupType, int i)
    {
        this.groupType = groupType;
        this.index = i;
        possibleValues = new List<SlotPossibleValues>();
        remains = new List<int>(){1, 2, 3, 4, 5, 6,7, 8, 9};
    }
}
