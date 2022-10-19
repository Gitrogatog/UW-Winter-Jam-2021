using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RunXTimesNode : DecoratorNode
{
    public int numberOfIterations;
    private int currentIterations = 0;
    public bool endOnFailure = false;
    public bool endOnSuccess = false;
    protected override void OnStart() {
        currentIterations = 0;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        switch (child.Update()) {
                case State.Running:
                    break;
                case State.Failure:
                    if (!endOnFailure && currentIterations < numberOfIterations - 1) {
                        currentIterations++;
                        return State.Running;
                    } else {
                        return State.Failure;
                    }
                case State.Success:
                    if (!endOnSuccess && currentIterations < numberOfIterations - 1) {
                        return State.Running;
                    } else {
                        return State.Success;
                    }
            }
            return State.Running;
    }
}
