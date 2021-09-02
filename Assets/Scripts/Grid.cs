using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("--- Grid Settings ---")]
    [SerializeField] int xDimension = 1;
    [SerializeField] int yDimension = 1;
    [SerializeField] float fillTime = 1;

    [Header("--- Prefabs ---")]
    [SerializeField] BlockPrefab[] blockPrefabs;

    Block[,] blocks;
    BlocksMatcher blocksMatcher;
    PointCalculator pointCalculator;

    void Awake()
    {
        blocksMatcher = GetComponent<BlocksMatcher>();
        pointCalculator = GetComponent<PointCalculator>();
    }
    void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        MakeNewBoard();
        pointCalculator.Reset();
    }
    public void MakeNewBoard()
    {
        ClearBoard();
        InstantiateBlocks();
        StartCoroutine(Fill());
    }

    void InstantiateBlocks()
    {
        blocks = new Block[xDimension, yDimension];
        Vector2 randStartPos = new Vector2(Random.Range(0, xDimension), Random.Range(0, xDimension));
        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                SpawnNewBlock(i, j, BlockMode.NORMAL, randStartPos);
            }
        }
    }

    public IEnumerator Fill()
    {
        bool needRefill = true;
        while (needRefill)
        {
            yield return new WaitForSeconds(fillTime);
            while (FillBoard())
            {
                yield return new WaitForSeconds(fillTime);
            }
            needRefill = blocksMatcher.ClearAllValidMatches();
            Block.canPress = !needRefill;
        }
    }
    public bool FillBoard()
    {
        bool filledInbetween = FillBetweenTopAndBottomRow();
        bool filledTop = FillTopRow();
        return filledInbetween || filledTop;
    }
    private bool FillTopRow()
    {
        bool filled = false;
        int topRow = yDimension - 1;
        for (int i = 0; i < xDimension; i++)
        {
            Block topBlock = blocks[i, topRow];
            if (topBlock.Mode == BlockMode.EMPTY)
            {
                Destroy(topBlock.gameObject);
                SpawnNewBlock(i, topRow, BlockMode.NORMAL, new Vector2(i, topRow + 2));
                filled = true;
            }
        }
        return filled;
    }

    private bool FillBetweenTopAndBottomRow()
    {
        bool filled = false;
        for (int j = 1; j < yDimension; j++)
        {
            for (int i = 0; i < xDimension; i++)
            {
                Block block = blocks[i, j];
                if (block.Moveable())
                {
                    Block blockBelow = blocks[i, j - 1];
                    if (blockBelow.Mode == BlockMode.EMPTY)
                    {
                        Destroy(blockBelow.gameObject);
                        block.MoveableComponent.Move(i, j - 1);
                        blocks[i, j - 1] = block;
                        SpawnNewBlock(i, j, BlockMode.EMPTY);
                        filled = true;
                    }
                }
            }
        }
        return filled;
    }

    public Block SpawnNewBlock(int x, int y, BlockMode mode)
    {
        return SpawnNewBlock(x, y, mode, new Vector2(x, y));
    }

    public Block SpawnNewBlock(int x, int y, BlockMode mode, Vector2 position)
    {
        GameObject newBlock = Instantiate(blockPrefabs[(int)mode].prefab, position, Quaternion.identity);
        Block blockscript = newBlock.GetComponent<Block>();
        blockscript.Init(x, y, this, mode);

        if (mode == BlockMode.NORMAL)
        {
            blockscript.Sprite.SetType((BlockType)Random.Range(0, blockscript.Sprite.TypeCount));
            blockscript.MoveableComponent.Move(blockscript.X, blockscript.Y);
            newBlock.name = blockscript.Sprite.Type.ToString() + $" [{x},{y}]";
        }
        else newBlock.name = $"EMPTY [{x},{y}]";

        newBlock.transform.parent = this.transform;
        blocks[x, y] = blockscript;

        return blockscript;
    }

    private void ClearBoard()
    {
        StopAllCoroutines();
        blocksMatcher.Reset();
        var blocks = FindObjectsOfType<Block>();
        foreach (Block block in blocks)
        {
            Destroy(block.gameObject);
        }
    }
    public Block[,] Blocks { get => blocks; }
    public BlocksMatcher BlocksMatcher { get => blocksMatcher; }
    public int XDimension { get => xDimension; }
    public int YDimension { get => yDimension; }
    public PointCalculator PointCalculator { get => pointCalculator; }

    [System.Serializable]
    public struct BlockPrefab
    {
        public BlockMode mode;
        public GameObject prefab;
    }
}

