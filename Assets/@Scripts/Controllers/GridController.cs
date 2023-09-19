using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

public class GridController : BaseController
{
    private Grid _grid;
    
    Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();

    public override bool Init()
    {
        base.Init();

        _grid = gameObject.GetOrAddComponent<Grid>();

        return true;
    }
    
    // 몇 번째 셀인지
    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;

        if (_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell();
            _cells.Add(cellPos, cell);
        }

        return cell;
    }

    public void Add(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        // 해당하는 cell 번호에다 게임오브젝트를 등록
        cell.Objects.Add(go);
    }

    public void Remove(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Objects.Remove(go);
    }

    public List<GameObject> GatherObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();

        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                // 키를 포함하고 있지 않다면
                // 아직 해당 키(위치)에 Object가 등록되지 않았다는 뜻
                // 굳이 검사할 필요 X
                if (_cells.ContainsKey(new Vector3Int(x, y, 0))==false)
                    continue;
                
                // 범위 내에 있는 모든 오브젝트들을 긁어서 리스트에 저장
                objects.AddRange(_cells[new Vector3Int(x, y, 0)].Objects);
            }
        }
        
        return objects;
    }
}
