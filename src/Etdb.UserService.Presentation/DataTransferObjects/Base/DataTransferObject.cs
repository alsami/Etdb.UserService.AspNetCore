namespace Etdb.UserService.Presentation.DataTransferObjects.Base
{
    public abstract class DataTransferObject : IDataTransferObject
    {
        public string Id { get; set; }
        public byte[] ConccurencyToken { get; set; }
    }
}
