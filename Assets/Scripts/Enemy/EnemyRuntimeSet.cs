using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New Enemy Runtime Set", menuName = "Set/Enemy Runtime")]
    public class EnemyRuntimeSet : RuntimeSet<Enemy>
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Items.Clear();
            GameEventManager.GameStart += () => Items.Clear();
        }
    }
}