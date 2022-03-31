using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace QuadTree.Scripts
{
    public class QuadTreeBaker : MonoBehaviour
    {
        public int PerAreaMaxCount;
        
        private Transform _treeTransform;
        private float3 _treeSize;
        private float3 _treePosition;
        
        private List<GameObject> _gamebjects;
        private List<GameObject> _collectedGameObjects = new List<GameObject>();
        private int _indexCounter = 1;

        
        private List<Node> _tree = new List<Node>();
        public QuadTreeData QuadTreeData;
        
        [ContextMenu("Bake Tree")]
        private void Bake()
        {
            _gamebjects = GameObjectId.GameObjectsList;
            _treeTransform = gameObject.transform;
            _treePosition = _treeTransform.position;
            _treeSize = _treeTransform.localScale;
            _collectedGameObjects.AddRange(_gamebjects);
            
            BakeTree(_tree);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(QuadTreeData.GetHashCode()));
            QuadTreeData = ScriptableObject.CreateInstance<QuadTreeData>();
            QuadTreeData.TreeData = SortQuadTree(_tree);
            AssetDatabase.CreateAsset(QuadTreeData,"Assets/QuadTree/QuadTreeData.asset");
        }

        void BakeTree(List<Node> tree)
        {
            Node quadTree = new Node(ref _collectedGameObjects, tree, _treePosition, _treeSize, PerAreaMaxCount,0, ref _indexCounter);
        }
        
        List<Node> SortQuadTree(List<Node> originalTree)
        {
            List<Node> newTree = new List<Node>();
            for (int i = 0; i < originalTree.Count; i++)
            {
                foreach (var node in originalTree)
                {
                    if (node.Info.Index == i)
                    {
                        newTree.Add(node);
                        break;
                    }
                }
            }
            return newTree;
        }
    }

    [Serializable]
    public struct NodeInfo
    {
        public int Index;
        public float3 Position;
        public float3 Size;
        public int ChildIndex;
    }

    [Serializable]
    public struct NodeRange
    {
        public float4 Range;
        public NodeRange(float3 position, float3 size)
        {
            Range.x = position.x - size.x * 0.5f; /* Min x */
            Range.y = position.x + size.x * 0.5f; /* Max x */
            Range.z = position.z - size.z * 0.5f; /* Min y */
            Range.w = position.z + size.z * 0.5f; /* Max y */
        }
        public bool IsInRange(float3 position)
        {
            bool isInRange = position.x >= Range.x && position.x <= Range.y && position.z >= Range.z && position.z <= Range.w;
            return isInRange;
        }
    }
    
    [Serializable]
    public partial class Node
    {
        public NodeInfo Info;
        public int[] GameObjectID;
        public Node(ref List<GameObject> gameObjects,List<Node> tree,float3 position,float3 size, int max,int index, ref int indexCounter)
        {
            tree.Add(this);
            Info.Index = index;
            Info.Position = position;
            Info.Size = size;
            NodeRange range = new NodeRange(position,size);
            List<GameObject> collected = new List<GameObject>();
            foreach (var gameObject in gameObjects)
            {
                float3 gameObjPos = gameObject.transform.position;
                bool isInRange = range.IsInRange(gameObjPos);
                if (isInRange)
                {
                    collected.Add(gameObject);
                }
            }

            foreach (var gameObject in collected)
            {
                gameObjects.Remove(gameObject);
            }

            if (collected.Count > max)
            {
                Scripts.Node[] child = new Scripts.Node[4];
                GetChildInfo(position,size,out float3[] childPos,out float3 childsize);
                Info.ChildIndex = indexCounter;
                indexCounter += 4;
                for (int i = 0; i < child.Length; i++)
                {
                    child[i] = new Scripts.Node(ref collected,tree,childPos[i],childsize,max,Info.ChildIndex+i,ref indexCounter);
                }
            }
            else
            {
                List<int> id = new List<int>();
                foreach (var gameObject in collected)
                {
                    id.Add(gameObject.GetComponent<GameObjectId>().GameObjectID);
                }
                GameObjectID = id.ToArray();
                Info.ChildIndex = 0;
            }
        }

        private void GetChildInfo(float3 rootPos, float3 rootSize, out float3[] childPos, out float3 childSize)
        {
            childPos = new float3[4];
            childPos[0] = new float3(rootPos.x + rootSize.x * 0.25f, rootPos.y, rootPos.z + rootSize.z * 0.25f);
            childPos[1] = new float3(rootPos.x - rootSize.x * 0.25f, rootPos.y, rootPos.z + rootSize.z * 0.25f);
            childPos[2] = new float3(rootPos.x - rootSize.x * 0.25f, rootPos.y, rootPos.z - rootSize.z * 0.25f);
            childPos[3] = new float3(rootPos.x + rootSize.x * 0.25f, rootPos.y, rootPos.z - rootSize.z * 0.25f);
            childSize = new float3(rootSize.x * 0.5f, rootSize.y, rootSize.z * 0.5f);
        }
        
    }
    
}