using UnityEngine;

namespace vikwhite.Presenter
{
    public class BattleHUDPresenter : IPresenter
    {
        public void Bind(IBattle model, BattleHUDView view) {
            view.Initialize(model.LeftSide.BattlePlayer.Name, model.RightSide.BattlePlayer.Name);
            view.SetLeftHeroHealth(model.LeftSide.Hero.Health);
            view.SetRightHeroHealth(model.RightSide.Hero.Health);
            view.OnDragCardToBoardPlace = (d,p) => model.LeftSide.Board.TryAdd(d, BoardHandler.WorldToBoardPosition(p, Side.Left));;
            view.OnPlaceCharacter += model.LeftSide.EndChoose;
            model.OnNextRound = view.OnNextRound;
            model.LeftSide.OnStartChoose = () => view.UpdateCards(model.LeftSide.BattlePlayer.GetRandomCards(3));
            model.LeftSide.Hero.OnHealthChanged = view.SetLeftHeroHealth;
            model.RightSide.Hero.OnHealthChanged = view.SetRightHeroHealth;
            var battleView = GameObject.FindAnyObjectByType<BattleView>();
            view.OnDrag = (t) => {
                if(t == CharacterType.Melee) battleView.ShowMeleePoints();
                if(t == CharacterType.Range) battleView.ShowRangePoints();
            };
            view.OnDrop = () => {
                battleView.HideMeleePoints();
                battleView.HideRangePoints();
            };
        } 
    }
}