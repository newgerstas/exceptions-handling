using System.Collections;
using System.Collections.Generic;

namespace ExceptionStrategy.Exceptions
{
    public struct ErrorCode : IEnumerable<string>
    {
        public const string Separator = "-";

        public string Value { get; }

        public ErrorCode(string code)
        {
            this.Value = code;
        }

        public static ErrorCode operator +(ErrorCode left, ErrorCode right) 
            => new ErrorCode(left.Value + Separator + right.Value);

        public static implicit operator ErrorCode(string code)
            => new ErrorCode(code);

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var segment in Value.Split(Separator))
            {
                yield return segment;
            }
        }

        public override string ToString() => Value;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}