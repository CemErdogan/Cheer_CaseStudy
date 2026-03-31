using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.LevelSystem;
using Abstractions.SaveSystem;
using Zenject;

namespace Game.LevelSystem.Runtime
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly LevelDatabase _database;
        [Inject] private readonly ISaveManager _saveManager;
        
        private int _currentLevelIndex;
        private ILevel _currentLevel;
        private readonly Queue<int> _rndQueue = new();
        
        public void Initialize()
        {
            _currentLevelIndex = _saveManager.Load<int>(SaveKeys.LevelIndex);
            _signalBus.Subscribe<ICompleteLevelSignal>(OnLevelCompleted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ICompleteLevelSignal>(OnLevelCompleted);
        }
        
        public void LoadLevel() => UnloadAndLoadLevel(GetCurrentLevel());

        private void UnloadAndLoadLevel(ILevel level)
        {
            _currentLevel?.Unload();
            _currentLevel = level;
            _currentLevel.Load();
        }

        private ILevel GetCurrentLevel()
        {
            var lvls = _database.Levels;
            return lvls[_currentLevelIndex];
        }

        private void RefillRandomQueue()
        {
            _rndQueue.Clear();
            var indices = Enumerable.Range(0, _database.Levels.Length).OrderBy(_ => UnityEngine.Random.value).ToList();
            foreach (var i in indices)
            {
                _rndQueue.Enqueue(i);
            }
        }
        
        private void OnLevelCompleted(ICompleteLevelSignal signal)
        {
            var completeType = signal.CompleteType;
            var lvs = _database.Levels;
            var nextIndex = _currentLevelIndex;

            if (completeType == CompleteType.Win)
            {
                nextIndex++;
            }

            if (nextIndex < lvs.Length)
            {
                _currentLevelIndex = nextIndex;
            }
            else
            {
                if (_rndQueue.Count == 0)
                {
                    RefillRandomQueue();
                }

                _currentLevelIndex = _rndQueue.Dequeue();
            }
            
            _saveManager.Save(_currentLevelIndex, SaveKeys.LevelIndex);
        }
    }
}