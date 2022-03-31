using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace QuadTree.Scripts
{
    public class QuadTreeChecker : MonoBehaviour
    {
        public Camera Camera;
        private float3 _cameraPositionPrevious;
        
        public Material Default;
        public Material Feedback;
        
        public QuadTreeData QuadTreeData;
        
        private Node _currentNode;
        private Node _previousNode;
        
        public static Dictionary<int, Renderer> Renderers = new Dictionary<int, Renderer>();
        private void Update()
        {
            float3 cameraPosition = Camera.transform.position;
            float3 moved = math.abs(_cameraPositionPrevious - cameraPosition);
            if (math.dot(moved,1) > 0)
            {
                if(QuadTreeData.TreeData != null)_currentNode = Node.Search(cameraPosition,QuadTreeData.TreeData);
                
                if (_previousNode != null && _previousNode != _currentNode)
                {
                    foreach (var id in _previousNode.GameObjectID)
                    {
                        Renderers[id].sharedMaterial = Default;
                    }
                }
                if (_currentNode != null)
                {
                    foreach (var id in _currentNode.GameObjectID)
                    {
                        Renderers[id].sharedMaterial = Feedback;
                    }
                    _previousNode = _currentNode;
                }
                _cameraPositionPrevious = cameraPosition;
            }
        }
        private void OnDrawGizmos()
        {
            if (QuadTreeData != null&&QuadTreeData.TreeData!=null && QuadTreeData.TreeData.Count != 0)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(QuadTreeData.TreeData[0].Info.Position,QuadTreeData.TreeData[0].Info.Size);
                foreach (var node in QuadTreeData.TreeData)
                {
                    Gizmos.DrawWireCube(node.Info.Position,node.Info.Size);
                    
                }
                if (_previousNode != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(_previousNode.Info.Position,_previousNode.Info.Size);
                }
            }
        }
    }
}