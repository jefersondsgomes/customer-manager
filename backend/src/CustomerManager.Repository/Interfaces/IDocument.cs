using System;

namespace CustomerManager.Repository.Interfaces
{
    public interface IDocument
    {
        string Id { get; set; }
        DateTime Created { get; }
    }
}