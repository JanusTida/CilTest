using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class ContructorPerformanceTest {

        [TestMethod]

        public void NoArguments() {

            ClassB b = CreateObjectFactory.CreateInstance<ClassB>();

            Assert.AreEqual(default(int), b.GetInt());

        }



        [TestMethod]

        public void GenerateArgument() {

            ClassA a = CreateObjectFactory.CreateInstance<ClassA>(new ClassB());

            Assert.AreEqual(default(int), a.GetInt());

        }



        [TestMethod]

        public void ExpressionType() {

            var instance = ExpressionCreateObjectFactory.CreateInstance<ClassB>();

            Assert.AreEqual(default(int), instance.GetInt());

        }





        [TestMethod]

        public void PerformanceReportWithNoArguments() {
            int count = 1000000;
            Trace.WriteLine($"#{count} 次调用:");



            double time = StopwatchHelper.Calculate(count, () => {

                ClassB b = new ClassB();

            }).TotalMilliseconds;

            Trace.WriteLine($"‘New’耗时 {time} milliseconds");



            double time2 = StopwatchHelper.Calculate(count, () => {

                ClassB b = CreateObjectFactory.CreateInstance2<ClassB>();

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Emit 工厂’耗时 {time2} milliseconds");



            double time3 = StopwatchHelper.Calculate(count, () => {

                ClassB b = ExpressionCreateObject.CreateInstance<ClassB>();

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Expression’耗时 {time3} milliseconds");



            double time4 = StopwatchHelper.Calculate(count, () => {

                ClassB b = ExpressionCreateObjectFactory.CreateInstance<ClassB>();

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Expression 工厂’耗时 {time4} milliseconds");



            double time5 = StopwatchHelper.Calculate(count, () => {

                ClassB b = Activator.CreateInstance<ClassB>();

                //ClassB b = Activator.CreateInstance(typeof(ClassB)) as ClassB;

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Activator.CreateInstance’耗时 {time5} milliseconds");





            /**

              #1000000 次调用:

                ‘New’耗时 21.7474 milliseconds

                ‘Emit 工厂’耗时 174.088 milliseconds

                ‘Expression’耗时 42.9405 milliseconds

                ‘Expression 工厂’耗时 162.548 milliseconds

                ‘Activator.CreateInstance’耗时 67.3712 milliseconds

             * */

        }


        [TestMethod]

        public void PerformanceReportWithArguments() {
            int count = 1000000;
            Trace.WriteLine($"#{count} 次调用:");



            double time = StopwatchHelper.Calculate(count, () => {

                ClassA a = new ClassA(new ClassB());

            }).TotalMilliseconds;

            Trace.WriteLine($"‘New’耗时 {time} milliseconds");



            double time2 = StopwatchHelper.Calculate(count, () => {

                ClassA a = CreateObjectFactory.CreateInstance2<ClassA,ClassB>(new ClassB());

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Emit 工厂’耗时 {time2} milliseconds");

            

            double time4 = StopwatchHelper.Calculate(count, () => {

                ClassA a = ExpressionCreateObjectFactory.CreateInstance<ClassA>(new ClassB());

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Expression 工厂’耗时 {time4} milliseconds");



            double time5 = StopwatchHelper.Calculate(count, () => {

                ClassA a = Activator.CreateInstance(typeof(ClassA), new ClassB()) as ClassA;

            }).TotalMilliseconds;

            Trace.WriteLine($"‘Activator.CreateInstance’耗时 {time5} milliseconds");



            double time6 = StopwatchHelper.Calculate(count, () => {

                var a = EmitStateCell<ClassB, ClassA>.CreateObjectHandler.Invoke(new ClassB());
            }).TotalMilliseconds;
            Trace.WriteLine($"Emit Object工厂耗时:{time6}");

            //    /**

            //      #1000000 次调用:

            //        ‘New’耗时 29.3612 milliseconds

            //        ‘Emit 工厂’耗时 634.2714 milliseconds

            //        ‘Expression 工厂’耗时 620.2489 milliseconds

            //        ‘Activator.CreateInstance’耗时 588.0409 milliseconds

            //     * */

            //}

        }

    }
}
