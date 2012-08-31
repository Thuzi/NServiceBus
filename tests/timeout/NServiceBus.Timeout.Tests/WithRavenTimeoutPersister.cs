namespace NServiceBus.Timeout.Tests
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Hosting.Windows.Persistence;
    using NUnit.Framework;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using Raven.Client.Extensions;

    public class WithRavenTimeoutPersister
    {
        protected IPersistTimeouts persister;
        protected IDocumentStore store;

        [SetUp]
        public void SetupContext()
        {
            Configure.GetEndpointNameAction = () => "MyEndpoint";

            store = new EmbeddableDocumentStore { RunInMemory = true };
            //store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Test" };
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.MonotonicRead;
            store.Conventions.MaxNumberOfRequestsPerSession = 10;
            store.Initialize();
            persister = new RavenTimeoutPersistence(store);
        }

        public List<Tuple<string, DateTime>> GetNextChunk()
        {
            DateTime nextTimeToRunQuery;
            return persister.GetNextChunk(DateTime.UtcNow.AddYears(3), out nextTimeToRunQuery);
        }
    }
}