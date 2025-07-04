using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFarthestRoomSelector
{
    int SelectFarthestRoom(List<NodeModel> nodes, int startNodeId);
}
