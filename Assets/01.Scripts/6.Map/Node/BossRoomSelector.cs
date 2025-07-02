using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFarthestRoomSelector
{
    int SelectFarthestRoom(List<NodeModel> nodes, int startNodeId);
}
public class FarthestRoomSelector:IFarthestRoomSelector
{
    public int SelectFarthestRoom(List<NodeModel> nodes, int startNodeId)
    {
        var distanceByNode = new Dictionary<int, int>();
        foreach (var node in nodes)
            distanceByNode[node.Id] = int.MaxValue;
        distanceByNode[startNodeId] = 0;

        var queue = new Queue<int>();
        queue = new Queue<int>();
        queue.Enqueue(startNodeId);
        while (queue.Count > 0)
        {
            int currentId = queue.Dequeue();
            var currentNode = nodes.Find(n=>n.Id==currentId);

            foreach (var neighborId in currentNode.ConnectedNodeIds)
            {
                if(distanceByNode[neighborId] > distanceByNode[currentNode.Id]+1)
                {
                    distanceByNode[neighborId] = distanceByNode[currentNode.Id] + 1;
                    queue.Enqueue(neighborId);
                }
            }
        }

        int fartehestNodeId = startNodeId;
        int maxDistance = 0;
        foreach(var kvp in distanceByNode)
        {
            if (kvp.Value > maxDistance && kvp.Value < int.MaxValue)
            {
                maxDistance = kvp.Value;
                fartehestNodeId = kvp.Key;
            }
        }
        return fartehestNodeId;
    }
}
