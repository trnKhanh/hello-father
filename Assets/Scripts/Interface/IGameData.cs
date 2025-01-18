using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGameData
{
    public abstract void Save(string root);
    public abstract void Load(string root);
}
