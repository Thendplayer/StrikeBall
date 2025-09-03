using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace StrikeBall.Core.Repositories
{
    public class RepositoryScopeContainer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Repository[] _repositories;

        private List<LifetimeScope> _repositoryScopes = new List<LifetimeScope>();
        
        public void Register(LifetimeScope parentScope)
        {
            foreach (var repository in _repositories)
            {
                var childScope = parentScope.CreateChild(builder => repository.Register(builder));
                _repositoryScopes.Add(childScope);
            }
        }

        public void Dispose()
        {
            foreach (var scope in _repositoryScopes)
            {
                scope.Dispose();
            }
        }
    }
}