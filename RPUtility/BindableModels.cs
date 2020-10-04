using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPUtility
{
    /// <summary>
    /// バインドモデル
    /// </summary>
    [Utility.Developer("tokusan1015")]
    public class Model<T> : BindableBase
    {
        private T _value;
        public T Value
        {
            get { return this._value; }
            set { SetProperty(ref this._value, value); }
        }
    }
}
