namespace Tengu.Business.Commons.Objects
{
    public class TenguResult<TModel> : TenguResult
    {
        public TModel Data { get; set; } = default!;
    }
    public class TenguResult
    {
        public TenguResultInfo[] Infos { get; set; } = null!;
    }

    public class TenguResultInfo
    {
        public TenguHosts Host { get; set; }
        public bool Success { get; set; }
        public TenguException Exception { get; set; } = null!;
    }
}
