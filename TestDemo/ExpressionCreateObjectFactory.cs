using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestDemo {
    public class ClassA {

        public ClassA(ClassB classB) { }

        public int GetInt() => default(int);

    }

    public class ClassB {

        public int GetInt() => default(int);

    }



    /// <summary>

    /// 为了测试Expression实现方式和直接New/Emit方式的对比，这里写一个最简单的Demo进行对比

    /// </summary>

    public class ExpressionCreateObjectFactory {

        private static Dictionary<string, Func<object[], object>> funcDic = new Dictionary<string, Func<object[], object>>();

        public static T CreateInstance<T>() where T : class {

            return CreateInstance(typeof(T), null) as T;

        }



        public static T CreateInstance<T>(params object[] parameters) where T : class {

            return CreateInstance(typeof(T), parameters) as T;

        }



        static Expression[] BuildParameters(Type[] parameterTypes, ParameterExpression paramExp) {

            List<Expression> list = new List<Expression>();

            for (int i = 0; i < parameterTypes.Length; i++) {

                //从参数表达式（参数是：object[]）中取出参数

                var arg = BinaryExpression.ArrayIndex(paramExp, Expression.Constant(i));

                //把参数转化成指定类型

                var argCast = Expression.Convert(arg, parameterTypes[i]);



                list.Add(argCast);

            }

            return list.ToArray();

        }



        public static object CreateInstance(Type instanceType, params object[] parameters) {



            Type[] ptypes = new Type[0];

            string key = instanceType.FullName;



            if (parameters != null && parameters.Any()) {

                ptypes = parameters.Select(t => t.GetType()).ToArray();

                key = string.Concat(key, "_", string.Concat(ptypes.Select(t => t.Name)));

            }



            if (!funcDic.ContainsKey(key)) {

                ConstructorInfo constructorInfo = instanceType.GetConstructor(ptypes);



                //创建lambda表达式的参数

                var lambdaParam = Expression.Parameter(typeof(object[]), "_args");



                //创建构造函数的参数表达式数组

                var constructorParam = BuildParameters(ptypes, lambdaParam);



                var newExpression = Expression.New(constructorInfo, constructorParam);



                funcDic.Add(key, Expression.Lambda<Func<object[], object>>(newExpression, lambdaParam).Compile());

            }

            return funcDic[key](parameters);

        }

    }





    /// <summary>

    /// 为了测试Expression代参数兼容性能损耗，特加个最简单的

    /// </summary>

    public class ExpressionCreateObject {

        private static Func<object> func;

        public static T CreateInstance<T>() where T : class {

            if (func == null) {

                var newExpression = Expression.New(typeof(T));

                func = Expression.Lambda<Func<object>>(newExpression).Compile();

            }

            return func() as T;

        }

    }


    
}
