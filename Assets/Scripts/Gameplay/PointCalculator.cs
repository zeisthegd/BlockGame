using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointCalculator : MonoBehaviour
{
    [SerializeField] TMP_Text totalPointsTxt;
    [SerializeField] TMP_Text pointsToAddTxt;

    [SerializeField] int match3Point;
    [SerializeField] float addDuration;
    [SerializeField] int bonusAfter3Spree;
    [SerializeField] int totalMatchesBonus;
    BlocksMatcher blocksMatcher;

    void Awake()
    {
        blocksMatcher = GetComponent<BlocksMatcher>();
        blocksMatcher.BeginClearingBlocks += StartCalculation;
    }

    void OnDisable()
    {
        blocksMatcher.BeginClearingBlocks -= StartCalculation;

    }
    public void StartCalculation()
    {
        StartCoroutine(SetTotalScoreText());
    }

    IEnumerator SetTotalScoreText()
    {
        int currentPoints = Convert.ToInt32(totalPointsTxt.text);
        int pointsToAdd = CalculatePoints(blocksMatcher.MatchedBlocks);
        int newTotal = currentPoints + pointsToAdd;
        SetScoreToAddText(pointsToAdd);
        float time = 0;
        while (currentPoints <= newTotal)
        {
            yield return null;
            time += (Time.deltaTime + 1 / addDuration);
            currentPoints += (int)time;
            totalPointsTxt.text = $"{currentPoints}";
            pointsToAddTxt.text = $"+ {newTotal - currentPoints}";
        }
        totalPointsTxt.text = newTotal + "";
        pointsToAddTxt.text = $"+ 0";
        pointsToAddTxt.gameObject.SetActive(false);
    }

    void SetScoreToAddText(int score)
    {
        pointsToAddTxt.gameObject.SetActive(true);
        pointsToAddTxt.text = $"+ {score}";
    }

    int CalculatePoints(List<Block> blocks)
    {
        BlockType lastType = blocks[0].Sprite.Type;
        int lastX = blocks[0].X, lastY = blocks[0].Y;

        int totalPoints = 0;
        int currentChain = 0;
        int totalMatches = 1;

        foreach (Block block in blocks)
        {
            if (block.Sprite.Type != lastType || (block.X != lastX && block.Y != lastY))
            {
                totalMatches++;
                totalPoints += Calculate(ref currentChain, totalMatches);
            }
            currentChain += 1;
            if (blocks.IndexOf(block) == blocks.Count - 1)
            {
                totalPoints += Calculate(ref currentChain, totalMatches);
            }
            lastX = block.X;
            lastY = block.Y;
            lastType = block.Sprite.Type;
        }
        return totalPoints;
    }

    private int Calculate(ref int currentChain, int totalMatches)
    {
        int points = match3Point + bonusAfter3Spree * (currentChain - 3) + totalMatches * totalMatchesBonus;
        currentChain = 0;
        return points;
    }

    public void Reset()
    {
        StopAllCoroutines();
        totalPointsTxt.text = 0 + "";
        pointsToAddTxt.text = "";
    }
}
