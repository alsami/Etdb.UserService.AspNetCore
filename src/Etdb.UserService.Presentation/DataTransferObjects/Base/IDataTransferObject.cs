namespace Etdb.UserService.Presentation.DataTransferObjects.Base
{
    public interface IDataTransferObject
    {
        string Id { get; set; }

        byte[] ConccurencyToken { get; set; }
    }
}
