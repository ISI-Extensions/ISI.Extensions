docker run --name ravendb -p 8080:8080 -p 38888:38888 -v E:/RavenDb/Data:/opt/RavenDB/Server/RavenData ravendb/ravendb


docker run --name elk -p 5601:5601 -p 9200:9200 -p 5044:5044  -e "discovery.type=single-node" 
-v E:/ELK/Data/elasticsearch/logs:/usr/share/elasticsearch/data 
-v E:/ELK/Data/elasticsearch:/usr/share/elasticsearch/data 
sebp/elk


https://csharp.hotexamples.com/examples/Raven.Client.Document/DocumentStore/ParseConnectionString/php-documentstore-parseconnectionstring-method-examples.html
