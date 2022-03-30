using System.Collections.Generic;
using UnityEngine;

namespace QuadTree.Scripts
{
    [CreateAssetMenu(fileName = "QuadTreeData", menuName = "QuadTreeData", order = 0)]
    public class QuadTreeData : ScriptableObject
    {
        [SerializeField]public List<Scripts.Node> TreeData;
    }
}