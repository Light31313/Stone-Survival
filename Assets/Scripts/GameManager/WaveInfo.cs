using System;
using UnityEngine;

[Serializable]
public class WaveInfo
{
    [SerializeField]
    private EnemyType[] enemies;
    [SerializeField]
    private EnemyType[] bossesWave;
    public EnemyType[] Enemies => enemies;
    public EnemyType[] Bosses => bossesWave;
}
