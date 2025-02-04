using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class IngameOptionUI : UIBase
    {
        // Start is called before the first frame update
        void Start()
        {
            UIManager.Show<IngameOptionUI>(UIList.IngameOptionUI);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
