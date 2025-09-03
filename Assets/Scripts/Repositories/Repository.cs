using UnityEngine;
using VContainer;

namespace StrikeBall.Core.Repositories
{
    public abstract class Repository : MonoBehaviour
    {
        public abstract void Register(IContainerBuilder builder);
    }
}