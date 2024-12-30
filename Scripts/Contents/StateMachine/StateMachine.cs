using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]    
    Define.State eState;

    public Define.State EState {  get { return eState; } }

    [SerializeField]
    BaseState tempState;

    public BaseState TempState { get { return tempState; } }

    BaseState curState;

    public BaseState CurState {  get { return curState; } }

    Dictionary<Define.State, BaseState> stateDic = new Dictionary<Define.State, BaseState>();

    protected int skillCount = 0;

    List<Define.State> skillList = new List<Define.State>();

    public void DefaultState(Define.State eState, BaseState state)
    {
        tempState = state;
        AddState(eState, state);
        ChangeState(eState, true);
    }

    public bool ChangeState(Define.State eState, bool ignoreCondition = false)
    {
        if (stateDic.TryGetValue(eState, out BaseState state) == false)
            return false;
        
        if(!ignoreCondition && state.CheckCondition() == false)
            return false;

        if (curState != null)
            curState.ExitState();
        tempState = curState;
        this.eState = eState;
        curState = stateDic[eState];
        curState.EnterState();

        return true;
    }

    public bool AddState(Define.State eState, BaseState state)
    {
        if (stateDic.ContainsKey(eState))
            return false;

        int stateIndex = (int)eState;
        if ((int)Define.State.Skill0 <= stateIndex && stateIndex < (int)Define.State.Max)
        {
            skillCount++;
        }

        stateDic.Add(eState, state);

        return true;
    }

    public bool RemoveState(Define.State eState)
    {
        if(stateDic.ContainsKey(eState))
        {
            stateDic.Remove(eState);
            return true;
        }

        return false;
    }

    public List<Define.State> UseableSkill()
    {
        if (skillCount == 0)
            return null;

        int start = (int)Define.State.Skill0;
        int end = (int)Define.State.Max;

        skillList.Clear();

        for (int i = start; i < end; i++)
        {
            Define.State eState = (Define.State)i;
            if (stateDic.ContainsKey(eState))
            {
                skillList.Add(eState);
            }
        }

        return skillList;
    }

    public int GetSkillCount()
    {
        return skillCount;
    }

    public Define.State RandomSelectSkill(List<Define.State> list)
    {
        if (list == null || list.Count == 0)
            return Define.State.None;

        int index = Random.Range(0, list.Count);

        return list[index];
    }


    public bool IsHasState(BaseState state)
    {
        return stateDic.ContainsValue(state);
    }

    public bool IsHasState(Define.State eState)
    {
        return stateDic.ContainsKey(eState);
    }
}
