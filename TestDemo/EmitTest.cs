using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDemo {
    [TestClass]
    public class EmitTest {
        private const string AsmName = "TidaAssembly";
        private const string ModuleName = "TidaModule";
        private const string DllPostFix = ".dll";
        private const string ClassName = "TidaEntity";

        public TestContext TestContext { get; set; }

        
        public class TidaEntity {
            private int _value;
            public int Value {
                get => _value;
                set => _value = value;
            }
        }

        public class TidaEntityJsonWrapper {
            public string WriterToJson(TidaEntity tidaEntity) {
                return $"{nameof(tidaEntity.Value)}:{tidaEntity}";
            }
        }

        [TestMethod]
        public void TestPropertyEmit() {
            
            var asmName = new AssemblyName(AsmName);

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assembly.DefineDynamicModule(ModuleName, asmName + DllPostFix);
            
            var typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public);
            
            var noParamsCtor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);


            ILGenerator ctor0IL = noParamsCtor.GetILGenerator();
            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before pushing the default
            // value on the stack, then call constructor ctor1.
            ctor0IL.Emit(OpCodes.Ret);

            var getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var valueFieldBuilder = typeBuilder.DefineField("_value", typeof(int), FieldAttributes.Private);
            
            var valuePropertyBuilder = typeBuilder.DefineProperty("Value", PropertyAttributes.None,typeof(int),new Type[] { typeof(int) });

            var getValueMethodBuilder = typeBuilder.DefineMethod("get_Value", getSetAttr, typeof(int), Type.EmptyTypes);
            var setValueMethodBuilder = typeBuilder.DefineMethod("set_Value", getSetAttr, null,new Type[] { typeof(int) });

            var getValueILGenerator = getValueMethodBuilder.GetILGenerator();

            getValueILGenerator.Emit(OpCodes.Ldarg_0);
            getValueILGenerator.Emit(OpCodes.Ldfld, valueFieldBuilder);
            getValueILGenerator.Emit(OpCodes.Ret);

            valuePropertyBuilder.SetGetMethod(getValueMethodBuilder);

            var setValueILGenerator = setValueMethodBuilder.GetILGenerator();

            setValueILGenerator.Emit(OpCodes.Ldarg_0);
            setValueILGenerator.Emit(OpCodes.Ldarg_1);
            setValueILGenerator.Emit(OpCodes.Stfld, valueFieldBuilder);
            setValueILGenerator.Emit(OpCodes.Ret);

            valuePropertyBuilder.SetSetMethod(setValueMethodBuilder);

            var tp = typeBuilder.CreateType();
            
            var props = tp.GetProperties();
            assembly.Save(asmName.Name + DllPostFix);
        }

        [TestMethod]
        public void TestGenricEmit() {
            var asmName = new AssemblyName(AsmName);

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assembly.DefineDynamicModule(ModuleName, asmName + DllPostFix);

            var typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public);
            

            var listType = typeof(List<>);
            var myType = typeof(int);

            var myListType = listType.MakeGenericType(myType);
            var listType2 = myListType.GetGenericTypeDefinition();
            var s = listType == listType2;

            var exMethod = typeBuilder.DefineMethod("ExampleMethod",
                MethodAttributes.Public | MethodAttributes.Static,
                myListType,
                Type.EmptyTypes
            );

            var ilGenerator = exMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
            assembly.Save(asmName.Name + DllPostFix);
        }

        [TestMethod]
        public void TestGenericMethodEmit() {
            var asmName = new AssemblyName(AsmName);

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assembly.DefineDynamicModule(ModuleName, asmName + DllPostFix);

            var typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public);

            var listType = typeof(List<>);
            var myType = typeof(int);
            var myListType = listType.MakeGenericType(myType);

            var exMethod = typeBuilder.DefineMethod("ExampleMethod",
                MethodAttributes.Public | MethodAttributes.Static,
                myListType,
                Type.EmptyTypes
            );
            
            var ilGenerator = exMethod.GetILGenerator();

            ilGenerator.DeclareLocal(myListType);
            ilGenerator.Emit(OpCodes.Newobj, myListType.GetConstructor(Type.EmptyTypes));
            ilGenerator.Emit(OpCodes.Stloc_0);

            var addMethod = myListType.GetMethod(nameof(List<int>.Add));
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Callvirt, addMethod);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
            typeBuilder.CreateType();
            assembly.Save(asmName.Name + DllPostFix);
        }

        [TestMethod]
        public void TestInheritEmit() {
            var asmName = new AssemblyName(AsmName);

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assembly.DefineDynamicModule(ModuleName, asmName + DllPostFix);

            var typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public);
        }

        [TestMethod]
        public void TestEmitDemo() {
            AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder tb = mb.DefineType(
                "MyDynamicType",
                 TypeAttributes.Public);

            // Add a private field of type int (Int32).
            FieldBuilder fbNumber = tb.DefineField(
                "m_number",
                typeof(int),
                FieldAttributes.Private);

            // Define a constructor that takes an integer argument and 
            // stores it in the private field. 
            Type[] parameterTypes = { typeof(int) };
            ConstructorBuilder ctor1 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();
            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before calling the base
            // class constructor. Specify the default constructor of the 
            // base class (System.Object) by passing an empty array of 
            // types (Type.EmptyTypes) to GetConstructor.
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call,
                typeof(object).GetConstructor(Type.EmptyTypes));
            // Push the instance on the stack before pushing the argument
            // that is to be assigned to the private field m_number.
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbNumber);
            ctor1IL.Emit(OpCodes.Ret);

            // Define a default constructor that supplies a default value
            // for the private field. For parameter types, pass the empty
            // array of types or pass null.
            ConstructorBuilder ctor0 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                Type.EmptyTypes);

            ILGenerator ctor0IL = ctor0.GetILGenerator();
            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before pushing the default
            // value on the stack, then call constructor ctor1.
            ctor0IL.Emit(OpCodes.Ldarg_0);
            ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
            ctor0IL.Emit(OpCodes.Call, ctor1);
            ctor0IL.Emit(OpCodes.Ret);

            // Define a property named Number that gets and sets the private 
            // field.
            //
            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use the built-in array with no elements: Type.EmptyTypes)
            PropertyBuilder pbNumber = tb.DefineProperty(
                "Number",
                PropertyAttributes.HasDefault,
                typeof(int),
                null);

            // The property "set" and property "get" methods require a special
            // set of attributes.
            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method for Number. The method returns
            // an integer and has no arguments. (Note that null could be 
            // used instead of Types.EmptyTypes)
            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                "get_Number",
                getSetAttr,
                typeof(int),
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
            // For an instance property, argument zero is the instance. Load the 
            // instance, then load the private field and return, leaving the
            // field value on the stack.
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_Number",
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);

            // Define a method that accepts an integer argument and returns
            // the product of that integer and the private field m_number. This
            // time, the array of parameter types is created on the fly.
            MethodBuilder meth = tb.DefineMethod(
                "MyMethod",
                MethodAttributes.Public,
                typeof(int),
                new Type[] { typeof(int) });
            
            ILGenerator methIL = meth.GetILGenerator();
            // To retrieve the private instance field, load the instance it
            // belongs to (argument zero). After loading the field, load the 
            // argument one and then multiply. Return from the method with 
            // the return value (the product of the two numbers) on the 
            // execution stack.
         
            
            methIL.Emit(OpCodes.Ldarg_0);
            methIL.Emit(OpCodes.Ldfld, fbNumber);
            methIL.Emit(OpCodes.Ldarg_1);
            methIL.Emit(OpCodes.Mul);
            methIL.Emit(OpCodes.Ret);

            // Finish the type.
            Type t = tb.CreateType();

            // The following line saves the single-module assembly. This
            // requires AssemblyBuilderAccess to include Save. You can now
            // type "ildasm MyDynamicAsm.dll" at the command prompt, and 
            // examine the assembly. You can also write a program that has
            // a reference to the assembly, and use the MyDynamicType type.
            // 
            ab.Save(aName.Name + ".dll");
        }

        [TestMethod]
        public void TestReverse() {
            var res = Reverse(1534236469);
        }

        private const int PosMaxNumber = int.MaxValue / 10;
        private const int NegMaxNumber = -int.MaxValue / 10;

        public int Reverse(int x) {
            var temp = x;
            var res = 0;
            while (temp != 0) {
                var thisIndexNumber = temp % 10;
                if(res > PosMaxNumber || res < NegMaxNumber) {
                    return 0;
                }
                res *= 10;
                if (thisIndexNumber != 0) {
                    res += thisIndexNumber;
                }
                temp /= 10;
            }

            return res;
        }

        /// <summary>
        /// 乘法函数;
        /// </summary>
        /// <param name="number0"></param>
        /// <param name="number1"></param>
        /// <returns></returns>
        private static int Multi(int number0, int number1) => number0 * number1;

        /// <summary>
        /// 获取一个使用Emit创建的简单的乘法的委托;
        /// </summary>
        /// <returns></returns>
        private static Func<int, int, int> CreateEmitMultiDelegate() {
            var addMethod = new DynamicMethod(nameof(Multi), typeof(int), new Type[] { typeof(int), typeof(int) });

            var ilgenerator = addMethod.GetILGenerator();

            ///将第0(1)个参数入栈,此处使用了参数入栈的精简指令格式<see cref="OpCodes.Ldarg_0"/>,
            ///对应的参数非精简指令语句为
            ///ilgenerator.Emit(OpCodes.Ldarg, 0);
            ilgenerator.Emit(OpCodes.Ldarg_0);
            //同上,将第1(2)个参数入栈
            ilgenerator.Emit(OpCodes.Ldarg_1);

            ///使用乘法指令以进行数积运算,此操作会将栈中的两个元素取出，并将数积结果压入栈顶。
            ///由于CIL并非最终生成的代码,在内存的位运算中究竟如何处理数积运算,将由JIT处理得到;
            ilgenerator.Emit(OpCodes.Mul);

            ///返回,对于CIL代码函数,无论其是否具备返回值,都必须显示指定返回,否则CLR将会抛出程序不可用异常。
            ///对于具备返回值的函数,在返回时,栈中有且只能存在一个元素。
            ///反之,栈须为空。
            ilgenerator.Emit(OpCodes.Ret);

            return addMethod.CreateDelegate(typeof(Func<int, int, int>)) as Func<int, int, int>;
        }
        
    }

    public struct Entity {
        public int X { get; set; }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

    }
}
