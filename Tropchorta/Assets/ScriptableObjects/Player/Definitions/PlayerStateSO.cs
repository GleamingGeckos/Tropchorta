using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateSO", menuName = "Player/PlayerStateSO")]
public class PlayerStateSO : ScriptableObject
{
    public PlayerState state;

    private void OnEnable()
    {
        state = PlayerState.Normal;
    }
}
