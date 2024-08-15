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
            // if (chunk.State < GameVariables.Depth)
            if (chunk.IsActive)
                positions.Add(chunk.GetPositions());
        return positions;
    }
    public void Register(BaseChunk chunk)
    {
        if (_chunks.Contains(chunk))
            return;
        _chunks.Add(chunk);
    }
    // * testing, object pooling
    void OnEnable()
    {
        // BaseChunk.onPlayerDetect += ChunkUpdate;
        // GameClock.onTickPool += ChunkUpdate;
        GameMaster.onTransitionReady += ChunkUpdate;
    }
    void OnDisable()
    {
        // BaseChunk.onPlayerDetect -= ChunkUpdate;
        // GameClock.onTickPool -= ChunkUpdate;
        GameMaster.onTransitionReady -= ChunkUpdate;
    }
    public delegate void OnChunkUpdate(List<string> vfxPooled, List<string> attackPooled, List<string> damagePooled, List<string> sfxPooled);
    public static event OnChunkUpdate onChunkUpdate;
    private void ChunkUpdate(Vector2Int room)
    {
        List<Entity> entities = new List<Entity>();
        // - currently active chunks
        foreach (BaseChunk chunk in _chunks)
            if (chunk.IsActive)
                // - active entities from chunks
                entities.AddRange(chunk.GetEntitesActive());
        // - id lists from active entities
        List<string> vfxPooled = new List<string>();
        List<string> attackPooled = new List<string>();
        List<string> damagePooled = new List<string>();
        List<string> sfxPooled = new List<string>();
        foreach (Entity entity in entities)
        {
            vfxPooled.AddRange(entity.PooledVFX);
            attackPooled.AddRange(entity.PooledAttack);
            damagePooled.AddRange(entity.PooledDamage);
            sfxPooled.AddRange(entity.PooledSFX);
        }
        // 
        onChunkUpdate?.Invoke(vfxPooled, attackPooled, damagePooled, sfxPooled);
    }
}