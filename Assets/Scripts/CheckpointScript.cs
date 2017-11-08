using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private int RewardLeft;
    public int InitialReward;
    public int Index;

    public bool TakeRewardIfAnyLeft()
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
