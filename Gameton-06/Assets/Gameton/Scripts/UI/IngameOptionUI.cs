using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class IngameOptionUI : UIBase
    {
        // Start is called before the first frame update
        void Start()
        {
            UIManager.Show<IngameOptionUI>(UIList.IngameOptionUI);
        }

        public void OnClickOptionButton()
        {
            Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

            if (activeScene.name.Equals("Lobby"))
            {
                //  
            }
            else if (activeScene.name.StartsWith("Stage"))
            {
                //
            }
        }
    }
}
