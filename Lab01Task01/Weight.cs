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

public class Weight : IEquatable<Weight>, IComparable<Weight>
{
    public double Value{ get; init; }
    public WeightUnits Unit{ get; init; }
    private double UnitValue{ 
        get
        {
            switch(Unit){
            case WeightUnits.G:
                return 1;
            case WeightUnits.DAG:
                return 10;
            case WeightUnits.KG:
                return 1_000;
            case WeightUnits.T:
                return 1_000_000;
            case WeightUnits.LB:
                return 453.59237;
            case WeightUnits.OZ:
                return 28.3495;
            default:
                throw new ArgumentException("Nieznana jednostka masy!");
        }
        } 
    }
    private Weight(){

    }
    public static Weight Of(double value, WeightUnits unit){
        if(value<0) throw new ArgumentException("Ujemna wartość masy!");
        return new Weight(){Value = value, Unit = unit};
    }
    public static Weight Parse(string s){
        string[]? splitString = s.Split(" ");
        if(!double.TryParse(splitString[0], out double value)){
            throw new ArgumentException("Niepoprawny format liczby określającej masę!");
        }
        if(value<0){
            throw new ArgumentException("Ujemna wartość masy!");
        }
        WeightUnits unit;
        WeightUnits.TryParse(splitString[1].ToUpper(),out unit);
        return Weight.Of(value, unit);
    }
    private double ToGram(){
        return Math.Round((double)Value*UnitValue,6);
    }
/*    public bool Equals(Weight? other)
    {
        if(other is null) return false;
        return this.ToGram()==other.ToGram();
    }
    public override bool Equals(object? other)
    {
        if(other is null) return false;
        if(other is not Weight) return false;
        return this==(Weight)other;
    }*/
    public static int Compare(Weight a, Weight b)
    {
        if(Math.Round(a.ToGram(),2)>Math.Round(b.ToGram(),2)) return 1;
        if(Math.Round(a.ToGram(),2)<Math.Round(b.ToGram(),2)) return -1;
        return 0;
    }
        public int CompareTo(Weight? other)
    {
        return Compare(this, other);
    }

    public int CompareTo(object? other)
    {
        if(other is not Weight) throw new ArgumentException("");
        if(other is null) throw new ArgumentException("");
        return CompareTo((Weight)other);
    }
    public static bool operator ==(Weight a, Weight b){
        if(a is null || b is null) return false;
        return a.CompareTo(b)==0;
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
        return this.Value.ToString() + " " + this.Unit.ToString().ToLower()+" "+ToGram();
    }
    public static Weight operator +(Weight a, Weight b){
        if(a.Unit>b.Unit)
            return Weight.Of((a.ToGram()+b.ToGram())/(double)a.UnitValue,a.Unit);
        return Weight.Of((a.ToGram()+b.ToGram())/(double)b.UnitValue,b.Unit);
    }
    public Weight ToUnit(WeightUnits unit){
        return Weight.Of(this.ToGram()/(double)this.UnitValue,this.Unit);
    }

    public bool Equals(Weight? other)
    {
        return ToGram()== other.ToGram();
    }
}