//change abstract members to virtual where applicable? wouldnt have to reimplement everywhere

using Newtonsoft.Json;

namespace Database.Database.Interfaces {
    public abstract class IDatabaseComponent : ILogger, IPrimaryKey {
        protected ulong primary_key = default_incremental_primary_key++;
        protected IDatabaseComponent(in ConsoleColor color = ConsoleColor.White, in int tabs = 0, in string class_name = "IDatabaseComponent") : base(color, tabs, class_name) { }
        private static   IDatabaseComponent? singleton                       { get; }
        public           bool                bError                          { get; set; } = false;
        protected static ulong               default_incremental_primary_key { get; set; }
        protected        bool                bIsPrimaryKeySet                { get; set; }

        // Factories can call an override of this to set a primary key unique to the class, no need for this with singletons.
        // Overrides should decrement the default key for consistency, and only set the key if the bool indicates it isn't already set.
        public virtual ulong TryGetOrCreatePrimaryKey() {
            if (bIsPrimaryKeySet) { return primary_key; }

            primary_key = default_incremental_primary_key++;
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

        protected abstract bool IsEqual(IDatabaseComponent?    rhs);
        protected abstract bool IsLessThan(IDatabaseComponent? rhs);

        public virtual string ToJson() { return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }); }

        public abstract void ToJson(string   save_path);
        public abstract void FromJson(string save_path);

        public override bool Equals(object? obj) {
            if (ReferenceEquals(this, obj)) { return true; }

            if (ReferenceEquals(obj, null)) { return false; }

            throw new NotImplementedException();
        }
    }
}