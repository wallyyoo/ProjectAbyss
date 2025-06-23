using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    private List<Enemy> enemies = new();
    private Player player;

    public void RegisterPlayer(Player p)
    {
        player = p;
    }

    /// <summary>
    /// 생성한 적 등록
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    /// <summary>
    /// 등록한 적 삭제
    /// </summary>
    /// <param name="enemy"></param>
    public void UnRegisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        { 
            // 결과창
        }
    }

    public void PlayerAttack(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= enemies.Count) return;

        Enemy target = enemies[targetIndex];
        int damage = player.GetAttackDamage();

        target.TakeDamage(damage);

        PlayerTurnEnd();
    }

    /// <summary>
    /// 플레이어 데미지 받는 호출
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerTakeDamage(int damage)
    {
        player.TakeDamage(damage);
    }

    public void PlayerTurnEnd()
    {
        foreach (var enemy in enemies)
        {
            enemy.ProcessTurn();
        }
    }
}
