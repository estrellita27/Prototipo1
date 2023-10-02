using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace TMG.Shooter
{
    public class GameManager : MonoBehaviour
    {
        private float puntos;

        private TextMeshProUGUI textMesh;

        private void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>(); 

        }

        private void Update()
        {
            puntos += Time.deltaTime;
            textMesh.SetText(puntos.ToString("0"));

        }
    }

}
