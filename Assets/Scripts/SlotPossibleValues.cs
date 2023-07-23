using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPossibleValues : MonoBehaviour
{
    public int x1;
    public int y1;

    public override string ToString()
    {
        return $"{x1} {y1}";
    }

    public int currentValue;
    public bool isInput;

    public List<int> values = new List<int>(){
        1, 2, 3, 4, 5, 6, 7, 8, 9
    };

    public List<Group> groups = new List<Group>();

    public void SetValue(int value, bool isInput = false)
    {
        if(currentValue != -1)
        {
            if(currentValue != value)
            {
                Debug.LogError($"inner Error {currentValue} {value}");
            }

            return;
        }

        values.Clear();
        values.Add(value);

        currentValue = value;
        this.isInput = isInput;
        // ShowItem(value.ToString());

        foreach(var x in groups)
        {
            x.OnSetValue(this);
        }
    }

    internal void Remove(int value)
    {
        values.Remove(value);

        foreach(var x in groups)
        {
            x.OnPossibleChange(this);
        }
    }

    internal void Init(int x, int y)
    {
        this.x1 = x;
        this.y1 = y;
        this.currentValue = -1;

        values = new List<int>(){
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        groups = new List<Group>();
    }
}
