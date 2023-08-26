using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class KODecReq : DecisionRequester
{
    [Range(0,200)]
    public new int DecisionPeriod;
    protected override bool ShouldRequestAction(DecisionRequestContext context)
    {
        return Time.frameCount % DecisionPeriod == 0;
    }
}
