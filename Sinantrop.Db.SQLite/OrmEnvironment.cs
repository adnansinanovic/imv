using System;
using System.Collections.Generic;
using SQLite;

namespace Sinantrop.DB.SQLite
{
    public class OrmEnvironment
    {
        private static OrmEnvironment _instance;
        private readonly Dictionary<string, OrmContext> _contexts;
        private OrmContext _activeContext;

        public OrmContext ActiveContext
        {
            get
            {
                if (_activeContext == null)
                    throw new Exception("There is no active context");

                return _activeContext;
            }
            set { _activeContext = value; }
        }

        public static OrmEnvironment Instance
        {
            get { return _instance ?? (_instance = new OrmEnvironment()); }
        }

        private OrmEnvironment()
        {
            _contexts = new Dictionary<string, OrmContext>();
        }

        public SQLiteConnection GetConnection(string contextName)
        {
            return _contexts[contextName].GetConnection();
        }

        public SQLiteConnection GetConnection()
        {
            return GetConnection(ActiveContext.ContextName);
        }

        public SQLiteAsyncConnection GetAsyncConnection(string contextName)
        {
            return _contexts[contextName].GetAsyncConnection();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return GetAsyncConnection(ActiveContext.ContextName);
        }


        public OrmContext AddContextIfNotExists(OrmContextSettings settings, bool changeActiveContext = true)
        {
            if (!_contexts.ContainsKey(settings.ContextName))
                return AddContext(settings, changeActiveContext);

            if (changeActiveContext)
                SetActiveContext(settings.ContextName);

            return _contexts[settings.ContextName];
        }

        public OrmContext AddContext(OrmContextSettings settings, bool changeActiveContext = true)
        {
            if (_contexts.ContainsKey(settings.ContextName))
                throw new ArgumentException($"Context with name '{settings.ContextName}' already exists. Try another name.");

            _contexts.Add(settings.ContextName, new OrmContext());

            if (changeActiveContext || _contexts.Count == 0)
                SetActiveContext(settings.ContextName);

            _contexts[settings.ContextName].Initialize(settings);

            return _contexts[settings.ContextName];
        }

        public bool Remove(string contextName)
        {
            if (!_contexts.ContainsKey(contextName))
                throw new ArgumentException($"Context with name '{contextName}' does not exist. Try another name.");

            return _contexts.Remove(contextName);
        }

        public void Clear()
        {
            _contexts.Clear();
        }

        public void SetActiveContext(string contextName)
        {
            if (!_contexts.ContainsKey(contextName))
                throw new ArgumentException($"Context with name '{contextName}' does not exist. Try another name.");

            ActiveContext = _contexts[contextName];
        }
    }
}
