using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game;
using Game.Configs;
using Services.Log;
using Services.Save;
using Services.UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ProjectSettings projectSettings;
        public override void InstallBindings()
        {
            Container.Bind<ProjectSettings>().FromInstance(projectSettings).AsSingle();
            Container.Bind<SaveService>().AsSingle();
            Container.Bind<GameData>().AsSingle();
            Container.Bind<LogService>().AsSingle();
        }
    }
}
