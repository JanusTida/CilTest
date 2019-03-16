
using StandardDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

class Program {
    static void Main(string[] args) {
        var en = En2.Do(2);
        
    }
    
    private static void TestUnBoxGeneric<T>(object ob) {
        if(ob is T t) {
            Console.WriteLine($"It's a int.{t}");
        }
    }
    private static object TestUnBox(object ob) {
        if(ob is int intVal) {
            var s = intVal;
            return s;
        }
        else if(ob is string strVal){
            return new En(strVal);
        }

        return null;
    }

    private static void TestGeneric() {
        var list = new List<int>(1024);
        list.Add(0);
        var int0Arr = new int[0];
        var int7Arr = new int[7];
        var int8Arr = new int[8];
    }

    private const int AddNum = 128;

    private static void TestEmitConsole() {
        var tidaMethod = new DynamicMethod("fuckyou", typeof(string), null);

        var ilgenerator = tidaMethod.GetILGenerator();
        ilgenerator.DeclareLocal(typeof(string));

        ilgenerator.Emit(OpCodes.Ldstr, "Please enter the word you wonna say.");
        ilgenerator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new Type[] { typeof(string) }));
        ilgenerator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.ReadLine)));

        ilgenerator.Emit(OpCodes.Stloc_0);

        ilgenerator.Emit(OpCodes.Ldstr, "You have input:");
        ilgenerator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new Type[] { typeof(string) }));

        ilgenerator.Emit(OpCodes.Ldloc, 0);
        ilgenerator.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.WriteLine), new Type[] { typeof(string) }));

        ilgenerator.Emit(OpCodes.Ldloc, 0);
        ilgenerator.Emit(OpCodes.Ret);

        var dele = (Func<string>)tidaMethod.CreateDelegate(typeof(Func<string>));
        var word = dele();
    }

    private static void TestConditions() {
        int x = 7, y = short.MaxValue;
        if (x > y) {
            Console.WriteLine(x);
        }
        else {
            Console.WriteLine(y);
        }

        int s = 12;
        int s2 = s * 1;
    }

    private static void TestMembers() {
        var entity = new Entity();
        var strValue = entity.GetStringValue();
        var intValue = entity.IntVal;
        var fieldString = entity.FieldString;

        Console.WriteLine(strValue);
        Console.WriteLine(fieldString);
        Console.WriteLine(intValue);

        entity.IntVal = "suck";
        entity.FieldString = "ss";

        entity.ShowString("S");
    }
}

class En {
    public En(string s) {

    }
    public En(int s) { }
    public En() {

    }

    public string Value { get; set; } = "Fuckyou";
}

class En2 : En {
    public En2(string s) : base(s) {

    }
    public En2() : base("2") {

    }

    private En _en = new En();
    public void DODO(string s) {
        _en.Value = s;
    }

    public static object Do(object s) {
        if(s is int intVal) {
            return new En(intVal);
        }

        return null;
    }
}