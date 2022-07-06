namespace Tengu.Business.Commons.Objects
{
    public class OperationResult<TModel>
    {
        public TModel Data { get; set; } = default!;
        public TenguHosts Host { get; set; }
        public bool Success { get; set; }
        public TenguException Exception { get; set; } = null!;
    }
}
