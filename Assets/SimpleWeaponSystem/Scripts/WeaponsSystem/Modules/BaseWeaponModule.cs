using RolePoloGame;

namespace WeaponSystem.Modules
{
    public abstract class BaseWeaponModule : RuntimeScriptableObject
    {
        public abstract bool CanPerform(Types.BaseWeaponType weapon);
        public virtual void OnStartPerform(Types.BaseWeaponType weapon) { }
        public virtual void OnEndPerform(Types.BaseWeaponType weapon) { }
    }
}
