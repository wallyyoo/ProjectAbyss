using NaughtyAttributes;

public class TurnManager : Singleton<TurnManager>
{
    private Enemy enemy;
    public Player player;

    public void RegisterPlayer(Player p)
    {
        player = p;
    }

    /// <summary>
    /// 생성한 적 등록
    /// </summary>
    /// <param name="e"></param>
    public void RegisterEnemy(Enemy e)
    {
        enemy = e;
    }

    /// <summary>
    /// 플레이어 공격
    /// </summary>
    [Button("플레이어 공격 테스트")]
    public void PlayerAttack()
    {
        int damage = player.GetAttackDamage();

        enemy.TakeDamage(damage);

        PlayerTurnEnd();
    }

    /// <summary>
    /// 플레이어 데미지 받음
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerTakeDamage(int damage)
    {
        player.TakeDamage(damage);
    }

    /// <summary>
    /// 플레이어 턴이 끝나면 적의 턴 감소 또는 공격
    /// </summary>
    public void PlayerTurnEnd()
    {
        enemy.ProcessTurn();
    }
}