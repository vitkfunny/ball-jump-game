using UnityEngine;

public class PlatformProperties : MonoBehaviour
{
    private bool _latest;

    public bool moving;
    
    public int type;
    // PlatformTypes:
    // 0 - Regular
    // 1 - OneTime
    // 2 - Timer
    // 3 - Jump
    // 4 - Kill

    public bool IsLatest()
    {
        return _latest;
    }

    public void SetLatest(bool latestState)
    {
        _latest = latestState;
    }
}
