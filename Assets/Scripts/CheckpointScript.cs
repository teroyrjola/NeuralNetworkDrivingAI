using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public int InitialReward;
    public int RewardLeft;
    public int Index;

    public bool TakeExtraRewardIfAnyLeft()
    {
        if (RewardLeft < 1) return false;
        RewardLeft--;
        return true;
    }

    public void SetRewardLeftToInitialValue()
    {
        RewardLeft = InitialReward;
    }
}
