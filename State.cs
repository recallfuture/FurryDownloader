namespace FurryDownloader
{
    /// <summary>
    /// 状态信息类
    /// </summary>
    public class State
    {
        public StateCode code;
        public string message;

        public State(StateCode code)
        {
            this.code = code;
        }

        public State(StateCode code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum StateCode {
        ok,
        error,
        finish
    }
}
