using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    Start,
    Battle,
    Exploration,
    Treasure,
    Shop,
    Boss
}
public class StageNodeData
{
    public int Id { get; }
        public StageType Type { get; set; }
        public Vector2 Position { get; set; }
        public List<StageNodeData> Connections { get; }
    
        public StageNodeData(int id, StageType type)
        {
            Id = id;
            Type = type;
            Connections = new List<StageNodeData>();
        }
    
        public void AddConnection(StageNodeData other)
        {
            if (other == null || Connections.Contains(other))
            {
                return;
            }
    
            Connections.Add(other);
            other.Connections.Add(this);
        }
}
