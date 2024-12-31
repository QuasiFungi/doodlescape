// using UnityEngine;
// // using UnityEngine.Pool;
// using System.Collections.Generic;
// // vfx, attack, damage, sfx
// public class ManagerPool : MonoBehaviour
// {
//     // // private DictionaryPool<string,ObjectPool<Entity>> _pool;
//     // // private Dictionary<string,ObjectPool<Entity>> _pool;
//     // private static ObjectPool<Entity> _pool;
//     // void Awake()
//     // {
//     //     // _pool = new DictionaryPool<string, ObjectPool<Entity>>();
//     //     // ? not create in awake but instead when chunks reloaded via active chunk change
//     //     // ? created here but populate on chunk loading ? population handled automatically
//     //     _pool = new ObjectPool<Entity>(PoolCreate, OnPooledGet, OnPooledRelease, OnPooledDestroy, true, 10, 20);
//     // }
//     // private Entity PoolCreate()
//     // {
//     //     // ? id not defined at time of pool creation, not needed
//     //     return Instantiate(Resources.Load(_id), _position, _rotation) as Entity;
//     // }
//     // private void OnPooledGet(Entity entity)
//     // {
//     //     print(entity);
//     //     entity.ToggleActive(true, _position, _rotation);
//     // }
//     // private void OnPooledRelease(Entity entity)
//     // {
//     //     entity.ToggleActive(false);
//     // }
//     // private void OnPooledDestroy(Entity entity)
//     // {
//     //     // ! pool-able entities must override discard with added a destroy call
//     //     entity.Discard();
//     // }
//     // // 
//     // // ? account for asynchronous ? defaults not needed as created on required
//     // // private static string _id = "Attack/atk_fortress_spikeSnail";
//     // private static string _id;
//     // // private static Vector3 _position = Vector3.zero;
//     // private static Vector3 _position;
//     // // private static Quaternion _rotation = Quaternion.identity;
//     // private static Quaternion _rotation;
//     // public static Entity PooledGet(string id, Vector3 position, Quaternion rotation)
//     // // public static void PooledGet(string id, Vector3 position, Quaternion rotation)
//     // {
//     //     // ? use struct or class ? case for multiple calls in same frame
//     //     _id = id;
//     //     _position = position;
//     //     _rotation = rotation;
//     //     // ? handle id filtering etc here
//     //     return _pool.Get() as Entity;
//     //     // _pool.Get();
//     // }
//     // // ? get id to target appropriate pool
//     // public static void PooledRelease(Entity entity)
//     // {
//     //     _pool.Release(entity);
//     // }
//     // 
//     // private static Dictionary<string,List<Entity>> _pool;
//     // private static List<Entity> _pool;
//     // void Awake()
//     // {
//     //     // _pool = new DictionaryPool<string, ObjectPool<Entity>>();
//     //     // ? not create in awake but instead when chunks reloaded via active chunk change
//     //     // ? created here but populate on chunk loading ? population handled automatically
//     //     // _pool = new Dictionary<Entity>(PoolCreate, OnPooledGet, OnPooledRelease, OnPooledDestroy, true, 10, 20);
//     //     // ? activeInHierarchy
//     //     _pool = new List<Entity>();
//     // }
//     private static Dictionary<string, List<Entity>> _vfxPooled;
//     private static Dictionary<string, List<Entity>> _attackPooled;
//     private static Dictionary<string, List<Entity>> _damagePooled;
//     private static Dictionary<string, List<Entity>> _sfxPooled;
//     void Awake()
//     {
//         _vfxPooled = new Dictionary<string, List<Entity>>();
//         _attackPooled = new Dictionary<string, List<Entity>>();
//         _damagePooled = new Dictionary<string, List<Entity>>();
//         _sfxPooled = new Dictionary<string, List<Entity>>();
//     }
//     // void OnEnable()
//     // {
//     //     ManagerChunk.onChunkUpdate += Initialize;
//     // }
//     // void OnDisable()
//     // {
//     //     ManagerChunk.onChunkUpdate += Initialize;
//     // }
//     private void Initialize(List<string> vfxPooled, List<string> attackPooled, List<string> damagePooled, List<string> sfxPooled)
//     {
//         // - check if entities already exist
//         // - otherwise add them
//         // - discard unneeded entites
//         // print("vfx:");
//         // foreach (string vfx in vfxPooled) print(vfx);
//         // print("attack:");
//         // foreach (string attack in attackPooled) print(attack);
//         // print("damage:");
//         // foreach (string damage in damagePooled) print(damage);
//         // print("sfx:");
//         // foreach (string sfx in sfxPooled) print(sfx);
//     }
//     // public static Entity PooledGet(string id, Vector3 position, Quaternion rotation)
//     // {
//     //     // 
//     // }
//     // public static void PooledRelease(Entity entity)
//     // {
//     //     // 
//     // }
// }