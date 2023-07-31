using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GroupMgr
{
    public Group[] groupHorizon = new Group[9];
    public Group[] groupVertual = new Group[9];
    public Group[] groupBlock = new Group[9];

    GameObject[] parents = new GameObject[3];

    Group GetGroup(GroupType groupType, int index)
    {
        switch(groupType)
        {
            case GroupType.blockGroup: return groupBlock[index];
            case GroupType.horizonGroup: return groupHorizon[index];
            case GroupType.vertualGroup: return groupVertual[index];
            default: throw new Exception("1");
        }
    }

    Group CreateGroup(GroupType groupType, int i)
    {
        GameObject o = new GameObject();
        o.transform.parent = parents[(int)groupType].transform;
        var group = o.AddComponent<Group>();
        group.Init(groupType, i);
        o.name = group.ToString();
        return group;
    }

    internal void Init(SlotList[] possibles)
    {
        for(int i = 0; i < possibles.Length; i++)
        {
            for(int j = 0; j < possibles[i]._list.Length; j++)
            {
                possibles[i][j].Init(i, j);
            }
        }

        parents[0] = new GameObject("horizon");
        parents[1] = new GameObject("vertual");
        parents[2] = new GameObject("block");

        for(int i = 0; i < 9; i++)
        {
            groupHorizon[i] = CreateGroup(GroupType.horizonGroup, i);
            groupVertual[i] = CreateGroup(GroupType.vertualGroup, i);
            groupBlock[i] = CreateGroup(GroupType.blockGroup, i);
        }

        for(int i = 0; i < 9; i++)
        {
            var horizonGroup = GetGroup(GroupType.horizonGroup, i);
            var vertualGroup = GetGroup(GroupType.vertualGroup, i);
            var blockGroup = GetGroup(GroupType.blockGroup, i);
            
            for(int j = 0; j < 9; j++)
            {
                var pH = possibles[i][j];
                horizonGroup.Add(pH);

                var pV = possibles[j][i];
                vertualGroup.Add(pV);

                var x1 = i % 3 * 3;
                var y1 = i / 3 * 3;
                var x2 = j % 3;
                var y2 = j / 3;
                var pB = possibles[y1 + y2][x1 + x2];
                blockGroup.Add(pB);

                // Debug.LogError($"{pB} {i} {j}");
            }
        }

        foreach(var x in possibles)
        {
            foreach(var y in x._list)
            {
                Assert.AreEqual(y.groups.Count, 3, $"{y} count > 3");
            }
        }

        var list = new List<Group>();
        list.AddRange(groupHorizon);
        list.AddRange(groupVertual);
        list.AddRange(groupBlock);
        foreach(var x in list)
        {
            Assert.AreEqual(x.possibleValues.Count, 9);
        }
    }

    internal bool IsWin()
    {
        return groupHorizon.All(m=>m.IsWin) && groupVertual.All(m=>m.IsWin) && groupBlock.All(m=>m.IsWin);
    }
}