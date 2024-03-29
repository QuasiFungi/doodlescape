using UnityEngine;
using System.Collections.Generic;
public class ManagerChunk : MonoBehaviour
{
    public static ManagerChunk Instance;
    protected List<BaseChunk> _chunks;
    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
        _chunks = new List<BaseChunk>();
    }
    public List<Vector2Int[]> GetActive()
    {
        List<Vector2Int[]> positions = new List<Vector2Int[]>();
        foreach (BaseChunk chunk in _chunks)
            if (chunk.State < GameVariables.Depth)
                positions.Add(chunk.GetPositions());
        return positions;
    }
    public void Register(BaseChunk chunk)
    {
        if (_chunks.Contains(chunk))
            return;
        _chunks.Add(chunk);
    }
}
