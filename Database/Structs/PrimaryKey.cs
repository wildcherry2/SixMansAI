
using System.Collections;
using Database.Database;
using Database.Database.Interfaces;

namespace Database.Structs; 

public struct PrimaryKey : IComparable<PrimaryKey> { 
    public ulong           key { get; init; }
    public EPrimaryKeyType key_type = EPrimaryKeyType.UNKNOWN;
    public PrimaryKey(in ulong key, in EPrimaryKeyType key_type) { 
        this.key = key;
        this.key_type = key_type;
    }
    public PrimaryKey(in IDatabaseComponent component) { key = component.TryGetOrCreatePrimaryKey(); }
    public ComponentType? GetRecord<ComponentType>() where ComponentType : IDatabaseComponent {
        var res = QueryFactory.GetSingleton().Query<ComponentType>(this);
        return (ComponentType?)res;
    }
    public static bool operator ==(in PrimaryKey lhs, in PrimaryKey rhs) {
        return lhs.key == rhs.key;
    }
    public static bool operator !=(in PrimaryKey lhs, in PrimaryKey rhs) {
        return lhs.key != rhs.key;
    }
    public static bool operator <(in PrimaryKey lhs, in PrimaryKey rhs) {
        return lhs.key < rhs.key;
    }
    public static bool operator >(in PrimaryKey lhs, in PrimaryKey rhs) {
        return rhs.key > lhs.key;
    }
    public static bool operator <=(in PrimaryKey lhs, in PrimaryKey rhs) {
        return (lhs.key <= rhs.key);
    }
    public static bool operator >=(in PrimaryKey lhs, in PrimaryKey rhs) {
        return lhs.key >= rhs.key;
    }
    public static implicit operator ulong(in PrimaryKey key) {
        return key.key;
    }
    
    public bool Equals(in PrimaryKey other) {
        return key == other.key;
    }
    public override bool Equals(object? obj) {
        return obj is PrimaryKey other && Equals(other);
    }
    public override int GetHashCode() {
        return key.GetHashCode();
    }
    public int CompareTo(PrimaryKey other) {
        if (other == null) return 1;

        return key.CompareTo(other.key);
    }
}