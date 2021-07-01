using ISO.API.Models;
using System;

namespace ISO.API.Service
{
    public interface IDocumentService
    {
        DocumentListRespDTO GetData(bool type, int folderID, int pageIndex, int pageSize, int userID, int employeeID);

        int SavePublicPermission(int documentId, bool isPublic);
        bool CheckQuickDownload(Guid fileID);
    }
}
