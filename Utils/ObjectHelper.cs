namespace Synx.Common.Utils
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 交换两个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Change<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);
        }
    }
}
