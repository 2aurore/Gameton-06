using UnityEngine;

namespace TON
{
    public partial class MonsterBase
    {
        private void OnGUI()
        {
            if (GUILayout.Button("Hit"))
            {
                ApplyDamage(1);
            }

            if (GUILayout.Button("Dead"))
            {
                ApplyDamage(1000000000);
            }
        }
    }
}
