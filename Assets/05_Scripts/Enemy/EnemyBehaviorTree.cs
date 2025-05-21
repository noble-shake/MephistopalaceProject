using System;
using System.Collections.Generic;

public interface INode
{
    public enum STATE { RUN, SUCCESS, FAILED }
        //{ RUN, SUCCESS, FAILED
    public INode.STATE Evaluate();
}

[System.Serializable]
public class ActionNode : INode
{
    public Func<INode.STATE> action; // 반환형이 INode.STATE 인 대리자

    public ActionNode(Func<INode.STATE> action) // 노드를 생성할 때 매개변수로 대리자를 받음(지정자)
    {
        this.action = action;
    }

    public INode.STATE Evaluate()
    {
        // 대리자가 null 이 아닐 때 호출, null 인 경우 Failed 반환
        return action?.Invoke() ?? INode.STATE.FAILED;
    }
}

[System.Serializable]
public class SelectorNode : INode
{
    List<INode> children; // 여러 노드를 가질 수 있도록 리스트 생성

    public SelectorNode() { children = new List<INode>(); }

    public void Add(INode node) { children.Add(node); } // 셀렉터에 자식노드를 추가하는 메서드

    public INode.STATE Evaluate()
    {
        // 리스트 내의 노드들을 왼쪽부터(넣은 순으로) 검사
        foreach (INode child in children)
        {

            INode.STATE state = child.Evaluate();
            // child 노드의 state 가 하나라도 SUCCESS 이면 성공을 반환
            // 실행 중인 경우 RUN 반환
            switch (state)
            {
                case INode.STATE.SUCCESS:
                    return INode.STATE.SUCCESS;
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
            }
        }
        // 반복문이 끝났다면 해당 셀렉터의 자식노드들은 전부 FAILED 상태이므로 셀렉터는 FAILED 반환
        return INode.STATE.FAILED;
    }
}

[System.Serializable]
public class SequenceNode : INode
{
    List<INode> children; // 자식 노드들을 담을 수 있는 리스트

    public SequenceNode() { children = new List<INode>(); }

    public void Add(INode node) { children.Add(node); }

    public INode.STATE Evaluate()
    {
        // 자식 노드의 수가 0 이하라면 실패
        if (children.Count <= 0)
            return INode.STATE.FAILED;

        foreach (INode child in children)
        {
            // 자식 노드들중 하나라도 FAILED 라면 시퀀스는 FAILED
            switch (child.Evaluate())
            {
                case INode.STATE.RUN:
                    return INode.STATE.RUN;
                // SUCCESS 이면 아래는 검사하지 않고 continue 키워드로 다시 반복문 호출
                case INode.STATE.SUCCESS:
                    continue;
                case INode.STATE.FAILED:
                    return INode.STATE.FAILED;
            }
        }
        // FAILED 에 걸리지 않고 반복문을 빠져나왔으므로 시퀀스는 SUCCESS
        return INode.STATE.SUCCESS;
    }
}