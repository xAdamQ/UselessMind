// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Map2D<T>
// {
//     Dictionary<Vector2Int, T> Dictionary = new Dictionary<Vector2Int, T>();
//     public List<HBorders> HorizontalBorders;
//     public List<VBorders> VerticalBorders;

//     #region a project for adding in the middle of map
//     // public void Add(T value, Vector2Int poz)
//     // {
//     //     if (Dictionary.ContainsKey(poz))
//     //     {
//     //         //extend borders and ini this
//     //         var moveDir = new Vector2Int(); var borders = new Vector2Int();
//     //         if (poz.x >= 0)
//     //         {
//     //             HorizontalBorders[poz.y].Right++;

//     //             moveDir.x = 1;
//     //             borders.x = HorizontalBorders[poz.y].Right;
//     //         }
//     //         else
//     //         {
//     //             HorizontalBorders[poz.y].Left--;

//     //             moveDir.x = -1;
//     //             borders.x = HorizontalBorders[poz.y].Left;
//     //         }
//     //         if (poz.y >= 0)
//     //         {
//     //             VerticalBorders[poz.x].Up++;

//     //             moveDir.y = 1;
//     //             borders.y = VerticalBorders[poz.x].Up;
//     //         }
//     //         else
//     //         {
//     //             VerticalBorders[poz.x].Down--;

//     //             moveDir.y = -1;
//     //             borders.y = VerticalBorders[poz.x].Down;
//     //         }

//     //         //x loop
//     //         Dictionary.Add(borders,Dictionary[borders])
//     //         for (int i = borders.x -= moveDir.x * 2; i != poz.x; i -= moveDir.x)
//     //         {

//     //         }
//     //     }
//     //     else
//     //     {
//     //         Dictionary[poz] = value;

//     //         if (poz.x > HorizontalBorders[poz.y].Right)
//     //             HorizontalBorders[poz.y].Right = poz.x;
//     //         else if (poz.x < HorizontalBorders[poz.y].Left)
//     //             HorizontalBorders[poz.y].Left = poz.x;

//     //         if (poz.y > VerticalBorders[poz.x].Up)
//     //             VerticalBorders[poz.x].Up = poz.y;
//     //         else if (poz.y < VerticalBorders[poz.x].Down)
//     //             VerticalBorders[poz.x].Down = poz.y;
//     //     }
//     // }

//     #endregion

//     public void AddRight(T value, int row)
//     {
//     }
//     public void AddLeft(T value, int row)
//     {

//     }
//     public void AddUp(T value, int column)
//     {

//     }
//     public void AddDown(T value, int column)
//     {

//     }

//     // public void AddOnIni(T value, Vector2Int poz)
//     // {
//     //     Dictionary.Add(poz, value);
//     // }

//     static readonly Vector2Int[] Dirs2D = new Vector2Int[] { new Vector2Int(0, 1) };
//     public Map2D(T[] allValues, Vector2Int pivot, Vector2Int size)
//     {
//         // for (int axis = 0; axis < 2; axis4++)
//         // {
//         //     if (pivot[axis] == 0)
//         //     {
//         //         for (int i = 0; i < length; i++)
//         //         {

//         //         }
//         //     }
//         // }

//         //borders
//         var borders = new Vector2Int(size.x / (Mathf.Abs(pivot.x) + 1), 0);
//         var shortDimension = size.x < size.y ? size.x : size.y;
//         for (int level = 0; level < shortDimension; level++)
//         {
//             for (int side = 0; side < 4; side++)
//             {
//                 for (int ind = 0; ind < 0000; ind++)
//                 {

//                 }
//             }
//         }

//     }

// }

// // public class Borders
// // {
// //     public int Right, Left;

// //     public int this[int index]
// //     {
// //         get
// //         {
// //             return index == 0 ? Right : Left;
// //         }

// //         set
// //         {
// //             if (index == 0) Right = value;
// //             else Left = value;
// //         }
// //     }
// // }

// public class HBorders
// {
//     public int Right, Left;
// }
// public class VBorders
// {
//     public int Up, Down;
// }
