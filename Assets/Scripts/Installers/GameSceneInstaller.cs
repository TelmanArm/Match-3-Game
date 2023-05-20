using Services.Audio;
using Services.UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UiService uiService;
        [SerializeField] private AudioService audioService;
        public override void InstallBindings()
        {
            Container.Bind<UiService>().FromInstance(uiService);
            Container.Bind<AudioService>().FromInstance(audioService);
        }
    }
}