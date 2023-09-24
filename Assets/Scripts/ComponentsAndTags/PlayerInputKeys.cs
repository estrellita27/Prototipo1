using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{
    [GenerateAuthoringComponent]
    public struct PlayerInputKeys : IComponentData
    {
        public KeyCode UpKey;
        public KeyCode DownKey;
        public KeyCode LeftKey;
        public KeyCode RightKey;
        public KeyCode PrimaryWeaponKey;
    }
}
