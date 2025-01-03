using UnityEngine;
using System.Collections.Generic;
public class ManagerChunk : MonoBehaviour
{
    public static ManagerChunk Instance;
    protected List<BaseChunk> _chunks;
    // * testing
    // [SerializeField] private bool _showChunkState = false;
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
            // if (chunk.State < GameVariables.Depth)
            if (chunk.IsActive)
                // positions.Add(chunk.GetPositions());
                positions.Add(chunk.Positions);
        return positions;
    }
    public void Register(BaseChunk chunk)
    {
        if (_chunks.Contains(chunk))
            return;
        _chunks.Add(chunk);
    }
    public Vector2Int[] GetDomain(Vector2 position)
    {
        List<Vector2Int[]> domain = new List<Vector2Int[]>();
        // get primary domain
        // domain.Add(new Vector2Int(Mathf.FloorToInt((position.x + 4.5f) / 9f), Mathf.FloorToInt((position.y + 4.5f) / 9f)));
        // get secondary domain(s)
        // // - initialize to default
        // _domain[1] = _domain[0];
        // // - get adjusted spawn
        // Vector2 offset = new Vector2(position.x + 4.5f, position.y + 4.5f);
        // // - check if on border
        // if (offset.x % 9f == 0f) _domain[1] += _domain[0].x * 9f > position.x ? Vector2Int.left : Vector2Int.right;
        // else if (offset.y % 9f == 0f) _domain[1] += _domain[0].y * 9f > position.y ? Vector2Int.down : Vector2Int.up;
        // 
        // check which chunk(s) the position is inside
        // get domain(s) of those chunks
        // 
        int count = 0;
        // check all chunks
        for (int i = _chunks.Count - 1; i > -1; i--)
            // get domain(s) if position inside chunk
            if (_chunks[i].IsChunked(position))
            {
                domain.Add(_chunks[i].Domain);
                count += _chunks[i].Domain.Length;
            }
        // 
        Vector2Int[] domainFinal = new Vector2Int[count];
        // compile domains into array
        for (int i = domain.Count - 1; i > -1; i--)
        {
            for (int j = domain[i].Length - 1; j > -1; j--)
            {
                count--;
                domainFinal[count] = (domain[i])[j];
            }
        }
        return domainFinal;
    }
    // // * testing, object pooling
    // void OnEnable()
    // {
    //     // BaseChunk.onPlayerDetect += ChunkUpdate;
    //     // GameClock.onTickPool += ChunkUpdate;
    //     GameMaster.onTransitionReady += ChunkUpdate;
    // }
    // void OnDisable()
    // {
    //     // BaseChunk.onPlayerDetect -= ChunkUpdate;
    //     // GameClock.onTickPool -= ChunkUpdate;
    //     GameMaster.onTransitionReady -= ChunkUpdate;
    // }
    // public delegate void OnChunkUpdate(List<string> vfxPooled, List<string> attackPooled, List<string> damagePooled, List<string> sfxPooled);
    // public static event OnChunkUpdate onChunkUpdate;
    // private void ChunkUpdate(Vector2Int room)
    // {
    //     List<Entity> entities = new List<Entity>();
    //     // - currently active chunks
    //     foreach (BaseChunk chunk in _chunks)
    //         if (chunk.IsActive)
    //             // - active entities from chunks
    //             entities.AddRange(chunk.GetEntitesActive());
    //     // - id lists from active entities
    //     List<string> vfxPooled = new List<string>();
    //     List<string> attackPooled = new List<string>();
    //     List<string> damagePooled = new List<string>();
    //     List<string> sfxPooled = new List<string>();
    //     foreach (Entity entity in entities)
    //     {
    //         vfxPooled.AddRange(entity.PooledVFX);
    //         attackPooled.AddRange(entity.PooledAttack);
    //         damagePooled.AddRange(entity.PooledDamage);
    //         sfxPooled.AddRange(entity.PooledSFX);
    //     }
    //     // 
    //     onChunkUpdate?.Invoke(vfxPooled, attackPooled, damagePooled, sfxPooled);
    // }
    // public bool ShowChunkState
    // {
    //     get { return _showChunkState; }
    // }
    // * testing null error on reload
    void OnDestroy()
    {
        Instance = null;
    }
    // * testing room aoe
    public List<Entity> GetEntites()
    {
        foreach (BaseChunk chunk in _chunks)
            if (chunk.IsCurrent)
                return chunk.Entities;
        return null;
    }
}