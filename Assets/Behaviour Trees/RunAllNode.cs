using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RunAllNode : CompositeNode
{
    protected int current;
    public bool returnSuccess = false;

    protected override void OnStart() {
        current = 0;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        for (int i = current; i < children.Count; ++i) {
            current = i;
            var child = children[current];

            switch (child.Update()) {
                case State.Running:
                    return State.Running;
                case State.Success:
                    continue;
                case State.Failure:
                    continue;
            }
        }
        if(returnSuccess){
            return State.Success;
        }
        return State.Failure;
    }
}
