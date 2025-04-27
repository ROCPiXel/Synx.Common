using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synx.Common.Utils
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 交换两个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void Change<T>(ref T A, ref T B)
        {
            (A, B) = (B, A);
        }
    }
}
