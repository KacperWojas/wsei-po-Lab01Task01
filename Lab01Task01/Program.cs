// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Lab01Task01;
using Weight = Lab01Task01.Weight;
using WeightUnits = Lab01Task01.WeightUnits;

Test test = new Test();
// Zadanie 1
var enumNames = Enum.GetNames<WeightUnits>().Select(n => n.ToLower());
test.AddCase(
    new TestCase("Lab01 Zadanie1 1", "Brak stałej dla kilogramów!", "Stała KG istnieje",
        () => enumNames.Contains("kg"), 1));
test.AddCase(new TestCase("Lab01 Zadanie1 2", "Brak stałej dla gramów!", "Stała G istnieje",
    () => enumNames.Contains("g"), 1));
test.AddCase(new TestCase("Lab01 Zadanie1 3", "Brak stałej dla dekagramów!", "Stała DAG istnieje",
    () => enumNames.Contains("dag"), 1));
test.AddCase(new TestCase("Lab01 Zadanie1 4", "Brak stałej dla tony!", "Stała T istnieje",
    () => enumNames.Contains("t"),
    1));
test.AddCase(new TestCase("Lab01 Zadanie1 5", "Brak stałej dla uncji!", "Stała OZ istnieje",
    () => enumNames.Contains("oz"), 1));
test.AddCase(new TestCase("Lab01 Zadanie1 6", "Brak stałej dla funtów!", "Stała LB istnieje",
    () => enumNames.Contains("lb"), 1));
// Zadanie 2
Object? weight = Activator.CreateInstance(typeof(Weight), true);
if (weight is null)
{
    return;
}
Type wType = weight.GetType();
// Value property
test.AddCase(
    new TestCase("Lab01 Zadanie2 1", "Brak właściwości Value!", "Właściwość Value istnieje",
        () => { return wType.GetProperty("Value") is not null; }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 2", "Value ma niewłaściwy typ!", "Właściwość Value ma typ double",
        () =>
        {
            var prop = wType.GetProperty("Value");
            if (prop is null)
            {
                return false;
            }
            return prop.PropertyType.Name == "Double";
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 3", "Value nie ma metody get!", "Właściwość Value ma metodę get",
        () =>
        {
            var prop = wType.GetProperty("Value");
            if (prop is null)
            {
                return false;
            }
            return prop.GetGetMethod() is not null;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 4", "Value nie może posiadać metody set!", "Właściwość Value nie ma metody set",
        () =>
        {
            var prop = wType.GetProperty("Value");
            if (prop is null)
            {
                return false;
            }

            return prop.IsInitOnly();
        }, 1));
// Unit property
test.AddCase(
    new TestCase("Lab01 Zadanie2 5", "Brak właściwości Unit!", "Właściwość Unit istnieje",
        () => { return wType.GetProperty("Value") is not null; }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 6", "Unit ma niewłaściwy typ!", "Właściwość Unit ma typ WeightUnits",
        () =>
        {
            var prop = wType.GetProperty("Unit");
            if (prop is null)
            {
                return false;
            }
            return prop.PropertyType.Name == "WeightUnits";
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 7", "Unit nie ma metody get!", "Właściwość Unit ma metodę get",
        () =>
        {
            var prop = wType.GetProperty("Unit");
            if (prop is null)
            {
                return false;
            }
            return prop.GetGetMethod() is not null;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie2 8", "Unit nie może posiadać metody set!", "Właściwość Unit nie ma metody set",
        () =>
        {
            var prop = wType.GetProperty("Unit");
            if (prop is null)
            {
                return false;
            }
            return prop.IsInitOnly();
        }, 1));
// Zadanie 3
test.AddCase(
    new TestCase("Lab01 Zadanie3 1", "Klasa nie posiada bezargumentowego, prywatnego konstruktora!", "Wymagany konstruktor obency",
        () =>
        {
            var constructorInfo = wType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            return constructorInfo is not null && constructorInfo.IsPrivate;
        }, 1));
// Zadanie 4
test.AddCase(
    new TestCase("Lab01 Zadanie4 1", "Klasa nie posiada metody statycznej Of!", "Metoda Of obecna!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of");
            return methodInfo is not null && methodInfo.IsPublic;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie4 2", "Metoda Of nie jest statyczna!", "Metoda Of jest statyczna!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of");
            return methodInfo is not null && methodInfo.IsStatic;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie4 3", "Metoda Of nie posiada wymaganych parametrów!", "Metoda Of posiada wymagane parametry!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of", new Type[]{typeof(double), typeof(WeightUnits)});
            return methodInfo is not null;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie4 4", "Metoda Of nie zwraca typu Weight!", "Metoda Of zwraca typ Weight!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of", new Type[]{typeof(double), typeof(WeightUnits)});
            return methodInfo?.ReturnType.Name == "Weight";
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie4 5", "Metoda nie może zwracać obiektu dla ujemnej masy!", "Metoda Of zgłasza wyjątek!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of", new Type[]{typeof(double), typeof(WeightUnits)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { -10.0, Enum.Parse<WeightUnits>("KG") });
                return false;
            }
            catch
            {
                return true;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie4 6", "Metoda Of nie zwraca obiektu dla poprawnych danych!", "Metoda Of zwraca obiekt!",
        () =>
        {
            var methodInfo = wType.GetMethod("Of", new Type[]{typeof(double), typeof(WeightUnits)});
            try
            {
                var invoke = methodInfo.Invoke(weight, new object[] { 10.0, Enum.Parse<WeightUnits>("KG") });
                return true;
            }
            catch
            {
                return false;
            }
        }, 1));
// Zadanie 5
test.AddCase(
    new TestCase("Lab01 Zadanie5 1", "Klasa nie posiada metody statycznej Parse!", "Metoda Parse obecna!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse");
            return methodInfo is not null && methodInfo.IsPublic;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 2", "Metoda Parse nie jest statyczna!", "Metoda Parse jest statyczna!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse");
            return methodInfo is not null && methodInfo.IsStatic;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 3", "Metoda Parse nie zwraca typu Weight lub nie posiada argumentu typy string!", "Metoda Parse zwraca typ Weight!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            return methodInfo?.ReturnType.Name == "Weight";
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 4", "Metoda nie zwraca obiektu dla poprawnego łańcucha '13.45 kg'!", "Metoda Of zwraca obiekt!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { "13,45 kg" });
                return invoke is not null;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 5", "Metoda nie zwraca obiektu dla poprawnego łańcucha '1345 G'!", "Metoda Of zwraca obiekt!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { "1345 G" });
                return invoke is not null;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 6", "Metoda nie zgłasza wyjatku dla niepoprawnego łańcucha '-13.45 kg'!", "Metoda Of zgłasza wyjątek!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { "-13,45 kg" });
                return false;
            }
            catch
            {
                return true;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 7", "Metoda nie zgłasza wyjatku dla niepoprawnego łańcucha '134a kg'!", "Metoda Of zgłasza wyjątek!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { "1345a kg" });
                return invoke is not null;
            }
            catch
            {
                return true;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie5 8", "Metoda nie zgłasza wyjatku dla niepoprawnego łańcucha '124 hj'!", "Metoda Of zgłasza wyjątek!",
        () =>
        {
            var methodInfo = wType.GetMethod("Parse", new Type[]{typeof(string)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { "124 hj" });
                return invoke is not null;
            }
            catch
            {
                return true;
            }
        }, 1));
