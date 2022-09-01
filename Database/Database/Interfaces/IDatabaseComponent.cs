//change abstract members to virtual where applicable? wouldnt have to reimplement everywhere

using Database.Structs;
using Newtonsoft.Json;

namespace Database.Database.Interfaces {
    public abstract class IDatabaseComponent : ILogger, IPrimaryKey, IComparable<IDatabaseComponent> {
        protected PrimaryKey primary_key;
        protected IDatabaseComponent() {
            logger = this as ILogger;
        }
        private static   IDatabaseComponent? singleton                       { get; }
        public           bool                bError                          { get; set; } = false;
        protected static ulong               default_incremental_primary_key { get; set; }
        protected        bool                bIsPrimaryKeySet                { get; set; }
        protected        ILogger             logger                          { get; set; }

        // Factories can call an override of this to set a primary key unique to the class, no need for this with singletons.
        // Overrides should decrement the default key for consistency, and only set the key if the bool indicates it isn't already set.
        public virtual PrimaryKey TryGetOrCreatePrimaryKey() {
            if (bIsPrimaryKeySet) { return primary_key; }

            primary_key = new PrimaryKey(default_incremental_primary_key++, EPrimaryKeyType.UNKNOWN);
            bIsPrimaryKeySet = true;
            return primary_key;
        }

        public static IDatabaseComponent? GetSingleton() { return null; }

        public static implicit operator bool(IDatabaseComponent? component) { return component != null && !component.bError; }

        public static bool operator ==(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) { return true; }
            if (ReferenceEquals(lhs, null) && !ReferenceEquals(rhs, null)) { return false; }
            if (!ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) { return false; }

            return lhs.primary_key == rhs.primary_key;
        }

        public static bool operator !=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) { return !(lhs == rhs); }

        public static bool operator <(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (lhs == rhs) return false;
            if (lhs != null && rhs == null) return false;
            if (lhs == null && rhs != null) return true;

            return lhs.primary_key < rhs.primary_key;
        }

        public static bool operator >(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (lhs == rhs) return false;
            if (lhs != null && rhs == null) return true;
            if (lhs == null && rhs != null) return false;

            return lhs.primary_key > rhs.primary_key;
        }

        public static bool operator >=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            return !(lhs < rhs);
        }

        public static bool operator <=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            return !(lhs > rhs);
        }

        protected virtual bool IsEqual(IDatabaseComponent? rhs) {
            return this == rhs;
        }
        protected virtual bool IsLessThan(IDatabaseComponent? rhs) {
            return this < rhs;
        }

        public virtual string ToJson() { return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }); }

        public override bool Equals(object? obj) {
            return IsEqual(obj as IDatabaseComponent);
        }

        public int CompareTo(IDatabaseComponent? rhs) {
            if (rhs == null) return 1;
            return primary_key.CompareTo(rhs.primary_key);
        }
    }
}