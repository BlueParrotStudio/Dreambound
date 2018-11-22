﻿using UnityEngine;

namespace Dreambound.Astar
{
    public class Node : IHeapItem<Node>
    {
        public bool Walkable;
        public int MovementPenalty;

        public readonly Vector3 GroundPosition;
        public readonly Vector3 WorldPosition;
        public readonly Vector3Int GridPosition;
        public readonly bool GroundNode;

        public int gCost;
        public int hCost;
        public Node Parent;

        private int _heapIndex;
        public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }

        public Node(bool walkable, Vector3 worldPosition, int x, int y, int z, Vector3 groundPosition, int penalty, bool groundNode)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridPosition = new Vector3Int(x, y ,z);
            MovementPenalty = penalty;
            GroundNode = groundNode;
            GroundPosition = groundPosition;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int CompareTo(Node node)
        {
            int compare = fCost.CompareTo(node.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(node.hCost);
            }
            return -compare;
        }
    }
}