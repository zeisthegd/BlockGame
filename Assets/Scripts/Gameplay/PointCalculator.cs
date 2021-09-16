using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


///<summary>
/// Calculate the points to be added
///</summary>
public class PointCalculator : MonoBehaviour
{
    [SerializeField] TMP_Text totalPointsTxt; //Total points the player has scored.
    [SerializeField] TMP_Text pointsToAddTxt; //Points to add when the player clears some blocks.

    [SerializeField] float addDuration;// The animation duration of adding points.


    BlocksMatcher blocksMatcher; // The blocks matcher components to get the matches that the player has matched in order to calculate points
    GameSettings settings;// Settings for points calculation.


    ///<summary>
    /// Get components and connect events
    ///</summary>
    void Awake()
    {
        blocksMatcher = GetComponent<BlocksMatcher>();
    }

    void Start()
    {
        GameManager.Instance.RestartGame += Reset;
        blocksMatcher.BeginClearingBlocks += StartCalculation;
        settings = GameManager.Instance.Settings;
    }

    ///<summary>
    /// Disconnect all the game events
    ///</summary>
    void OnDisable()
    {
        GameManager.Instance.RestartGame -= Reset;
        blocksMatcher.BeginClearingBlocks -= StartCalculation;
    }

    ///<summary>
    ///Start the points calculation routine
    ///</summary>
    public void StartCalculation()
    {
        StartCoroutine(SetTotalScoreText());
    }

    ///<summary>
    ///Calculate the points and display the texts
    ///</summary>
    IEnumerator SetTotalScoreText()
    {
        int currentPoints = Convert.ToInt32(totalPointsTxt.text);
        int pointsToAdd = CalculatePoints(blocksMatcher.MatchedBlocks);
        int newTotal = currentPoints + pointsToAdd;
        SetPointsToAddText(pointsToAdd);

        float time = 0;
        while (currentPoints <= newTotal)
        {
            yield return null;
            time += (Time.deltaTime + 1 / addDuration);
            currentPoints += (int)time;
            totalPointsTxt.text = $"{currentPoints}";
            pointsToAddTxt.text = $"+ {newTotal - currentPoints}";
        }

        GameManager.Instance.CurrentPoints = newTotal;

        totalPointsTxt.text = newTotal + "";
        pointsToAddTxt.text = $"+ 0";
        pointsToAddTxt.gameObject.SetActive(false);
    }

    ///<summary>
    /// Display the points to add to the total points
    ///</summary>
    /// <param name="points">Points to add.</param>
    void SetPointsToAddText(int points)
    {
        pointsToAddTxt.gameObject.SetActive(true);
        pointsToAddTxt.text = $"+ {points}";
    }

    ///<summary>
    /// Using the matched blocks data from the blocks matcher component, start calculate the points
    /// Bonus points will be added when:
    /// a) The player matched more than 3 blocks;
    /// b) The player matched more than 1 cluster of blocks.
    ///</summary>
    ///<returns>Total points to add.</returns>
    ///<param name ="blocks">Matched blocks</param>
    int CalculatePoints(List<Block> blocks)
    {
        BlockType lastType = blocks[0].Sprite.Type;
        int previousX = blocks[0].X, previousY = blocks[0].Y;

        int totalPoints = 0;
        int currentChain = 0;
        int totalMatches = 1;

        foreach (Block block in blocks)
        {
            if (block.Sprite.Type != lastType || (block.X != previousX && block.Y != previousY))
            {
                totalMatches++;
                totalPoints += Calculate(ref currentChain, totalMatches);
            }
            currentChain += 1;
            if (blocks.IndexOf(block) == blocks.Count - 1)
            {
                totalPoints += Calculate(ref currentChain, totalMatches);
            }
            previousX = block.X;
            previousY = block.Y;
            lastType = block.Sprite.Type;
        }
        return totalPoints;
    }

    /// <summary>
    /// Based on the total blocks of the match and the current chain, and the total matches, calculate the points.
    /// </summary>
    /// <param name="currentChain">Total blocks of the current match </param>
    /// <param name="totalMatches">Total matches that will be cleared </param>
    /// <returns></returns>
    private int Calculate(ref int currentChain, int totalMatches)
    {
        int points = settings.Match3Point + settings.BonusAfter3Spree * (currentChain - 3) + totalMatches * settings.TotalMatchesBonus;
        currentChain = 0;
        return points;
    }


    ///<summary>
    /// Reset the points, stop all coroutines when the game is reset.
    ///</summary>
    public void Reset()
    {
        StopAllCoroutines();
        totalPointsTxt.text = 0 + "";
        pointsToAddTxt.text = "";
    }
}
