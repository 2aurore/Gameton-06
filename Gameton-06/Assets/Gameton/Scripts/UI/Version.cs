using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class Version : MonoBehaviour
    {
        private TextMeshProUGUI versionText;
        
        void Start()
        {
            // TextMesh Pro UI 텍스트 컴포넌트
            versionText = GetComponent<TextMeshProUGUI>();

            // Application.version을 사용하여 버전을 표시
            versionText.text = "Version: " + Application.version;
        }
    }
}
