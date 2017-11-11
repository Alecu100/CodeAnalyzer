using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo7
{
    public class MyClassWithProperty
    {
        private MyClassForProperty1 _myClassForProperty1;

        private MyClassForProperty2 _myClassForProperty2;

        public MyClassForProperty1 MyClassForProperty1
        {
            get
            {
                return _myClassForProperty1;
            }
            set
            {
                _myClassForProperty1 = value;
            }
        }

        public MyClassForProperty2 MyClassForProperty2
        {
            get
            {
                return _myClassForProperty2;
            }
            set
            {
                _myClassForProperty2 = value;
            }
        }
    }
}
