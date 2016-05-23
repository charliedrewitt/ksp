using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDKspUtil.UI
{
    public class Parseable<T> where T : IConvertible
    {
        public Parseable(T initialValue)
        {
            _value = initialValue;
            _text = _value.ToString();
        }

        private string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                try
                {
                    if (String.IsNullOrEmpty(_text))
                    {
                        Success = false; return;
                    }

                    _value = (T)Convert.ChangeType(_text, typeof(T));

                    Success = true;
                }
                catch { Success = false; }
            }
        }

        private T _value = default(T);
        public T Value
        {
            get
            {
                return _value;
            }
        }

        public bool Success;
    }
}
