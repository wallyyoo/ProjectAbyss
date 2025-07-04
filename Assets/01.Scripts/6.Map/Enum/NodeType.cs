/// <summary>
/// 노드의 종류(전투, 상점, 보상 등)
/// </summary>
public enum NodeType
{
    Unknown, //미공개
    Start, //거점
    Battle, //전투
    Shop, //상점 
    Rest,//회복맵 이동
    Event, // 이벤트
    Move, // 맵 이동
    Empty, // 빈 노드
    Boss //  보스, 보스일 때만 사용
}
