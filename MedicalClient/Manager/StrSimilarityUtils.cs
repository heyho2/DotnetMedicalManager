using System;

namespace GD.Utility
{
    /// <summary>
    /// 词相似度 算法
    /// </summary>
    public class StrSimilarityUtils
    {
        private static int Compare(string str, string target)
        {
            int[,] d; // 矩阵

            int n = str.Length;
            int m = target.Length;
            int i; // 遍历str的
            int j; // 遍历target的
            string ch1; // str的
            string ch2; // target的
            int temp; // 记录相同字符,在某个矩阵位置值的增量,不是0就是1

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }
            d = new int[n + 1, m + 1];
            for (i = 0; i <= n; i++)
            { // 初始化第一列
                d[i, 0] = i;
            }

            for (j = 0; j <= m; j++)
            { // 初始化第一行
                d[0, j] = j;
            }

            for (i = 1; i <= n; i++)
            { // 遍历str
                ch1 = str.Substring(i - 1, 1);
                // 去匹配target
                for (j = 1; j <= m; j++)
                {
                    ch2 = target.Substring(j - 1, 1);
                    if (ch1 == ch2)
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }

                    // 左边+1,上边+1, 左上角+temp取最小
                    d[i, j] = Min(d[i - 1, j] + 1, d[i, j - 1] + 1, d[i - 1, j - 1] + temp);
                }
            }

            return d[n, m];
        }
        private static int Min(int one, int two, int three)
        {
            return (one = one < two ? one : two) < three ? one : three;
        }
        /// <summary>
        /// 获取两字符串的相似度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetSimilarityRatio(string str, string target)
        {
            return 1 - (float)Compare(str, target) / Math.Max(str.Length, target.Length);

        }
    }
}
