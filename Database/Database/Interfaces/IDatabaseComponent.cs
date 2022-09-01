//change abstract members to virtual where applicable? wouldnt have to reimplement everywhere

using Database.Structs;
using Newtonsoft.Json;

namespace Database.Database.Interfaces {
    public abstract class IDatabaseComponent : ILogger, IPrimaryKey {
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
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) { return false; }

            if (lhs.IsEqual(rhs)) { return true; }

            return false;
        }

        public static bool operator !=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) { return !(lhs == rhs); }

        public static bool operator <(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (!lhs || !rhs) { return false; }

            return lhs.IsLessThan(rhs);
        }

        public static bool operator >(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (!lhs || !rhs) { return false; }

            return !(lhs < rhs) && lhs != rhs;
        }

        public static bool operator >=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (!lhs || !rhs) { return false; }

            return !(lhs < rhs) || lhs == rhs;
        }

        public static bool operator <=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
            if (!lhs || !rhs) { return false; }

            return lhs < rhs || lhs == rhs;
        }

        protected virtual bool IsEqual(IDatabaseComponent? rhs) {
            if(rhs == null) return false;
            if(rhs == this) return true;

            if(primary_key == rhs.primary_key) return true;

            return false;
        }
        protected virtual bool IsLessThan(IDatabaseComponent? rhs) {
            return !IsEqual(rhs);
        }

        public virtual string ToJson() { return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }); }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(this, obj)) { return true; }

            if (ReferenceEquals(obj, null)) { return false; }

            throw new NotImplementedException();
        }
    }
}