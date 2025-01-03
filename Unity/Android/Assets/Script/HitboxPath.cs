// using UnityEngine;
// // ? used by seedFlower, projectile damage type instead
// public class HitboxPath : BaseHitbox
// {
//     private Vector3[] _path;
//     private int _index;
//     protected override void Awake()
//     {
//         base.Awake();
//         // 
//         _index = 0;
//         // * testing copied from mob ? search filter layer(s)
//         if (GameGrid.Instance.WorldToIndex(_target) != GameGrid.Instance.WorldToIndex(Position))
//             GameNavigation.Instance.PathCalculate(new PathData(Position, _target, true, OnPathFound));
//     }
//     // private Vector2 Position
//     // {
//     //     get { return transform.position; }
//     // }
//     private void OnPathFound(Vector3[] path, bool success)
//     {
//         if (success)
//         {
//             // _path = path[0];
//             // // 
//             // // * testing mob walk "animation"
//             // Move(_path);
//             _path = path;
//         }
//     }
//     protected override void OnEnable()
//     {
//         base.OnEnable();
//         // 
//         GameClock.onTick += Iterate;
//     }
//     protected override void OnDisable()
//     {
//         base.OnDisable();
//         // 
//         GameClock.onTick -= Iterate;
//     }
//     private void Iterate()
//     {
//         if (_index == _path.Length)
//         {
//             Discard();
//             return;
//         }
//         print(_path[_index]);
//         _index++;
//     }
// }