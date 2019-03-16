using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    internal delegate object CreateInstanceHandler(object[] parameters);



    public partial class CreateObjectFactory {

        static Dictionary<string, CreateInstanceHandler> mHandlers = new Dictionary<string, CreateInstanceHandler>();



        public static T CreateInstance<T>() where T : class {

            return CreateInstance<T>(null);

        }



        public static T CreateInstance<T>(params object[] parameters) where T : class {

            return (T)CreateInstance(typeof(T), parameters);

        }

        public static T CreateInstance2<T>() {
            if(EmitStateCell<T>.ConstructHandler == null) {
                throw new InvalidOperationException($"We failed to create construct handler for {typeof(T)}");
            }

            return EmitStateCell<T>.ConstructHandler();
        }

        public static T CreateInstance2<T,TPara>(TPara para) {
            if(EmitStateCell<TPara,T>.ConstructHandler == null) {
                throw new InvalidOperationException($"We failed to create construct handler for {typeof(T)}");
            }

            return EmitStateCell<TPara, T>.ConstructHandler(para);
        }

        public static object CreateInstance(Type instanceType, params object[] parameters) {

            Type[] ptypes = new Type[0];

            string key = instanceType.FullName;



            if (parameters != null && parameters.Any()) {

                ptypes = parameters.Select(t => t.GetType()).ToArray();

                key = string.Concat(key, "_", string.Concat(ptypes.Select(t => t.Name)));

            }



            if (!mHandlers.ContainsKey(key)) {

                CreateHandler(instanceType, key, ptypes);

            }

            return mHandlers[key](parameters);

        }



        static void CreateHandler(Type objtype, string key, Type[] ptypes) {

            lock (typeof(CreateObjectFactory)) {

                if (!mHandlers.ContainsKey(key)) {

                    DynamicMethod dm = new DynamicMethod(key, typeof(object), new Type[] { typeof(object[]) }, typeof(CreateObjectFactory).Module);
                    
                    ILGenerator il = dm.GetILGenerator();

                    ConstructorInfo cons = objtype.GetConstructor(ptypes);



                    if (cons == null) {

                        throw new MissingMethodException("The constructor for the corresponding parameter was not found");

                    }



                    il.Emit(OpCodes.Nop);



                    for (int i = 0; i < ptypes.Length; i++) {

                        il.Emit(OpCodes.Ldarg_0);

                        il.Emit(OpCodes.Ldc_I4, i);

                        il.Emit(OpCodes.Ldelem_Ref);

                        if (ptypes[i].IsValueType)

                            il.Emit(OpCodes.Unbox_Any, ptypes[i]);

                        else

                            il.Emit(OpCodes.Castclass, ptypes[i]);

                    }



                    il.Emit(OpCodes.Newobj, cons);

                    il.Emit(OpCodes.Ret);

                    CreateInstanceHandler ci = (CreateInstanceHandler)dm.CreateDelegate(typeof(CreateInstanceHandler));

                    mHandlers.Add(key, ci);

                }

            }

        }

    }

    public partial class CreateObjectFactory {
        private static Func<ClassB> _createBFunc = null;
        private static Func<ClassB> CreateCreateBFunc() {
            DynamicMethod dm = new DynamicMethod("CreateB", typeof(ClassB),Type.EmptyTypes);

            ILGenerator il = dm.GetILGenerator();

            ConstructorInfo cons = typeof(ClassB).GetConstructor(Type.EmptyTypes);

            il.Emit(OpCodes.Newobj, cons);
            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<ClassB>)) as Func<ClassB>;
        }
        public static ClassB CreateB() {
            if(_createBFunc == null) {
                _createBFunc = CreateCreateBFunc();
            }
            return _createBFunc();
        }

        private static Func<ClassB, ClassA> _createAFunc = null;
        private static Func<ClassB,ClassA> CreateCreateAFunc() {
            var createAArgTypes = new Type[] { typeof(ClassB) };

            DynamicMethod dm = new DynamicMethod("CreateA", typeof(ClassA),createAArgTypes);

            ILGenerator il = dm.GetILGenerator();

            ConstructorInfo cons = typeof(ClassA).GetConstructor(createAArgTypes);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, cons);
            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<ClassB, ClassA>)) as Func<ClassB, ClassA>;
        }
        public static ClassA CreateA(ClassB classB) {
            if(_createAFunc == null) {
                _createAFunc = CreateCreateAFunc();
            }

            return _createAFunc(classB);
        }
    }
}