// Zadanie 6
test.AddCase(
    new TestCase("Lab01 Zadanie6 1", "Klasa nie posiada prywatnej metody ToGram!", "Metoda ToGram obecna!",
        () =>
        {
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance);
            return methodInfo is not null && methodInfo.IsPrivate;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie6 2", "Metoda ToGram nie jest instancyjna!", "Metoda prywatna ToGram jest instancyjna!",
        () =>
        {
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance);
            return methodInfo is { IsPublic: false, IsStatic: false };
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie6 3", "Metoda ToGram nie zwraca typu double lub posiada argumenty!", "Metoda ToGram zwraca typ double!",
        () =>
        {
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            return methodInfo?.ReturnType.Name == "Double";
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie6 4", "Metoda nie zwraca poprawnej masy w gramach dla obiektu 10 kg!", "Metoda ToGram zwraca poprawną wartość!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 10);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("KG"));
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { });
                double result = invoke is double ? (double)invoke : 0;
                return Math.Abs(result - 10_000.0) < 0.0001;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie6 5", "Metoda nie zwraca poprawnej masy w gramach dla obiektu 14.5 lb!", "Metoda ToGram zwraca poprawną wartość!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 14.5);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("LB"));
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { });
                double result = invoke is double ? (double)invoke : 0;
                return Math.Abs(result - 6577.089365) < 0.0001;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie6 6", "Metoda nie zwraca poprawnej masy w gramach dla obiektu 2.3 oz!", "Metoda ToGram zwraca poprawną wartość!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 2.3);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("ToGram", BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { });
                double result = invoke is double ? (double)invoke : 0;
                return Math.Abs(result - 65.2039031875) < 0.0001;
            }
            catch
            {
                return false;
            }
        }, 1));
// Zadanie 7
test.AddCase(
    new TestCase("Lab01 Zadanie7 1", "Klasa nie implementuje interfejsu IEquatable!", "Klasa implementuje interfejs IEquatable!",
        () =>
        {
            return wType.GetInterfaces().Count(i => i is { IsGenericType: true, Name: "IEquatable`1" }) == 1;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie7 2", "Klasa niepoprawnie implementuje interfesj IEquitable!", "Klasa poprawnie implementuje IEquatable!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 5.2);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("KG"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 5200.0);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("G"));
            var methodInfo = wType.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, new[] {typeof(Weight)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { other});
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie7 3", "Klasa niepoprawnie implementuje interfesj IEquitable!", "Klasa poprawnie implementuje IEquatable!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 1.3);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 589.670081);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("G"));

            var methodInfo = wType.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, new[] {typeof(Weight)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { other});
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch
            {
                return false;
            }
        }, 1));
