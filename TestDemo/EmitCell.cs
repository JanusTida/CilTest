using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    static class EmitStateCell {
        /// <summary>
        /// 根据指定参数的类型,构建一个指定类型对象的动态方法;
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        public static DynamicMethod CreateConstructDelegate<TResult>(params Type[] paramTypes) {
            var returnType = typeof(TResult);
            var ctor = returnType.GetConstructor(paramTypes);

            if (ctor == null) {
                throw new MissingMethodException($"We can't find the proper constructor for {returnType}.");
            }

            var methodName = $"Create_{returnType.FullName}_{string.Concat(paramTypes.Select(p => p.Name))}";
            var dm = new DynamicMethod(methodName, returnType, paramTypes, typeof(CreateObjectFactory).Module);

            var il = dm.GetILGenerator();
            for (int i = 0; i < paramTypes.Length; i++) {
                switch (i) {
                    case 0:
                        il.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        il.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        il.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        il.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        il.Emit(OpCodes.Ldarg, i);
                        break;
                }
            }

            il.Emit(OpCodes.Newobj, ctor);
            
            il.Emit(OpCodes.Ret);

            return dm;
        }
    }

    /// <summary>
    /// 用于存储无参方式构造的状态元素;
    /// </summary>
    class EmitStateCell<TResult> {
        public static readonly Func<TResult> ConstructHandler = EmitStateCell.CreateConstructDelegate<TResult>().CreateDelegate(typeof(Func<TResult>)) as Func<TResult>;
    }

    /// <summary>
    /// 用于存储具有一个参数方式构造的状态元素;
    /// </summary>
    class EmitStateCell<TPara, TResult> {
    
        public static readonly Func<TPara, TResult> ConstructHandler = EmitStateCell.CreateConstructDelegate<TResult>(new Type[] { typeof(TPara) }).CreateDelegate(typeof(Func<TPara, TResult>)) as Func<TPara, TResult>;

        public static Func<object, object> CreateObjectHandler {
            get {
                if(_createObjectHandler == null) {
                    _createObjectHandler = (ob) => {
                        return ConstructHandler((TPara)ob);
                    };
                }

                return _createObjectHandler;
            }
        }

        private static Func<object, object> _createObjectHandler;
    }
    
    /*以此往后类推*/
}
