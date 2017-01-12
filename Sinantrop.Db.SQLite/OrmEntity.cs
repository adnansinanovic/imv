using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoreLinq;
using SQLite;

namespace Sinantrop.DB.SQLite
{
    public abstract class OrmEntity
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private static OrmContext GetActiveContext()
        {
            return OrmEnvironment.Instance.ActiveContext;
        }

        public static int Insert<T>(T item)
        {
            return Insert(item, GetActiveContext());
        }

        public static int Insert<T>(T item, OrmContext context)
        {
            int pk;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                pk = connection.Insert(item);
            }
            return pk;
        }

        public static int Update<T>(T item)
        {
            return Update(item, GetActiveContext());
        }

        public static int Update<T>(T item, OrmContext context)
        {
            int pk;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                pk = connection.Update(item);
            }
            return pk;
        }

        public static int Save<T>(T item)
        {
            return Save(item, GetActiveContext());
        }

        public static int Save<T>(T item, OrmContext context)
        {
            int pk = -1;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                connection.RunInTransaction(() =>
                {
                    // ReSharper disable AccessToDisposedClosure
                    pk = connection.Update(item);
                    if (pk == 0)
                        pk = connection.Insert(item);
                    // ReSharper restore AccessToDisposedClosure
                }
                );
            }
            return pk;
        }

        public static void SaveEnumerable<T>(IEnumerable<T> items)
        {
            SaveEnumerable(items, OrmEnvironment.Instance.ActiveContext);
        }

        public static void SaveEnumerable<T>(IEnumerable<T> items, OrmContext context)
        {
            SQLiteAsyncConnection asyncConnection = OrmEnvironment.Instance.GetAsyncConnection(context.ContextName);

            asyncConnection.RunInTransactionAsync((SQLiteConnection connection) =>
            {
                foreach (var item in items)
                {
                    if (connection.Update(item) == 0)
                        connection.Insert(item);
                }
                
            });           
        }       

        public static T FindById<T>(object id) where T : new()
        {
            return FindById<T>(id, GetActiveContext());
        }

        public static T FindById<T>(object id, OrmContext context) where T : new()
        {
            T result;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                result = connection.Find<T>(id);
            }
            return result;
        }

        public static T Find<T>(Expression<Func<T, bool>> predicate) where T : new()
        {
            return Find(predicate, GetActiveContext());
        }

        public static T Find<T>(Expression<Func<T, bool>> predicate, OrmContext context) where T : new()
        {
            T result;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                result = connection.Find(predicate);
            }
            return result;
        }

        public static IList<T> FindAll<T>() where T : new()
        {
            return FindAll<T>(GetActiveContext());
        }

        public static IList<T> FindAll<T>(OrmContext context) where T : new()
        {
            IList<T> result;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                result = connection.Table<T>().ToList();
            }
            return result;
        }

        public static IList<T> FindAll<T>(Func<T, bool> predicate) where T : new()
        {
            return FindAll(predicate, GetActiveContext());
        }

        public static IList<T> FindAll<T>(Func<T, bool> predicate, OrmContext context) where T : new()
        {
            IList<T> result;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                result = connection.Table<T>().Where(predicate).ToList();
            }
            return result;
        }

        public static IList<T> FindAll<T>(Func<T, bool> predicate, int limit) where T : new()
        {
            return FindAll(predicate, limit, GetActiveContext());
        }

        public static IList<T> FindAll<T>(Func<T, bool> predicate, int limit, OrmContext context) where T : new()
        {
            IList<T> result;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                result = connection.Table<T>().Where(predicate).Take(limit).ToList();
            }
            return result;
        }

        public static T MaxBy<T, TKey>(Func<T, bool> predicate, Func<T, TKey> maxBy) where T : OrmEntity, new()
        {
            return MaxBy(predicate, maxBy, GetActiveContext());
        }

        public static T MaxBy<T, TKey>(Func<T, bool> predicate, Func<T, TKey> maxBy, OrmContext context) where T : new()
        {
            T result = default(T);
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                var list = connection.Table<T>().Where(predicate).ToList();

                if (list.Any())
                    result = list.MaxBy(maxBy);
            }
            return result;
        }

        public static T MinBy<T, TKey>(Func<T, bool> predicate, Func<T, TKey> minBy) where T : new()
        {
            return MinBy(predicate, minBy, GetActiveContext());
        }

        public static T MinBy<T, TKey>(Func<T, bool> predicate, Func<T, TKey> minBy, OrmContext context) where T : new()
        {
            T result = default(T);
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                var list = connection.Table<T>().Where(predicate).ToList();
                if (list.Any())
                    result = list.MinBy(minBy);
            }
            return result;
        }

        public static int Count<T>() where T : new()
        {
            return Count<T>(OrmEnvironment.Instance.ActiveContext);
        }

        public static int Count<T>(OrmContext context) where T : new()
        {
            int count;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                count = connection.Table<T>().Count();
            }

            return count;
        }

        public static T First<T>() where T : new()
        {
            return First<T>(OrmEnvironment.Instance.ActiveContext);
        }

        public static T First<T>(OrmContext context) where T : new()
        {
            T item;
            using (var connection = OrmEnvironment.Instance.GetConnection(context.ContextName))
            {
                item = connection.Table<T>().First();
            }

            return item;
        }
    }
}
