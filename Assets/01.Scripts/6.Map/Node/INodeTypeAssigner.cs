using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeTypeAssigner
{
    /// <summary>
    /// 다음에 생성될 노드에 부여할 NodeType을 결정하여 반환한다.
    /// </summary>
    /// <param name="currentRoomCount">현재까지 생성된 방 개수</param>
    /// <param name="maxRoomCount">전체 방 개수 한계</param>
    /// <returns></returns>
    NodeType AssignType(int currentRoomCount, int maxRoomCount);
}
