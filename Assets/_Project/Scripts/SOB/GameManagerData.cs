using UnityEngine;

[CreateAssetMenu(fileName = "Game Manager Data", menuName = "SOB/Game Manager Data", order = 1)]
public class GameManagerData: ScriptableObject
{
    public float chainSpacing;
   
    public float hungerIncreaseTimeGap;
    public AnimationCurve opacityCurve;

    public float allyLerpSpeed;

    [Header("Set Dynamically")]
    public float roomWidth;
    public float roomHeight;

}