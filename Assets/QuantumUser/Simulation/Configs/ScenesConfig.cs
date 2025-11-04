using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    public class ScenesConfig : AssetObject
    {
        [Tooltip("Scene to be loaded after finishing Level1")]
        public AssetRef<Quantum.Map> Level2Map;
    }
}