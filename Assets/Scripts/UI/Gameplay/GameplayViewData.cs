using Board;
using Services.UI;

namespace UI.Gameplay
{
    public class GameplayViewData : IPresenterData
    {
       public BoardController BoardController { get; set; }
    }
}
