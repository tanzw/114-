using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Common
{

    /*Iterator迭代器抽象类*/
    abstract class Iterator<T>
    {
        public abstract T First();
        public abstract T Last();
        public abstract T Before();
        public abstract T Next();
        public abstract T CurrentItem();
        public abstract bool MoveNext();
        public abstract bool MoveBefore();
        public abstract void SetCurrent(int index);
        public abstract void SetCurrent(T item);
        public abstract int Count { get; }
        public abstract void Reset();
    }
    /*Aggregate聚集抽象类*/
    abstract class Aggregate
    {
        public abstract Iterator<T> createIterator<T>();
    }

    class ConcreteIterator<T> : Iterator<T>
    {

        // 定义了一个具体聚集对象    
        private ConcreteAggregate<T> aggregate;

        private int current = -1;

        // 初始化对象将具体聚集类传入
        public ConcreteIterator(ConcreteAggregate<T> aggregate)
        {
            this.aggregate = aggregate;
        }

        // 初始化对象将具体聚集类传入
        public ConcreteIterator(IList<T> aggregate)
        {
            this.aggregate = new ConcreteAggregate<T>(aggregate);
        }

        // 第一个对象
        public override T First()
        {
            return aggregate[0];
        }
        public override T Last()
        {
            return aggregate[aggregate.Count - 1];
        }

        public override T Before()
        {
            T ret = default(T);
            current--;
            if (current >= 0 && current < aggregate.Count)
            {
                ret = aggregate[current];
            }
            return ret;
        }

        public override void SetCurrent(int index)
        {
            if (index >= aggregate.Count || index < 0)
            {
                throw new Exception("设置迭代器的当前索引超出边界");
            }
            else
            {
                current = index;
            }
        }

        public override void Reset()
        {
            current = -1;
        }

        public override int Count
        {
            get
            {
                return aggregate.Count;
            }
        }

        public override void SetCurrent(T item)
        {
            var index = aggregate.FindIndex(item);
            if (index < 0)
            {
                throw new Exception("迭代器中未找到设置的对象");
            }
            else
            {
                current = index;
            }
        }

        // 得到聚集的下一对象
        public override T Next()
        {
            T ret = default(T);
            current++;
            if (current < aggregate.Count)
            {
                ret = aggregate[current];
            }
            return ret;
        }

        public override bool MoveBefore()
        {
            if ((current-1) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // 是否到结尾   
        public override bool MoveNext()
        {
            if ((current + 1) >= aggregate.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // 返回当前聚集对象
        public override T CurrentItem()
        {
            return aggregate[current];
        }
    }

    class ConcreteAggregate<T>
    {
        private IList<T> items;

        public ConcreteAggregate()
        {
            items = new List<T>();
        }

        public ConcreteAggregate(IList<T> list)
        {
            items = list;
        }



        // 返回聚集总个数
        public int Count
        {
            get { return items.Count; }
        }

        // 声明一个索引器
        public T this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }

        public int FindIndex(T item)
        {
            return items.IndexOf(item);
        }
    }
}