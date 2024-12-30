using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Pos
{
    public Pos(int y, int x) { Y = y; X = x; }
    public int Y;
    public int X;
}

public struct PQNode : IComparable<PQNode>
{
    public int F;
    public int G;
    public int Y;
    public int X;

    public int CompareTo(PQNode other)
    {
        if (F == other.F)
            return 0;
        return F < other.F ? 1 : -1;
    }
}


public class MapManager
{
    public Grid CurrentGrid { get; private set; }

    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }

    public int SizeX { get { return MaxX - MinX + 1; } }
    public int SizeY { get { return MaxY - MinY + 1; } }

    bool[,] _collision;

    public bool CanGo(Vector3Int cellPos)
    {
        if (cellPos.x < MinX || cellPos.x > MaxX)
            return false;
        if (cellPos.y < MinY || cellPos.y > MaxY)
            return false;

        int x = cellPos.x - MinX;
        int y = MaxY - cellPos.y;
        return !_collision[y, x];
    }

    public void LoadMap(string mapName)
    {
        GameObject go = Managers.Resource.Instantiate($"Prefabs/Map/{mapName}");
        go.name = mapName;

        Transform col_transform =  go.transform.Find("Tilemap_Base");
        if (col_transform != null)
            col_transform.GetComponent<TilemapRenderer>().enabled = false;
        Transform moveable_transform = go.transform.Find("Tilemap_Moveable");
        if (moveable_transform != null)
            moveable_transform.GetComponent<TilemapRenderer>().enabled = false;

        CurrentGrid =  go.GetComponent<Grid>();

        // Collision 관련 파일
        TextAsset txt = Managers.Resource.Load<TextAsset>($"Map/{mapName}");
        StringReader reader = new StringReader(txt.text);

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        int xCount = MaxX - MinX + 1;
        int yCount = MaxY - MinY + 1;
        _collision = new bool[yCount, xCount];

        for (int y = 0; y < yCount; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < xCount; x++)
            {
                _collision[y, x] = (line[x] == '1' ? true : false);
            }
        }
    }

    // 상 하 좌 우 좌상, 좌하, 우상, 우하
    int[] _nextY = new int[] { 1, -1, 0, 0, 1, -1, 1, -1};
    int[] _nextX = new int[] { 0, 0, -1, 1, -1, -1, 1, 1};
    int[] _cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14};

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int endPos, bool ignoreDestCollision = false)
    {
        bool[,] visited = new bool[SizeY, SizeX];

        int[,] open = new int[SizeY, SizeX];

        // 비용 초기화
        for (int y = 0; y < SizeY; y++)
            for (int x = 0; x < SizeX; x++)
                open[y, x] = Int32.MaxValue;

        Pos[,] parent = new Pos[SizeY, SizeX];

        PrioirtyQueue<PQNode> pq = new PrioirtyQueue<PQNode>();

        Pos pos = Cell2Pos(startPos);
        Pos dest = Cell2Pos(endPos);

        if (pos.X < 0 || pos.X >= SizeX || pos.Y < 0 || pos.Y >= SizeY)
            return new List<Vector3Int>();

        open[pos.Y, pos.X] = 10 * (Math.Abs(dest.Y - pos.Y) + Math.Abs(dest.X - pos.X));

        // F : 최종 비용 (G + H)
        // G : 시작점에서 해당 좌표까지의 비용
        // H : 목적지까지 얼마나 가까운지 비용
        pq.Push(new PQNode() { F = 10 * (Math.Abs(dest.Y - pos.Y) + Math.Abs(dest.X - pos.X)), G = 0, Y = pos.Y, X = pos.X });
        parent[pos.Y, pos.X] = new Pos(pos.Y, pos.X);

        while (pq.Count > 0)
        {
            PQNode node = pq.Pop();
            if (visited[node.Y, node.X])
                continue;

            visited[node.Y, node.X] = true;

            if (node.Y == dest.Y && node.X == node.Y)
                break;

            for (int i = 0; i < _nextY.Length; i++)
            {
                Pos next = new Pos(node.Y + _nextY[i], node.X + _nextX[i]);

                if(!ignoreDestCollision || next.Y != dest.Y || next.X != dest.X)
                {
                    if (CanGo(Pos2Cell(next)) == false)
                        continue;
                }

                if (visited[next.Y, next.X])
                    continue;

                // 비용 계산
                int g = node.G + _cost[i];
                int h = 10 * ((dest.Y - next.Y) * (dest.Y - next.Y) + (dest.X - next.X) * (dest.X - next.X));
                if (open[next.Y, next.X] < g + h)
                    continue;

                open[next.Y, next.X] = g + h;
                pq.Push(new PQNode() { F = g + h, G = g, Y = next.Y, X = next.X });
                parent[next.Y,next.X] = new Pos(node.Y, node.X);
            }
        }

        return CalcCellPathFromParent(parent, pos, dest);
    }

    List<Vector3Int> CalcCellPathFromParent(Pos[,] parent, Pos start, Pos dest)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        int y = dest.Y;
        int x = dest.X;

        if (y < 0 || y >= SizeY || x < 0 || y >= SizeX)
            return cells;

        while (parent[y, x].Y != y || parent[y, x].X != x)
        {
            cells.Add(Pos2Cell(new Pos(y, x)));
            Pos pos = parent[y, x];
            y = pos.Y;
            x = pos.X;
        }
        cells.Add(Pos2Cell(new Pos(y, x)));
        cells.Reverse();

        if (x != start.X || y != start.Y)
        {
            cells.Clear();
        }
        

        return cells;
    }

    Pos Cell2Pos(Vector3Int cell)
    {
        return new Pos(MaxY - cell.y, cell.x - MinX);
    }

    Vector3Int Pos2Cell(Pos pos)
    {
        return new Vector3Int(pos.X + MinX, MaxY - pos.Y, 0);
    }
}