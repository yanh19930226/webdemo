namespace webdemo.Models.Domain
{
    public abstract class Entity : Entity<long>
    {
    }
    public abstract class Entity<T>
    {
        public T Id
        {
            get;
            protected set;
        }

        public DateTime CreateTime
        {
            get;
            set;
        }

        public DateTime ModifyTime
        {
            get;
            set;
        }

        public bool IsDel
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            Entity<T> entity = obj as Entity<T>;
            if ((object)this == entity)
            {
                return true;
            }
            if ((object)entity == null)
            {
                return false;
            }
            return Id.Equals(entity.Id);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }
            if ((object)a == null || (object)b == null)
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id?.ToString() + "]";
        }
    }
}
