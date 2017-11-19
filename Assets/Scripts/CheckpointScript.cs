using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public int InitialReward;
    public int ExtraRewardLeft;
    public int Index;

    public bool TakeExtraRewardIfAnyLeft()
    {
        if (ExtraRewardLeft < 1) return false;
        ExtraRewardLeft--;
        return true;
    }

    public void SetRewardLeftToInitialValue()
    {
        ExtraRewardLeft = InitialReward;
    }
}
