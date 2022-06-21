namespace Tengu.Business.Commons.Objects
{
    public class TenguResult<TModel> : TenguResult
    {
        public TModel Data { get; set; } = default!;
    }
    public class TenguResult
    {
        public Hosts Host { get; set; }
        public bool Success { get; set; }
        public TenguException Exception { get; set; } = null!;
    }
}
