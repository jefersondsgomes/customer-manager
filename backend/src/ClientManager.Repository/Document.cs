using ClientManager.Repository.Interfaces;
using MongoDB.Bson;
using System;

namespace ClientManager.Repository
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}