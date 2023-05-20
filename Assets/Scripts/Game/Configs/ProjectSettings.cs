using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Config/ProjectSettings")]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private string gameName;
        [SerializeField] private int levelCont;
        [SerializeField] private int scoreForNextLevel;
    }
}