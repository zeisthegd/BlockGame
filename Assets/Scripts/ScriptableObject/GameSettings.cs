using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Settings for the game.
/// </summary>
[CreateAssetMenu(menuName = "Settings/Game", fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public int Moves = 10; // Number of moves that the player can make to match the blocks.
    public int OneStarScore = 200; // Reach this to get 1 star
    public int TwoStarScore = 1000;// Reach this to get 2 star
    public int ThreeStarScore = 2000;// Reach this to get 3 star
    public int SpecialBlockBonus = 30;// Reach this to get 3 star
    public int Match3Point; // The standard points to add when the player match a cluster of 3 blocks
    public int BonusAfter3Spree; // The bonus points if the players clears more than 3 blocks.
    public int TotalMatchesBonus; // The additional points to add if the player clears more than 1 cluster.
}
