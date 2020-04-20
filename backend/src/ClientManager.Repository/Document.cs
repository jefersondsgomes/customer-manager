using System;
using ClientManager.Repository.Interfaces;
using MongoDB.Bson;

namespace ClientManager.Repository
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}