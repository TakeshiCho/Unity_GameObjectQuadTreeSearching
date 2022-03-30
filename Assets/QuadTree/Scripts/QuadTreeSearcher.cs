using System.Collections.Generic;
using Unity.Mathematics;

namespace QuadTree.Scripts
{
    public partial class Node
    {
        public static Node Search(float3 currentPosition, List<Node> tree)
        {
            Node current = null;
            Node node = tree[0];
            NodeRange range = new NodeRange(node.Info.Position, node.Info.Size);
            if (range.IsInRange(currentPosition))
            {
                current = Search(currentPosition, tree, 0);
            }
            return current;
        }
        
        private static Node Search(float3 currentPosition, List<Node> tree,int indexBegin)
        {
            
            Node current;
            Node node = tree[indexBegin];
            if (node.Info.ChildIndex == 0)
            {
                current = node;
                return current;
            }
            else 
            {
                int quadrant = GetQuadrent(currentPosition, node.Info.Position);
                current = Search(currentPosition,tree,node.Info.ChildIndex+quadrant);
            }
            return current;
        }
        
        public static int GetQuadrent(float3 objPosition,float3 nodePostion)
        {
            float3 relativePos = objPosition - nodePostion;
            if (relativePos.x >= 0)
            {
                if (relativePos.z >= 0)
                {
                    return 0;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (relativePos.z >= 0)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
    }
}