// Zadanie 8
test.AddCase(
    new TestCase("Lab01 Zadanie8 1", "Klasa nie implementuje interfejsu IComparable!", "Klasa implementuje interfejs IComparable!",
        () =>
        {
            return wType.GetInterfaces().Where(i => i.IsGenericType && i.Name == "IComparable`1").Count() == 1;
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie8 2", "Klasa niepoprawnie implementuje interfesj IComparable!", "Klasa poprawnie implementuje IComparable!",
        () =>
        {
            Object? current = Activator.CreateInstance(typeof(Weight), true);
            weight.GetType().GetProperty("Value")?.SetValue(current, 5.3);
            weight.GetType().GetProperty("Unit")?.SetValue(current, Enum.Parse<WeightUnits>("KG"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 5200.0);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("G"));
            var methodInfo = current?.GetType().GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, new[] {typeof(Weight)});
            try
            {
                var invoke = methodInfo?.Invoke(current, new object[] { other});
                int result = invoke is int ? (int)invoke : 0;
                return result > 0;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie8 3", "Klasa niepoprawnie implementuje interfesj IComparable!", "Klasa poprawnie implementuje IComparable!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 1);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 589.6696);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("G"));
            var methodInfo = wType.GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, new[] {typeof(Weight)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { other});
                int result = invoke is int ? (int)invoke : 0;
                return result < 0;
            }
            catch
            {
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie8 4", "Klasa niepoprawnie implementuje interfesj IComparable!", "Klasa poprawnie implementuje IComparable!",
        () =>
        {
            weight.GetType().GetProperty("Value")?.SetValue(weight, 10);
            weight.GetType().GetProperty("Unit")?.SetValue(weight, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 160);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, new[] {typeof(Weight)});
            try
            {
                var invoke = methodInfo?.Invoke(weight, new object[] { other});
                if (invoke is null)
                {
                    return false;
                }
                int result = invoke is int ? (int)invoke : 0;
                return result == 0;
            }
            catch
            {
                return false;
            }
        }, 1));
// Zadanie 9
test.AddCase(
    new TestCase("Lab01 Zadanie9 1", "Klasa niepoprawnie implementuje operator <!", "Klasa poprawnie implementuje operator <!",
        () =>
        {
            Object? current = Activator.CreateInstance(typeof(Weight), true);
            current?.GetType().GetProperty("Value")?.SetValue(current, 9.99);
            current?.GetType().GetProperty("Unit")?.SetValue(current, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 160);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("op_LessThan", BindingFlags.Static | BindingFlags.Public);
            try
            {
                var invoke = methodInfo?.Invoke(null, new object[] { current, other });
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie9 2", "Klasa niepoprawnie implementuje operator >!", "Klasa poprawnie implementuje operator >!",
        () =>
        {
            Object? current = Activator.CreateInstance(typeof(Weight), true);
            current?.GetType().GetProperty("Value")?.SetValue(current, 10.1);
            current?.GetType().GetProperty("Unit")?.SetValue(current, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 160);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("op_GreaterThan", BindingFlags.Static | BindingFlags.Public);
            try
            {
                var invoke = methodInfo?.Invoke(null, new object[] { current, other });
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie9 3", "Klasa niepoprawnie implementuje operator ==!", "Klasa poprawnie implementuje operator ==!",
        () =>
        {
            Object? current = Activator.CreateInstance(typeof(Weight), true);
            current?.GetType().GetProperty("Value")?.SetValue(current, 10);
            current?.GetType().GetProperty("Unit")?.SetValue(current, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 160);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
            try
            {
                var invoke = methodInfo?.Invoke(null, new object[] { current, other });
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }, 1));
test.AddCase(
    new TestCase("Lab01 Zadanie8 4", "Klasa niepoprawnie implementuje operator !=!", "Klasa poprawnie implementuje operator !=!",
        () =>
        {
            Object? current = Activator.CreateInstance(typeof(Weight), true);
            current?.GetType().GetProperty("Value")?.SetValue(current, 11);
            current?.GetType().GetProperty("Unit")?.SetValue(current, Enum.Parse<WeightUnits>("LB"));
            Object? other = Activator.CreateInstance(typeof(Weight), true);
            other?.GetType().GetProperty("Value")?.SetValue(other, 160);
            other?.GetType().GetProperty("Unit")?.SetValue(other, Enum.Parse<WeightUnits>("OZ"));
            var methodInfo = wType.GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
            try
            {
                var invoke = methodInfo?.Invoke(null, new object[] { current, other });
                bool result = invoke is bool ? (bool)invoke : false;
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }, 1));
test.RunAllCases();