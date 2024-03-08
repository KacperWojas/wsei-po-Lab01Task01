using System.ComponentModel;
using AggregateException = System.AggregateException;

namespace Lab01Task01;
//
public enum WeightUnits
{
    G = 1,
    DAG = 10,
    KG = 1_000,
    T  = 1_000_000,
    LB = (int)453.59237,
    OZ = (int)28.3495
}

public class Weight : IEquatable<Weight>, IComparable
{
    double Value{ get; init; }
    WeightUnits Unit{ get; init; }
    private Weight(){

    }
    public static Weight Of(double value, WeightUnits unit){
        //if(value<0) throw new ArgumentException("Ujemna wartość masy!");
        return new Weight(){Value = value, Unit = unit};
    }
    public static Weight Parse(string s){
        string[]? splitString = s.Split(" ");
        if(!double.TryParse(splitString[0], out double value)){
            //throw new ArgumentException("Niepoprawny format liczby określającej masę!");
        }
        if(value<0){
            //throw new ArgumentException("Ujemna wartość masy!");
        }
        WeightUnits unit;
        switch(splitString[1]){
            case "g":
                unit=WeightUnits.G;
                break;
            case "dag":
                unit=WeightUnits.DAG;
                break;
            case "kg":
                unit=WeightUnits.KG;
                break;
            case "t":
                unit=WeightUnits.T;
                break;
            case "lb":
                unit=WeightUnits.LB;
                break;
            case "oz":
                unit=WeightUnits.OZ;
                break;
            default:
                //throw new ArgumentException("Nieznana jednostka masy!");
                unit=WeightUnits.G;
                break;
        }
        return Weight.Of(value, unit);
    }
    private double ToGram(){
        return Value*(double)Unit;
    }
    public bool Equals(Weight? other)
    {
        if(other is null) return false;
        return this.ToGram()==other.ToGram();
    }
    public override bool Equals(object? other)
    {
        if(other is null) return false;
        if(other is not Weight) return false;
        return this==(Weight)other;
    }
    public static int Compare(Weight a, Weight b)
    {
        if(a.ToGram()>b.ToGram()) return 1;
        if(a.ToGram()<b.ToGram()) return -1;
        return 0;
    }
    public int CompareTo(object? other)
    {
        return Compare(this, (Weight)other);
    }
    public static bool operator ==(Weight a, Weight b){
        if(a is null || b is null) return false;
        return a.Equals(b);
    }
    public static bool operator !=(Weight a, Weight b){
        return !(a.ToGram()==b.ToGram());
    }
    public static bool operator >(Weight a, Weight b){
        if(Weight.Compare(a,b)==1) return true;
        return false;
    }
    public static bool operator <(Weight a, Weight b){
        if(Weight.Compare(a,b)==-1) return true;
        return false;
    }
    public override string ToString(){
        return this.Value.ToString() + " " + this.Unit.ToString().ToLower();
    }
    public static Weight operator +(Weight a, Weight b){
        if(a.Unit>b.Unit)
            return Weight.Of((a.ToGram()+b.ToGram())/(double)a.Unit,a.Unit);
        return Weight.Of((a.ToGram()+b.ToGram())/(double)b.Unit,b.Unit);
    }
    public Weight ToUnit(WeightUnits unit){
        return Weight.Of(this.ToGram()/(double)this.Unit,this.Unit);
    }
}