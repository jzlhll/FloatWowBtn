using System;
using System.Collections.Generic;

namespace FloatWowBtn
{
    internal class Utils
    {/// 随机排列数组元素
     /// </summary>
     /// <param name="myList"></param>
     /// <returns></returns>
        public static List<IntPtr> ListRandom(List<IntPtr> myList)
        {

            Random ran = new Random();
            int index;
            IntPtr temp;
            for (int i = 0; i < myList.Count; i++)
            {

                index = ran.Next(0, myList.Count - 1);
                if (index != i)
                {
                    temp = myList[i];
                    myList[i] = myList[index];
                    myList[index] = temp;
                }
            }
            return myList;
        }

        public static bool IsSame(List<IntPtr> list1, List<IntPtr> list2)
        {
            if (list1.Count != list2.Count) {
                return false;
            }

            for (int i = 0; i < list1.Count; i++) {
                if (list1[i] != list2[i]) {
                    return false;
                }
            }
            return true;
        }

        public static List<IntPtr> ListRandomByOffset1(List<IntPtr> myList)
        {
            if (myList == null || myList.Count == 0)
            {
                return myList;
            }

            IntPtr temp = myList[0];
            for (int i = 0; i < myList.Count - 1; i++)
            {
                myList[i] = myList[i + 1];
            }
            myList[myList.Count - 1] = temp;
            return myList;
        }
    }
}
