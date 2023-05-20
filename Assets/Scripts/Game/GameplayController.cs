using System;
using Board;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private BoardController boardController;

        private void Start()
        {
            boardController.Setup();
        }
    }
}