using UnityEngine;

namespace vikwhite.Presenter
{
    public class CharacterPresenter : IPresenter
    {
        public void Bind(Character model, CharacterView view) {
            view.OnApplyDamage += model.ApplyDamage;
            model.OnGoPlace += view.GoPlace;
            model.OnHit += view.Hit;
            model.OnTakeDamage += view.TakeDamage;
            model.OnHealthChanged += view.SetHealth;
            model.OnDamageChanged += view.SetDamage;
            model.OnDie += view.Die;
        }
    }
}