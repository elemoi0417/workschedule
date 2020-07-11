using System;

namespace workschedule.Controls
{
    /// <summary>
    /// ランダム変数用関数
    /// </summary>
    public static class MyRandom
    {
        // 乱数のSeed値に乱数を使用する
        private static Random random = new Random();
        public static Random Create() => new Random(random.Next());
    }
}
