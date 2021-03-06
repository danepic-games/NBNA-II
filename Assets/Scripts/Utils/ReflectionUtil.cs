using System;
using System.Linq.Expressions;
using Utils;

namespace Util {
    public class ReflectionUtil {
        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess) {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }
    }
}