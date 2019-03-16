using System;
using System.Text;

namespace StandardDemo {
    public class Entity {

        private string _stringValue = "fuckyou";
        public string GetStringValue() => _stringValue;

        private int _intVal = 12;
        public int GetIntVal() => _intVal;

        private float _floatVal;
        public float GetFloatVal() => _floatVal;

        public string FieldString;

        public string IntVal { get; set; }

        public string ShowString(string s) {
            Console.WriteLine(s);
            return s + "2";
        }
       
    }

    public class Entity2 : Entity {
        public string get_IntVal() => "Dada;";

        private void DO() {
            var s = new StringBuilder();
        }
    }

    //public struct St {
    //    public int 
    //}
}
