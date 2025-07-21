RavenDB connection string format is:

DataDir - run in embedded mode, specify which directory to run from. This requires that you'll initialize an EmbeddableDocumentStore, not just DocumentStore.

Url - for server mode only, specify where to locate the server.

User / Password - for server mode only, the credentials to use when accessing the server.

Enlist - whatever RavenDB should enlist in distributed transactions. Not applicable for Silverlight.

ResourceManagerId - Optional, for server mode only, the Resource Manager Id that will be used by the Distributed Transaction Coordinator (DTC) service to identify Raven. A custom resource manager id will need to be configured for each Raven server instance when Raven is hosted more than once per machine. Not applicable for Silverlight.

Database - for server mode only, use a specific database, not the default one. Using this will also ensure that the database exists.

The following are samples of a few RavenDB connection strings:

* Url = http://ravendb.mydomain.com
    * connect to a remote RavenDB instance at ravendb.mydomain.com, to the default database
* Url = http://ravendb.mydomain.com;Database=Northwind
    * connect to a remote RavenDB instance at ravendb.mydomain.com, to the Northwind database there
* Url = http://ravendb.mydomain.com;User=user;Password=secret
    * connect to a remote RavenDB instance at ravendb.mydomain.com, with the specified credentials
* DataDir = ~\App_Data\RavenDB;Enlist=False 
    * use embedded mode with the database located in the App_Data\RavenDB folder, without DTC support.


