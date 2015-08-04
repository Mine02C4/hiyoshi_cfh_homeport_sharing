using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HiyoshiCfhClient.Utils
{
    /// <summary>
    /// 使用するスレッドの上限数を設けたタスクスケジューラー
    /// </summary>
    class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        /// <summary>
        /// 現在のスレッドがタスクを実行しているかどうか
        /// </summary>
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        /// <summary>
        /// 実行されるタスクのリスト
        /// </summary>
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

        /// <summary>
        /// 同時実行可能なタスク数の上限
        /// </summary>
        private readonly int _maxDegreeOfParallelism;

        /// <summary>
        /// 現在実行しているタスク数
        /// </summary>
        private int _delegatesQueuedOrRunning = 0;

        /// <summary>
        /// 上限数を指定してインスタンスを生成
        /// </summary>
        /// <param name="maxDegreeOfParallelism">同時実行可能なタスクの上限数(1以上の整数)</param>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// タスクをタスクキューに追加
        /// </summary>
        /// <param name="task">追加するタスク</param>
        protected sealed override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    _delegatesQueuedOrRunning++;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        /// <summary>
        /// 実行すべき作業があることをThreadPoolに通知します
        /// </summary>
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                _currentThreadIsProcessingItems = true;
                try
                {
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            if (_tasks.Count == 0)
                            {
                                _delegatesQueuedOrRunning--;
                                break;
                            }
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }
                        base.TryExecuteTask(item);
                    }
                }
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        /// <summary>
        /// 現在のスレッドでタスクを実行しようとします
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (!_currentThreadIsProcessingItems) return false;
            if (taskWasPreviouslyQueued)
            {
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            }
            else
            {
                return base.TryExecuteTask(task);
            }
        }

        /// <summary>
        /// 事前にスケジュールされてたタスクをスケジューラーから取り除きます
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}
