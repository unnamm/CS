
namespace Lib.CS
{
    internal class SingletonProcess
    {
        class Test : Singleton3<Test> //maybe use 3
        {
            public int Value = 0;
        }

        public void F() //async get singleton
        {
            List<Task> tasks = new List<Task>();
            foreach (var v in Enumerable.Range(0, 100))
            {
                //async access can make many object
                tasks.Add(Task.Run(() => { int value = Test.Instance.Value; })); //async run
            }
            Task.WhenAll(tasks).Wait(); //wait all async
        }
    }
    internal class Singleton<T> where T : class, new()
    {
        private static T? _instance;
        private static object syncObject = new object(); //need member obejct, 1 object memory always allocated

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                        //_instance ??= new T(); //same null check
                    }
                }
                return _instance;
            }
        }
    }

    internal class Singleton2<T> where T : class, new()
    {
        private static readonly T _instance;
        static Singleton2()
        {
            _instance = new T();
        }

        public static T Instance { get { return _instance; } }
    }

    internal class Singleton3<T> where T : class, new() //same Singleton2
    {
        private static readonly T _instance = new(); //omit T, Constructor

        public static T Instance => _instance; //ramda get
    }

    internal class Singleton4<T> where T : class, new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>();
        //private static readonly Lazy<T> lazy = new Lazy<T>(() => new T()); //same result

        public static T Instance => lazy.Value;
    }
}
