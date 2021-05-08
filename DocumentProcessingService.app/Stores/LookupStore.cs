﻿using System;
using System.Collections.Generic;

namespace DocumentProcessingService.app.Stores
{
    public interface ILookupStore
    {
        /// <summary> 
        /// Records set of keywords identified in the given document for a given client 
        /// </summary> 
        /// <param name="client">Client identifier</param> 
        /// <param name="documentId">Document identifier</param> 
        /// <param name="keywords">Enumeration of unique keywords found in the document, in any
        /// order. Only match exact words, not prefix match. </param> 
        void Record(string client, string documentId, IEnumerable<string> keywords);
    }

    public class LookupStore : ILookupStore
    {
        public void Record(string client, string documentId, IEnumerable<string> keywords)
        {
            throw new NotImplementedException();
        }
    }
}
