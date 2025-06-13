using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputConfig", menuName = "Configs/PlayerInputConfig")]
public class PlayerInputConfig : ScriptableObject
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode throwKey;
}
