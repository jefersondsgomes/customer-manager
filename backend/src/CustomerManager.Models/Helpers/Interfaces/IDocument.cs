using System;

namespace CustomerManager.Models.Helpers.Interfaces
{
    public interface IDocument
    {
        string Id { get; set; }
        DateTime Created { get; }
    }
}