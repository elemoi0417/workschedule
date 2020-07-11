using System;

namespace workschedule.Functions
{
    class CreateObject
    {
        /// <summary>
        /// COMオブジェクトへの参照を作成および取得
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static object GetCreateObject(string progId, string serverName)
        {
            Type t;
            if (serverName == null || serverName.Length == 0)
                t = Type.GetTypeFromProgID(progId);
            else
                t = Type.GetTypeFromProgID(progId, serverName, true);
            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// COMオブジェクトへの参照を作成および取得(プログラムIDのみ)
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static object GetCreateObject(string progId)
        {
            return GetCreateObject(progId, null);
        }
    }
}
