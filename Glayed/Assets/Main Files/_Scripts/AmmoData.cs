using System;
using UnityEngine;

[Serializable]
public class AmmoData
{
    public string ammoName;
    public int ammoWeight; // howmuch space the ammo uses
    public ParticleSystem ammoPS;

}

[Serializable]
public class FireAmmo : AmmoData
{ // AoE Dot
    
}

[Serializable]
public class IceAmmo : AmmoData
{ // Target Slow

}